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

	private Rigidbody playerRB;
	//private float acceleration = Physics.gravity.y;

	void Start()
	{
		playerRB = m_player.GetComponent<Rigidbody> ();

	}

/*	void FixedUpdate()
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
	}*/

	public void FireCannon()
	{
		//playerRB.isKinematic = true;
	//	m_player.transform.position = this.transform.position + new Vector3(-2.0f, 0.0f, 0.0f);
	//	m_player.transform.rotation = this.transform.rotation;
	//	playerRB.isKinematic = false;

	//	playerRB.velocity =  (CalculateBallistics (m_player.transform.position, m_target.transform.position, m_targetHeight));
	//	Debug.Log ("Calculated vel: " + CalculateBallistics (m_player.transform.position, m_target.transform.position, m_targetHeight));

		//playerRB.AddForce (CalculateBallisticsForce (m_player.transform.position, m_target.transform.position, m_targetHeight), ForceMode.Acceleration);
		playerRB.velocity =  (CalculateBallistics (m_player.transform.position, m_target.transform.position, m_targetHeight));
	//	playerRB.velocity =  (calcBallisticVelocityVector (m_player.transform, m_target.transform, 45.0f));
		Debug.Log ("calc vel:" + calcBallisticVelocityVector (m_player.transform, m_target.transform, 45.0f));
		Debug.Log ("Calculated force: " + CalculateBallisticsForce (m_player.transform.position, m_target.transform.position, m_targetHeight));
		Debug.Log ("Player vel: " + playerRB.velocity);
		Debug.Log ("calc myvel:" + (CalculateBallistics (m_player.transform.position, m_target.transform.position, m_targetHeight)));
	//	isFiringTime = true;
	//	isFiring = true;
	}


	Vector3 CalculateBallistics(Vector3 startPoint, Vector3 endPoint, float dispY)
	{
		float dispX = Vector3.Distance (startPoint, endPoint);

		float initVelY = Mathf.Sqrt(-2.0f * Physics.gravity.y * dispX);
		//Debug.Log ("init vel y: " + initVelY);

		time = (0.0f - initVelY) / Physics.gravity.y;
		//Debug.Log ("time: " + time);

		float initVelX = dispX / time;
		//Debug.Log ("init vel x: " + initVelX);

		Vector3 newVelocity = new Vector3(0.0f, initVelY, initVelX);
		return newVelocity;
	}

	Vector3 CalculateBallisticsForce(Vector3 startPoint, Vector3 endPoint, float dispY)
	{
		float dispX = Vector3.Distance (startPoint, endPoint);

		float initVelY = Mathf.Sqrt(-2.0f * Physics.gravity.y * dispX);
		Debug.Log ("init vel y: " + initVelY);

		time = (0.0f - initVelY) / Physics.gravity.y;
		Debug.Log ("time: " + time);

		float initVelX = dispX / time;
		Debug.Log ("init vel x: " + initVelX);

		Vector3 newVelocity = new Vector3(0.0f, initVelY, initVelX);

		Vector3 force = playerRB.mass * (newVelocity / Time.fixedDeltaTime);
		return force;
	}


	Vector3 calcBallisticVelocityVector(Transform source, Transform target, float angle)
	{
		Vector3 direction = target.position - source.position;            // get target direction
		float h = direction.y;                                            // get height difference
		direction.y = 0;                                                // remove height
		float distance = direction.magnitude;                            // get horizontal distance
		float a = angle * Mathf.Deg2Rad;                                // Convert angle to radians
		direction.y = distance * Mathf.Tan(a);                            // Set direction to elevation angle
		distance += h/Mathf.Tan(a);                                        // Correction for small height differences

		// calculate velocity
		float velocity = Mathf.Sqrt(distance * Physics.gravity.magnitude / Mathf.Sin(2*a));
		return velocity * direction.normalized;

	}
		

	IEnumerator TimeCannon()
	{
		yield return new WaitForSeconds (time);
		isFiring = false;
		isFiringTime = false;
	}
}
