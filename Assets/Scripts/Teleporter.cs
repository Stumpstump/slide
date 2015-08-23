using UnityEngine;
using System.Collections;

public class Teleporter : MonoBehaviour
{
    public Transform teleportTo;
    public bool zeroPlayersVelocity = true;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerController controller = other.gameObject.GetComponent<PlayerController>();
            Rigidbody2D otherRB = other.gameObject.GetComponent<Rigidbody2D>();
            if (controller.InteractWithTeleporters == true)
            {
                controller.teleportingTeleporter = this;
                if (zeroPlayersVelocity)
                {
                    otherRB.velocity = Vector2.zero;
                    otherRB.angularVelocity = 0f;
                }

                controller.InteractWithTeleporters = false;

                other.gameObject.transform.position = teleportTo.position;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerController controller = other.gameObject.GetComponent<PlayerController>();
            if (controller.teleportingTeleporter != this)
            {
                controller.InteractWithTeleporters = true;
                controller.teleportingTeleporter = null;
            }
        }
    }
}
