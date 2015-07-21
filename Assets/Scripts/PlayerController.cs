using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float rollSpeed;
    [SerializeField]
    private float minJumpForce;
    [SerializeField]
    private float maxJumpForce;
    [SerializeField]
    private float maxJumpSetInSeconds;
    [SerializeField]
    private Text percentageText;

    private Rigidbody2D rb;
    private Vector2 rollVector = Vector2.zero;
    private WorldController wc;
    private float radiusForGroundCheck = 0f;
    private float jumpPercentage = 0f;
    private float startJumpTime = 0f;
    private float totalJumpSetTime = 0f;
    private float percentageIncreasePerSecond = 0f;
    private bool isGrounded = false;
    private bool isSettingJumpHeight = false;
    private bool needsToJump = false;

    public Teleporter teleportingTeleporter { get; set; }
    public bool InteractWithTeleporters { get; set; }

    void Awake()
    {
        wc = GameObject.FindGameObjectWithTag("GameController").GetComponent<WorldController>();
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        radiusForGroundCheck = transform.localScale.x / 2 + 0.1f;
        InteractWithTeleporters = true;
    }

    void Update()
    {
        string percentageString = (jumpPercentage * 100f).ToString("0#") + "%";
        percentageText.text = percentageString;

        #region Jumping Logic
#if UNITY_EDITOR

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            isSettingJumpHeight = true;
            jumpPercentage = minJumpForce / maxJumpForce;
            startJumpTime = Time.time;
            totalJumpSetTime = 0f;
            percentageIncreasePerSecond = (1 / maxJumpSetInSeconds) - (minJumpForce / maxJumpForce / maxJumpSetInSeconds);
        }

        if (Input.GetButtonUp("Jump") && isSettingJumpHeight)
        {
            needsToJump = true;
            isSettingJumpHeight = false;
        }
#else
        
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (isGrounded)
                    {
                        isSettingJumpHeight = true;
                        jumpPercentage = minJumpForce / maxJumpForce;
                        startJumpTime = Time.time;
                        totalJumpSetTime = 0f;
                        percentageIncreasePerSecond = (1 / maxJumpSetInSeconds) - (minJumpForce / maxJumpForce / maxJumpSetInSeconds);
                    }
                    break;

                case TouchPhase.Ended:
                    if (isSettingJumpHeight)
                    {
                        needsToJump = true;
                        isSettingJumpHeight = false;
                    }
                    break;

                default:
                    break;
            }
        }
#endif

        if (isSettingJumpHeight && totalJumpSetTime < maxJumpSetInSeconds)
        {
            jumpPercentage += percentageIncreasePerSecond * Time.deltaTime;
            jumpPercentage = Mathf.Clamp(jumpPercentage, minJumpForce / maxJumpForce, 1f);
            totalJumpSetTime = Time.time - startJumpTime;
        }

        #endregion
    }

    void FixedUpdate()
    {
        // Check for grounding
        isGrounded = Physics2D.OverlapCircleAll(transform.position, radiusForGroundCheck, 1 << WorldController.layerPlatforms).Length != 0;

        #region Rolling Handling
        switch (wc.lastOrientation)
        {
            case DeviceOrientation.LandscapeLeft:
            case DeviceOrientation.LandscapeRight:
                rollVector.Set(wc.deviceGravity.x, 0f);
                break;

            case DeviceOrientation.Portrait:
            case DeviceOrientation.PortraitUpsideDown:
                rollVector.Set(0f, wc.deviceGravity.y);
                break;

            default:
                break;
        }

        rollVector = Utils.ZeroIfCloseToZero(rollVector);
        rb.AddForce(rollVector * rollSpeed);
        #endregion

        // Jumping
        if (needsToJump)
        {
            Vector2 force = -wc.deviceGravity * jumpPercentage * maxJumpForce;
            force = Utils.ZeroIfCloseToZero(force);
            rb.AddForce(force, ForceMode2D.Impulse);
            needsToJump = false;
            jumpPercentage = 0f;
        }


    }
}
