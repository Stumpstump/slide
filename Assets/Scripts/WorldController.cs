using UnityEngine;
using UnityEngine.UI;
using System.Collections;



public class WorldController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Used only to reference object in-code.")]
    private Text customOrientationText;
    [SerializeField]
    [Tooltip("Used only to reference object in-code.")]
    private Text lastOrientationText;
    [SerializeField]
    [Tooltip("Used only to reference object in-code.")]
    private Toggle useOld;
    [SerializeField]
    [Tooltip("A number between 0.0 and 1.0.\nWhen the rotation is higher than this amount, it will change orientation.")]
    private float tiltOrientationChangeThreshold;
    [Tooltip("The force amount below which will not register.\nHigher number means bigger buffer.")]
    public float zeroBuffer;

    // This is for the editor dragging
    [SerializeField]
    [Tooltip("How sensitive the editor dragging is.")]
    private float dragSensitivity;

    private const float gravity = 9.81f;
    private const float defaultCamSize = 8.888888f;
    private Vector2 gravityVector = Vector2.zero;
    private GameObject player;

    // These variables deal with the mouse dragging for editor control
    private Vector2 initialMousePos = Vector2.zero;
    private Vector2 mouseOffset = Vector2.zero;
    private float camRotationAmount = 0f;
    private bool draggingWorld = false;

    [HideInInspector]
    public DeviceOrientation currentOrientation = DeviceOrientation.FaceUp;
    [HideInInspector]
    public DeviceOrientation lastOrientation = DeviceOrientation.FaceUp;
    [HideInInspector]
    public Vector3 deviceGravity;

    public static LayerMask layerPlatforms;


    void Awake()
    {
        Camera.main.orthographicSize = defaultCamSize;
        player = GameObject.FindGameObjectWithTag("Player");
        layerPlatforms = LayerMask.NameToLayer("Platforms");
    }

    void Update()
    {
        Utils.zeroBuffer = this.zeroBuffer;
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            draggingWorld = true;
            initialMousePos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            draggingWorld = false;
        }

        if (draggingWorld)
        {
            mouseOffset = (Vector2)Input.mousePosition - initialMousePos;

            bool draggingHorz = Mathf.Abs(mouseOffset.x) >= Mathf.Abs(mouseOffset.y);

            camRotationAmount = draggingHorz ? mouseOffset.x : mouseOffset.y;
            camRotationAmount *= dragSensitivity;
            camRotationAmount %= 360;
            if ((draggingHorz && initialMousePos.y < (Screen.height / 2)) || (!draggingHorz && initialMousePos.x > (Screen.width / 2)))
                camRotationAmount *= -1;
            Camera.main.transform.Rotate(new Vector3(0f, 0f, camRotationAmount));

            initialMousePos = Input.mousePosition;
        }
#endif
    }

    void FixedUpdate()
    {
#if UNITY_EDITOR
        deviceGravity = GetEditorGravity();
#else
        deviceGravity = Input.gyro.gravity;
#endif
        CheckCurrentOrientation();
        customOrientationText.text = "Custom Orientation Detection: " + currentOrientation.ToString();
        CheckLastOrientation();
        lastOrientationText.text = "Orientation that Physics is Currently Using: " + lastOrientation.ToString();
        SetGravity();
    }

    Vector3 GetEditorGravity()
    {
        float camRot = Camera.main.transform.rotation.eulerAngles.z;
        float x, y;
        if (camRot >= 270)
        {
            x = ((camRot - 270) / 90) - 1;
            y = -((camRot - 270) / 90);
        }

        else if (camRot >= 180)
        {
            x = -((camRot - 180) / 90);
            y = 1 - ((camRot - 180) / 90);
        }

        else if (camRot >= 90)
        {
            x = 1 - ((camRot - 90) / 90);
            y = (camRot - 90) / 90;
        }

        else
        {
            x = camRot / 90;
            y = (camRot / 90) - 1;
        }

        return new Vector3(x, y, 0f);
    }

    // The hardware reading of device orientation is screwy, so this replaces that
    void CheckCurrentOrientation()
    {
#if UNITY_EDITOR
        float camRot = Camera.main.transform.rotation.eulerAngles.z;

        if (camRot >= 315f || camRot < 45)
            currentOrientation = DeviceOrientation.Portrait;
        else if (camRot >= 225f)
            currentOrientation = DeviceOrientation.LandscapeLeft;
        else if (camRot >= 135f)
            currentOrientation = DeviceOrientation.PortraitUpsideDown;
        else
            currentOrientation = DeviceOrientation.LandscapeRight;
#else
        if (deviceGravity.x >= tiltOrientationChangeThreshold)
        {
            if (Mathf.Abs(deviceGravity.x) > Mathf.Abs(deviceGravity.y))
                currentOrientation = DeviceOrientation.LandscapeRight;
            else if (deviceGravity.y < 0)
                currentOrientation = DeviceOrientation.Portrait;
            else
                currentOrientation = DeviceOrientation.PortraitUpsideDown;
        }

        else if (deviceGravity.x <= -tiltOrientationChangeThreshold)
        {
            if (Mathf.Abs(deviceGravity.x) > Mathf.Abs(deviceGravity.y))
                currentOrientation = DeviceOrientation.LandscapeLeft;
            else if (deviceGravity.y < 0)
                currentOrientation = DeviceOrientation.Portrait;
            else
                currentOrientation = DeviceOrientation.PortraitUpsideDown;
        }

        else
        {
            if (deviceGravity.y >= tiltOrientationChangeThreshold)
                currentOrientation = DeviceOrientation.PortraitUpsideDown;
            else if (deviceGravity.y <= -tiltOrientationChangeThreshold)
                currentOrientation = DeviceOrientation.Portrait;
            else if (deviceGravity.z <= 0)
                currentOrientation = DeviceOrientation.FaceUp;
            else
                currentOrientation = DeviceOrientation.FaceDown;
        }
#endif
    }

    // This in effect ignores faceup and facedown orientation
    void CheckLastOrientation()
    {
        if (currentOrientation != lastOrientation)
        {
            if (currentOrientation == DeviceOrientation.LandscapeLeft ||
                currentOrientation == DeviceOrientation.LandscapeRight ||
                currentOrientation == DeviceOrientation.Portrait ||
                currentOrientation == DeviceOrientation.PortraitUpsideDown)
                lastOrientation = currentOrientation;
        }
    }

    void SetGravity()
    {
        // First method
        if (useOld.isOn)
            Physics2D.gravity = (Vector2)deviceGravity * gravity;

        //Second method
        else
        {
            switch (lastOrientation)
            {
                case DeviceOrientation.Portrait:
                    gravityVector.Set(0f, -gravity);
                    break;

                case DeviceOrientation.PortraitUpsideDown:
                    gravityVector.Set(0f, gravity);
                    break;

                case DeviceOrientation.LandscapeRight:
                    gravityVector.Set(gravity, 0f);
                    break;

                case DeviceOrientation.LandscapeLeft:
                    gravityVector.Set(-gravity, 0f);
                    break;

                case DeviceOrientation.FaceDown:
                case DeviceOrientation.FaceUp:
                    break;

                default:
                    break;
            }

            Physics2D.gravity = gravityVector;
        }
    }

    public void CreateNewPlayer()
    {
        Instantiate(player, Vector2.zero, Quaternion.identity);
    }
}
