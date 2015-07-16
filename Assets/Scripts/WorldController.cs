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
    private float tiltOrientationChangeThreshold = 0.5f;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private Toggle useOld;

    private readonly float gravity = 9.81f;
    private Vector2 gravityVector = Vector2.zero;

    [HideInInspector]
    public DeviceOrientation currentOrientation = DeviceOrientation.FaceUp;
    [HideInInspector]
    public DeviceOrientation lastOrientation = DeviceOrientation.FaceUp;

    void FixedUpdate()
    {
        CheckCurrentOrientation();
        customOrientationText.text = "Custom Orientation Detection: " + currentOrientation.ToString();
        CheckLastOrientation();
        lastOrientationText.text = "Orientation that Physics is Currently Using: " + lastOrientation.ToString();
        SetGravity();
    }

    // The hardware reading of device orientation is screwy, so this replaces that
    void CheckCurrentOrientation()
    {
        // Tilt to portrait
        if (Input.gyro.gravity.x >= tiltOrientationChangeThreshold)
        {
            if (Mathf.Abs(Input.gyro.gravity.x) > Mathf.Abs(Input.gyro.gravity.y))
                currentOrientation = DeviceOrientation.Portrait;
            else if (Input.gyro.gravity.y < 0)
                currentOrientation = DeviceOrientation.LandscapeLeft;
            else
                currentOrientation = DeviceOrientation.LandscapeRight;
        }

        // Tilt to portraitupsidedown
        else if (Input.gyro.gravity.x <= -tiltOrientationChangeThreshold)
        {
            if (Mathf.Abs(Input.gyro.gravity.x) > Mathf.Abs(Input.gyro.gravity.y))
                currentOrientation = DeviceOrientation.PortraitUpsideDown;
            else if (Input.gyro.gravity.y < 0)
                currentOrientation = DeviceOrientation.LandscapeLeft;
            else
                currentOrientation = DeviceOrientation.LandscapeRight;
        }

        // Landscape or faceup / facedown
        else
        {
            if (Input.gyro.gravity.y >= tiltOrientationChangeThreshold)
                currentOrientation = DeviceOrientation.LandscapeRight;
            else if (Input.gyro.gravity.y <= -tiltOrientationChangeThreshold)
                currentOrientation = DeviceOrientation.LandscapeLeft;
            else if (Input.gyro.gravity.z < 0)
                currentOrientation = DeviceOrientation.FaceUp;
            else
                currentOrientation = DeviceOrientation.FaceDown;
        }
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
            Physics2D.gravity = (Vector2)Input.gyro.gravity * gravity;

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
