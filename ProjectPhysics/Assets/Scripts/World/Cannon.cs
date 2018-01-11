using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour 
{
	public GameObject m_target= null;
	public GameObject m_player = null;
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

	void Update()
	{
		Debug.DrawRay (this.transform.position, this.transform.forward);
	}

	public void FireCannon()
	{
	//	direction = Quaternion.Euler (angle, 0.0f, 0.0f) * this.transform.forward;
	//	direction.Normalize ();

		playerRB.isKinematic = true;
		m_player.transform.position = this.transform.position + new Vector3(-2.0f, 0.0f, 0.0f);
		playerRB.isKinematic = false;

		//playerRB.velocity = calcBallisticVelocityVector(this.transform, m_target.transform, 45.0f);
		playerRB.AddForce(calcBallisticVelocityVector(this.transform, m_target.transform, 45.0f), ForceMode.Impulse);

		//Debug.Log (playerRB.velocity);

		//playerRB.AddForce(transform.forward * force, ForceMode.Impulse);

		//playerRB.AddForce(transform.up * force, ForceMode.Impulse);

		//playerRB.AddTorque(direction, ForceMode.Force);
	}

	public Vector3 calcBallisticVelocityVector(Transform source, Transform target, float angle)
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
}
