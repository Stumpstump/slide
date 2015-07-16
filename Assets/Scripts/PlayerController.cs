using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float rollSpeed;
    [SerializeField]
    private WorldController wController;
    [SerializeField]
    private float maxDeltaForEditorTest;

    private Rigidbody2D rb;
    private Vector2 forceVector = Vector2.zero;

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
        if (wController.lastOrientation != DeviceOrientation.FaceUp)
            rb.AddForce(Input.gyro.userAcceleration * jumpForce);
    }

    public void SetWorldController(WorldController wc)
    {
        wController = wc;
    }
}
