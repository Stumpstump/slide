using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float rollSpeed;
    [SerializeField]
    private float maxDeltaForEditorTest;
    // This is for the editor jump
    [SerializeField]
    private float maxJumpInSeconds;
    [SerializeField]
    private float jumpSpeed;
    [SerializeField]
    private WorldController wController;

    private Rigidbody2D rb;
    private Vector2 forceVector = Vector2.zero;
    private float timeSpaceHeld = 0f;
    private bool grounded = false;
    private bool jumping = false;

    [HideInInspector]
    public Teleporter teleportingTeleporter;

    public bool InteractWithTeleporters { get; set; }

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        InteractWithTeleporters = true;
    }



    void FixedUpdate()
    {
        float radiusForGroundCheck = transform.localScale.x / 2 + 0.1f;
        grounded = Physics2D.OverlapCircle(transform.position, radiusForGroundCheck, 1 << WorldController.layerPlatforms) != null;

        switch (wController.lastOrientation)
        {
            case DeviceOrientation.LandscapeLeft:
            case DeviceOrientation.LandscapeRight:
                forceVector.Set(wController.deviceGravity.x, 0f);
                break;

            case DeviceOrientation.Portrait:
            case DeviceOrientation.PortraitUpsideDown:
                forceVector.Set(0f, wController.deviceGravity.y);
                break;

            default:
                break;
        }

        rb.AddForce(forceVector * rollSpeed);

        // This is when there is a sudden jerk
#if UNITY_EDITOR

        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            jumping = true;
            timeSpaceHeld = 0f;
        }


        if (Input.GetKeyUp(KeyCode.Space))
            jumping = false;

        if (jumping && Input.GetKey(KeyCode.Space) && timeSpaceHeld < maxJumpInSeconds)
        {
            rb.AddForce(Vector2.up * jumpSpeed);
            timeSpaceHeld += Time.deltaTime;
        }

#else
        if(grounded)
            if (wController.lastOrientation != DeviceOrientation.FaceUp && wController.lastOrientation != DeviceOrientation.FaceDown)
                rb.AddForce(Input.gyro.userAcceleration * jumpForce);
#endif
    }

    public void SetWorldController(WorldController wc)
    {
        wController = wc;
    }

    IEnumerator EditorJump()
    {
        float totalJumpTime = 0f;
        while (Input.GetKey(KeyCode.Space) && totalJumpTime < maxJumpInSeconds)
        {
            transform.Translate(0f, jumpSpeed * Time.deltaTime, 0f);
            totalJumpTime += totalJumpTime;
            yield return null;
        }
    }
}
