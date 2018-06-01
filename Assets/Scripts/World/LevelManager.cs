using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour 
{
	public void LoadLevel(string levelName)
	{
        Cursor.visible = false;
		SceneManager.LoadScene (levelName);
	}


	public void QuitGame()
	{
		Application.Quit ();
	}
}
