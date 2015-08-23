using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public void ButtonClicked()
    {
        Application.LoadLevel(Application.loadedLevel + 1);
    }
}
