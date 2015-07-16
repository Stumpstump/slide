using UnityEngine;
using UnityEngine.UI;
using System.Collections;



public class WorldController : MonoBehaviour
{
    [SerializeField]
    private Text customOrientationText;
    [SerializeField]
    private Text lastOrientationText;
    [SerializeField]
    private float tiltOrientationChangeThreshold;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private Toggle useOld;

    // This is for the editor dragging
    [SerializeField]
    private float dragSensitivity;

    private readonly float gravity = 9.81f;
    private readonly float screenRatio = 16 / 9;
    private readonly float defaultCamSize = 5f;
    private readonly float editorCamSize = 5 * 16 / 9;
    private Vector2 gravityVector = Vector2.zero;

    // These variables deal with the mouse dragging for editor control
    private bool draggingWorld = false;
    private Vector2 initialMousePos = Vector2.zero;
    private Vector2 mouseOffset = Vector2.zero;
    private float camRotationAmount = 0f;

    [HideInInspector]
    public DeviceOrientation currentOrientation = DeviceOrientation.FaceUp;
    [HideInInspector]
    public DeviceOrientation lastOrientation = DeviceOrientation.FaceUp;
    [HideInInspector]
    public Vector3 deviceGravity;

    void Awake()
    {
#if UNITY_EDITOR
        Camera.main.orthographicSize = 8.888888888f;
#else
        Camera.main.orthographicSize = defaultCamSize;
#endif
    }

    void Update()
    {
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
            camRotationAmount = mouseOffset.x * dragSensitivity;
            camRotationAmount %= 360;
            if (initialMousePos.y < (Screen.height / 2))
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
            y = y = (camRot - 90) / 90;
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
            currentOrientation = DeviceOrientation.LandscapeLeft;
        else if (camRot >= 225f)
            currentOrientation = DeviceOrientation.PortraitUpsideDown;
        else if (camRot >= 135f)
            currentOrientation = DeviceOrientation.LandscapeRight;
        else
            currentOrientation = DeviceOrientation.Portrait;
#else
        // Tilt to portrait
        if (deviceGravity.x >= tiltOrientationChangeThreshold)
        {
            if (Mathf.Abs(deviceGravity.x) > Mathf.Abs(deviceGravity.y))
                currentOrientation = DeviceOrientation.Portrait;
            else if (deviceGravity.y < 0)
                currentOrientation = DeviceOrientation.LandscapeLeft;
            else
                currentOrientation = DeviceOrientation.LandscapeRight;
        }

        // Tilt to portraitupsidedown
        else if (deviceGravity.x <= -tiltOrientationChangeThreshold)
        {
            if (Mathf.Abs(deviceGravity.x) > Mathf.Abs(deviceGravity.y))
                currentOrientation = DeviceOrientation.PortraitUpsideDown;
            else if (deviceGravity.y < 0)
                currentOrientation = DeviceOrientation.LandscapeLeft;
            else
                currentOrientation = DeviceOrientation.LandscapeRight;
        }

        // Landscape or faceup / facedown
        else
        {
            if (deviceGravity.y >= tiltOrientationChangeThreshold)
                currentOrientation = DeviceOrientation.LandscapeRight;
            else if (deviceGravity.y <= -tiltOrientationChangeThreshold)
                currentOrientation = DeviceOrientation.LandscapeLeft;
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
            switch (currentOrientation)
            {
                case DeviceOrientation.LandscapeLeft:
                    gravityVector.Set(0f, -gravity);
                    break;

                case DeviceOrientation.LandscapeRight:
                    gravityVector.Set(0f, gravity);
                    break;

                case DeviceOrientation.Portrait:
                    gravityVector.Set(gravity, 0f);
                    break;

                case DeviceOrientation.PortraitUpsideDown:
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
        GameObject newPlayer = Instantiate(player, Vector2.zero, Quaternion.identity) as GameObject;
        newPlayer.GetComponent<PlayerController>().SetWorldController(this);
    }
}
