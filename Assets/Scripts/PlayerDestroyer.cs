using UnityEngine;
using System.Collections;

public class PlayerDestroyer : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.collider.tag == "Player")
        {
			other.gameObject.SetActive(false);
			Client.UIManagerInstance.OpenMenu("death");
			//GetComponentInParent<LevelManager> ().Respawn();
        }
    }
}
