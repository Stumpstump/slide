using UnityEngine;
using System.Collections;

public class Exit : MonoBehaviour 
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.tag == "Player")
		{
			collision.gameObject.SetActive(false);
			Client.UIManagerInstance.OpenMenu("victory");
		}
	}
}
