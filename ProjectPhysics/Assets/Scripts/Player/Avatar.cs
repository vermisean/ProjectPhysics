using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avatar : MonoBehaviour 
{
	public float m_movementSpeed = 0.0f;
	public float m_angularSpeed = 1.57f;		// Measured in radians
	public float m_jumpSpeed = 7.0f;

	private Animation m_anim;
	private Rigidbody m_rb = null;
	private bool isInAir = false;
	private float m_forwardSpeed;
	private float m_backwardSpeed;
	private float m_rightAngularSpeed;
	private float m_leftAngularSpeed;
	private float m_totalSpeed;
	private float m_totalAngularSpeed;
	private Vector3 m_horizontalVelocity;
	private Vector3 m_angularVelocity;
	private Vector3 m_jumpVelocity;

	void Start () 
	{
		m_rb = GetComponent<Rigidbody> ();
		m_anim = GetComponent<Animation> ();
	}
	

	void Update () 
	{

	}


	void FixedUpdate () 
	{
		Move ();
		Animate ();
	}


	void Move()
	{
		// Strafing is transform.right

		// Check to see if in air
		isInAir = Mathf.Abs (m_rb.velocity.y) > 0.001f;

		// Jump
		float jumpSpeed = Input.GetKeyDown (KeyCode.Space) && !isInAir ? m_jumpSpeed : m_rb.velocity.y;
		m_jumpVelocity = jumpSpeed * transform.up;

		// Forward and backward movement
		m_forwardSpeed = Input.GetKey (KeyCode.W) ? m_movementSpeed : 0.0f;
		m_backwardSpeed = Input.GetKey (KeyCode.S) ? -m_movementSpeed : 0.0f;
		m_totalSpeed = m_forwardSpeed + m_backwardSpeed;
		Vector3 flatVelocity = m_rb.velocity;
		flatVelocity.y = 0.0f;
		m_horizontalVelocity = m_totalSpeed * transform.forward;

		m_rb.velocity = m_horizontalVelocity + m_jumpVelocity;

		// Rotation left and right
		m_rightAngularSpeed = Input.GetKey (KeyCode.A) ? -m_angularSpeed : 0.0f;
		m_leftAngularSpeed = Input.GetKey (KeyCode.D) ? m_angularSpeed : 0.0f;
		m_totalAngularSpeed = m_rightAngularSpeed + m_leftAngularSpeed;
		m_angularVelocity = m_totalAngularSpeed * transform.up;
		m_rb.angularVelocity = m_angularVelocity;
	}


	void Animate()
	{
		m_anim["rogue01_idle@loop"].speed = 1.0f;

		if(m_horizontalVelocity.z > 0.01f || m_horizontalVelocity.z < -0.01f) 
		{
			m_anim.CrossFade ("rogue01_run@loop");
			m_anim ["rogue01_run@loop"].speed = 1.0f;
		}
		else
		{
			m_anim.CrossFade ("rogue01_idle@loop");
			m_anim["rogue01_idle@loop"].speed = 1.0f;
		}

		if(isInAir)
		{
			m_anim.CrossFade ("rogue01_jump");
		}
		else
		{
			m_anim.CrossFade ("rogue01_idle@loop");
		}
	}
}
