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
    private Teleporter teleportingTeleporter;

    public bool InteractWithTeleporters { get; set; }

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        InteractWithTeleporters = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Teleporter" && InteractWithTeleporters == true)
        {
            teleportingTeleporter = other.gameObject.GetComponent<Teleporter>();
            if (other.gameObject.GetComponent<Teleporter>().zeroPlayersVelocity)
            {
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }

            InteractWithTeleporters = false;

            transform.position = other.gameObject.GetComponent<Teleporter>().teleportTo.position;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Teleporter" && teleportingTeleporter != other.gameObject.GetComponent<Teleporter>())
        {
            InteractWithTeleporters = true;
            teleportingTeleporter = null;
        }
    }

    void FixedUpdate()
    {
#if UNITY_EDITOR
        transform.position = Vector2.MoveTowards(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), maxDeltaForEditorTest);
#else
        switch (wController.lastOrientation)
        {
            case DeviceOrientation.LandscapeLeft:
            case DeviceOrientation.LandscapeRight:
                forceVector.Set(Input.gyro.gravity.x, 0f);
                break;

            case DeviceOrientation.Portrait:
            case DeviceOrientation.PortraitUpsideDown:
                forceVector.Set(0f, Input.gyro.gravity.y);
                break;

            default:
                break;
        }

        rb.AddForce(forceVector * rollSpeed);

        // This is when there is a sudden jerk
        if (wController.lastOrientation != DeviceOrientation.FaceUp)
            rb.AddForce(Input.gyro.userAcceleration * jumpForce);
#endif
    }

    public void SetWorldController(WorldController wc)
    {
        wController = wc;
    }
}
