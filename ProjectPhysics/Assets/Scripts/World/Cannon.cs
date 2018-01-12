using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour 
{
	public GameObject m_target= null;
	public GameObject m_player = null;
	public float m_targetHeight = 5.0f;
	public float time = 0.0f;

	private bool isFiringTime = false;
	private bool isFiring = false;


	public float speed = 5.0f;
	public float angle = 45.0f;
	//public float torque = 50.0f;

	private Rigidbody playerRB;
	private float acceleration = Physics.gravity.y;
	private float finalVelocity = 0.0f;
	private Vector3 direction;

	void Start()
	{
		playerRB = m_player.GetComponent<Rigidbody> ();

	}

	void FixedUpdate()
	{
		if (isFiringTime) 
		{
			if (isFiring) 
			{
				playerRB.velocity = CalculateBallistics (this.transform.position, m_target.transform.position, m_targetHeight);
			} 
			else 
			{
				//playerRB.velocity = Vector3.zero;
			}
		}
	}

	public void FireCannon()
	{
		playerRB.isKinematic = true;
		m_player.transform.position = this.transform.position + new Vector3(-2.0f, 0.0f, 0.0f);
		m_player.transform.rotation = this.transform.rotation;
		playerRB.isKinematic = false;

		isFiringTime = true;
		isFiring = true;
	}


	Vector3 CalculateBallistics(Vector3 startPoint, Vector3 endPoint, float dispY)
	{
		float dispX = Vector3.Distance (startPoint, endPoint);

		float initVelY = Mathf.Sqrt(-2.0f * Physics.gravity.y * dispX);
		Debug.Log ("init vel y: " + initVelY);

		time = (0.0f - initVelY) / Physics.gravity.y;
		Debug.Log ("time: " + time);

		float initVelX = dispX / time;
		Debug.Log ("init vel x: " + initVelX);

		return new Vector3(0.0f, initVelY, initVelX);
	}

	IEnumerator TimeCannon()
	{
		yield return new WaitForSeconds (time);
		isFiring = false;
		isFiringTime = false;
	}
}
