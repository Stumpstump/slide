using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour 
{
	public GameObject playerPrefab;
	public Level currentLevel;
	public List<Level> levels = new List<Level> ();
	
	private int index = -1;

	private GameObject player;
	private UIManager uiManager;

	public UIManager UIManagerReference
	{
		get{ return uiManager; }
		set{ uiManager = value; }
	}

	public int Index
	{
		get{ return index; }
		set{ index = value; }
	}

	public GameObject Player
	{
		get{ return player; }
		set
		{
			player = value;
			player.transform.position = currentLevel.spawnPoint.position;
		}
	}


	public void Respawn()
	{
		if(player == null) player = Instantiate (playerPrefab) as GameObject;

		player.SetActive(true);
		player.transform.position = currentLevel.spawnPoint.position;
	}


	public void NextLevel()
	{
		index++;



		if(currentLevel != null) currentLevel.root.SetActive(false);
		currentLevel = levels[index];

		currentLevel.root.SetActive(true);

		Respawn();
	}
}
