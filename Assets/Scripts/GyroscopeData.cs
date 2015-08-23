using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GyroscopeData : MonoBehaviour
{
    [SerializeField]
    private Text attitudeText;
    [SerializeField]
    private Text enabledText;
    [SerializeField]
    private Text gravityText;
    [SerializeField]
    private Text rotationRateText;
    [SerializeField]
    private Text rotationRateUnbiasedText;
    [SerializeField]
    private Text updateIntervalText;
    [SerializeField]
    private Text userAccelerationText;
    [SerializeField]
    private Text inputAccelerationText;
    [SerializeField]
    private Text inputAccelerationEventCountText;
    [SerializeField]
    private Text inputDeviceOrientationText;
    [SerializeField]
    private Text physicsEngineGravityText;

    void Awake()
    {
        // Enable the gyro and make sure the screen never times out
        Input.gyro.enabled = true;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    void Update()
    {
        attitudeText.text = "Attitude: " + Input.gyro.attitude.ToString();
        enabledText.text = "Enabled: " + Input.gyro.enabled.ToString();
        gravityText.text = "Gravity: " + Input.gyro.gravity.ToString();
        rotationRateText.text = "Rotation Rate: " + Input.gyro.rotationRate.ToString();
        rotationRateUnbiasedText.text = "Rotation Rate Unbiased: " + Input.gyro.rotationRateUnbiased.ToString();
        updateIntervalText.text = "Update Interval: " + Input.gyro.updateInterval.ToString();
        userAccelerationText.text = "User Acceleration: " + Input.gyro.userAcceleration.ToString();
        inputAccelerationText.text = "Input.acceleration: " + Input.acceleration.ToString();
        inputAccelerationEventCountText.text = "Input.accelerationEventCount: " + Input.accelerationEventCount.ToString();
        inputDeviceOrientationText.text = "Device Orientation: " + Input.deviceOrientation.ToString();
        physicsEngineGravityText.text = "Physics Engine Gravity: " + Physics2D.gravity.ToString();
    }
}
