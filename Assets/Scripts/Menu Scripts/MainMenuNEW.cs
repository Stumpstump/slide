using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuNEW : BaseMenu 
{
	public Button quitButton;
	public Button playButton;
	public Button levelSelectButton;
	
	
	public void Open ()
	{
		base.Open ();
	}
	
	
	public void Close ()
	{
		base.Close ();
	}
	
	
	public void Quit ()
	{
		Application.Quit ();
	}


	public void LevelSelect ()
	{
		base.Close ();
		Client.UIManagerInstance.OpenMenu("select");
	}
	
	
	public void Play ()
	{
		base.Close ();
		Client.LevelManagerInstance.NextLevel ();
	}
	
	
	public void Awake ()
	{
		quitButton.onClick.AddListener (Quit);
		playButton.onClick.AddListener (Play);
		levelSelectButton.onClick.AddListener (LevelSelect);
	}
}