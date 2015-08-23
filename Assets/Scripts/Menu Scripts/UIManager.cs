using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviour 
{
	public Transform menuRoot;
	public List<BaseMenu> menuPrefabs = new List<BaseMenu> ();

	private List<BaseMenu> existingMenus = new List<BaseMenu> ();

	public List<BaseMenu> ExistingMenus
	{
		set { existingMenus = value; }
		get { return existingMenus; }
	}


	public void OpenMenu (string id)
	{
		//locate the menu we are talking about.
		foreach (BaseMenu menu in menuPrefabs)
		{
			if (menu.id == id)
			{
				//found the menu, now to check if it exists. 
				//if it does we just need to turn it on.

				foreach (BaseMenu m in existingMenus)
				{
					if (m.id == id)
					{
						m.Open ();
						return;
					}
				}

				//menu not found - we need to build a new one.
				BaseMenu newMenu = Instantiate (menu) as BaseMenu;
				newMenu.transform.SetParent (menuRoot, false);

				existingMenus.Add (newMenu);
				newMenu.Open ();
				return;
			}
		}

		Debug.LogError ("No menu with that Id found.");
	}


	public void CloseMenu (string id)
	{
		foreach (BaseMenu menu in existingMenus)
		{
			if (menu.id == id)
			{
				menu.Close ();
			}
		}
	}
}
