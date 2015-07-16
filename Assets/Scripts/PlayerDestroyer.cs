using UnityEngine;
using System.Collections;

public class PlayerDestroyer : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.collider.tag == "Player")
        {
            Destroy(other.gameObject);
        }
    }
}
