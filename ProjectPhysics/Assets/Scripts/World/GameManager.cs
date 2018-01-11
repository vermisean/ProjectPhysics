using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour 
{
	public Camera introCam = null;
	public Camera mainCam = null;
	public Camera cannonCam = null;


	void Start () 
	{
		introCam.enabled = true;
		mainCam.enabled = false;
		cannonCam.enabled = false;
		StartCoroutine (WaitForIntro ());
	}


	IEnumerator WaitForIntro()
	{
		yield return new WaitForSeconds (2.83f);
		introCam.enabled = false;
		mainCam.enabled = true;
	}


	public IEnumerator WaitForCannon()
	{
		mainCam.enabled = false;
		cannonCam.enabled = true;
		yield return new WaitForSeconds (4.0f);
		cannonCam.enabled = false;
		mainCam.enabled = true;
	}
}
