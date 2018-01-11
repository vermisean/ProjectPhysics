using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatforms : MonoBehaviour 
{
	public float timeTilFall = 4.5f;
	public float timeTilRespawn = 1.5f;
	public Vector3 startPos;

	private bool isFalling = false;
	private Rigidbody rb = null;


	void Start () 
	{
		rb = GetComponent<Rigidbody> ();
		startPos = this.transform.position;
	}
	

	void Update () 
	{
		
	}

	void OnCollisionEnter(Collision col)
	{
		if(!isFalling && col.gameObject.tag == "Player")
		{
			isFalling = true;
			StartCoroutine (FallWhenTimeOut (timeTilFall, timeTilRespawn));
		}
	}


	IEnumerator FallWhenTimeOut(float timeFall, float timeRespawn)
	{
		yield return new WaitForSeconds (timeFall);
		rb.useGravity = true;
		rb.isKinematic = false;
		yield return new WaitForSeconds (timeRespawn);
		this.transform.position = startPos;
		rb.useGravity = false;
		rb.isKinematic = true;
		isFalling = false;
	}
}
