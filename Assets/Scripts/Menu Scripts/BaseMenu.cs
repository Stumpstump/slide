using UnityEngine;
using System.Collections;

public class BaseMenu : MonoBehaviour 
{
	public string id;
	public GameObject root;
	public bool destroyOnClose;


	public void Open ()
	{
		root.gameObject.SetActive (true);
	}


	public void Close ()
	{
		if(destroyOnClose)
		{
			Client.UIManagerInstance.ExistingMenus.Remove (this);
			Destroy (gameObject);
		}
		else
		{
			root.gameObject.SetActive (false);
		}
	}
}
