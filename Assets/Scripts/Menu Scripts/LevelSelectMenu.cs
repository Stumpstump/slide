using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LevelSelectMenu : BaseMenu 
{
	public Button backButton;
	public Scrollbar scrollBar;
	public LevelOption levelOptionPrefab;
	public Transform Container;

	public List<LevelOption> levelOptions = new List<LevelOption> ();
	
	public void Open ()
	{
		base.Open ();
	}
	
	
	public void Close ()
	{
		base.Close ();
	}
	
	
	public void Back ()
	{
		base.Close ();
		Client.UIManagerInstance.OpenMenu ("main");
	}
	
	
	public void SelectLevel (int levelToLoad)
	{
		base.Close ();
		Client.LevelManagerInstance.Index = (levelToLoad - 1);
		Client.LevelManagerInstance.NextLevel ();
	}


	public void Initialize()
	{
		int i = 0;
		foreach(Level level in Client.LevelManagerInstance.levels)
		{
			LevelOption option = Instantiate(levelOptionPrefab) as LevelOption;
			option.transform.SetParent(Container, false);
			option.title.text = level.name;
			option.levelToLoad = i;

			levelOptions.Add(option);
			i++;
		}

		for(int j = 0; j < levelOptions.Count; j++)
		{
			int index = levelOptions[j].levelToLoad;
			levelOptions[j].button.onClick.AddListener
				(
					delegate{SelectLevel(index);}
				);
		}

		scrollBar.value = 0;
	}


	public void Start()
	{
		Initialize ();
	}
	
	
	public void Awake ()
	{
		backButton.onClick.AddListener (Back);
	}
}
