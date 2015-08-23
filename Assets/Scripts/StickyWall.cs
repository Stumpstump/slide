using UnityEngine;
using System.Collections;

public class StickyWall : MonoBehaviour 
{
	private GameObject player;

	public GameObject Player
	{
		get{ return player; }
		set
		{
			player = value;
		}
	}


	private void OnCollisionEnter2D(Collision2D other)
	{
		//need to devise a way to actually get the 
		//player to stick to the wall until the player
		//RELEASES space to jump
		Debug.Log ("object detected + " +other.gameObject.name + " : " + other.gameObject.tag);
	}
}
