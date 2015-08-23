using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VictoryMenu : BaseMenu 
{
	public Button quitButton;
	public Button resumeButton;
	
	
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


	public void Resume ()
	{
		base.Close ();
		Client.LevelManagerInstance.NextLevel ();
	}
	
	
	public void Awake ()
	{
		quitButton.onClick.AddListener (Quit);
		resumeButton.onClick.AddListener (Resume);
	}
}
