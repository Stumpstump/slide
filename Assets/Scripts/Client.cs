using UnityEngine;
using System.Collections;


public class Client : MonoBehaviour 
{
	public static LevelManager LevelManagerInstance;
	public static UIManager UIManagerInstance;

	public LevelManager levelManager;
	public UIManager uiManager;

	private void Awake () 
	{
		LevelManagerInstance = levelManager;
		UIManagerInstance = uiManager;
		GameObject.DontDestroyOnLoad (this.gameObject);

		UIManagerInstance.OpenMenu("main");
	}
}
