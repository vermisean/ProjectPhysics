using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Avatar : MonoBehaviour 
{
	public Text m_playerMessage;
	public Text m_playerMessageHighlight;
	public Image m_playerMessageBox;
	public Transform cannonTarget = null;
	public float m_movementForce = 175.0f;
	public float m_sprintForce = 250.0f;
	public float m_jumpForce = 250.0f;
	//public float m_movementSpeed = 0.0f;
	public float m_angularSpeed = 1.57f;		// Measured in radians
	//public float m_jumpSpeed = 7.0f;

	private Animator m_anim;
	private Rigidbody m_rb = null;
	private PhysicMaterial m_physicsMaterial = null;
	private bool m_isInAir = false;
	private bool m_isOnIce = false;
	private bool m_checkPointOne = true;
	private bool m_checkPointTwo = false;
	private bool m_checkPointTwoDone = false;
	private bool m_checkPointThree = false;
	private bool m_checkPointThreeDone = false;
	private float m_forwardSpeed;
	private float m_sprintSpeed;
	private float m_sprintMultiplier = 1.005f;
	private float m_backwardSpeed;
	private float m_rightAngularSpeed;
	private float m_leftAngularSpeed;
	private float m_totalSpeed;
	private float m_totalAngularSpeed;
	private float m_lastVelocity = 0.0f;
	private Vector3 m_horizontalVelocity;
	private Vector3 m_angularVelocity;
	private Vector3 m_jumpVelocity;

	void Start () 
	{
		m_rb = GetComponent<Rigidbody> ();
		m_anim = GetComponent<Animator> ();
		m_physicsMaterial = this.GetComponent<CapsuleCollider> ().material;
		m_playerMessage.text = "";
		m_playerMessageHighlight.text = "";
		m_playerMessageBox.enabled = false;
	}
	

	void Update () 
	{
		UpdateUI ();
	}


	void FixedUpdate () 
	{
		NewMove ();
		Animate ();
	}

	void NewMove()
	{
		// Check to see if in air
		m_isInAir = Mathf.Abs (m_rb.velocity.y) > 0.05f || Mathf.Abs (m_rb.velocity.y) < -0.05f;

		// Jump
		if(Input.GetKeyDown (KeyCode.Space) && !m_isInAir)
			m_rb.AddForce (transform.up * m_jumpForce, ForceMode.Impulse);

		if	(Input.GetKey (KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
			m_rb.AddForce (transform.forward * m_sprintForce, ForceMode.Acceleration);
		else if (Input.GetKey (KeyCode.W))
			m_rb.AddForce (transform.forward * m_movementForce, ForceMode.Acceleration);

		m_rightAngularSpeed = Input.GetKey (KeyCode.A) ? -m_angularSpeed : 0.0f;
		m_leftAngularSpeed = Input.GetKey (KeyCode.D) ? m_angularSpeed : 0.0f;
		m_totalAngularSpeed = m_rightAngularSpeed + m_leftAngularSpeed;
		m_angularVelocity = m_totalAngularSpeed * transform.up;
		m_rb.angularVelocity = m_angularVelocity;
	}

	/*void Move()
	{
		// Strafing is transform.right

		// Check to see if in air
		m_isInAir = Mathf.Abs (m_rb.velocity.y) > 0.1f;

		// Jump
		float jumpSpeed = Input.GetKeyDown (KeyCode.Space) && !m_isInAir ? m_jumpSpeed : m_rb.velocity.y;
		m_jumpVelocity = jumpSpeed * transform.up;

		// Forward and backward movement
		if (!m_isOnIce) 
		{
			m_forwardSpeed = Input.GetKey (KeyCode.W) ? m_movementSpeed : 0.0f;
			m_sprintSpeed = (Input.GetKey (KeyCode.W) && Input.GetKey (KeyCode.LeftShift)) ? (m_movementSpeed * m_sprintMultiplier) : 0.0f;
			m_backwardSpeed = Input.GetKey (KeyCode.S) ? -m_movementSpeed : 0.0f;
			m_totalSpeed = m_forwardSpeed + m_sprintSpeed + m_backwardSpeed;
			m_horizontalVelocity = m_totalSpeed * transform.forward;

			m_rb.velocity = m_horizontalVelocity + m_jumpVelocity;
		}
		else
		{
			m_forwardSpeed = Input.GetKey (KeyCode.W) ? m_movementSpeed : m_lastVelocity;
			m_sprintSpeed = (Input.GetKey (KeyCode.W) && Input.GetKey (KeyCode.LeftShift)) ? (m_movementSpeed * m_sprintMultiplier) : m_lastVelocity;
			m_backwardSpeed = Input.GetKey (KeyCode.S) ? -m_movementSpeed : m_lastVelocity;
			m_totalSpeed = m_forwardSpeed + m_sprintSpeed + m_backwardSpeed;
			m_horizontalVelocity = m_totalSpeed * transform.forward;
			m_lastVelocity = m_horizontalVelocity.z;

			m_rb.velocity = m_horizontalVelocity + m_jumpVelocity;
		}

		// Rotation left and right
		m_rightAngularSpeed = Input.GetKey (KeyCode.A) ? -m_angularSpeed : 0.0f;
		m_leftAngularSpeed = Input.GetKey (KeyCode.D) ? m_angularSpeed : 0.0f;
		m_totalAngularSpeed = m_rightAngularSpeed + m_leftAngularSpeed;
		m_angularVelocity = m_totalAngularSpeed * transform.up;
		m_rb.angularVelocity = m_angularVelocity;
	}*/


	void Animate()
	{
		if(Input.GetKey(KeyCode.LeftShift))
		{
			m_anim.SetFloat ("Speed", 2f);
		}
		else if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
		{
			m_anim.SetFloat ("Speed", 1f);
		}
		else if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
		{
			m_anim.SetFloat ("Speed", 1f);
		}
		else
		{
			m_anim.SetFloat ("Speed", 0f);
		}

		if(Input.GetKeyDown(KeyCode.Space))
		{
			m_anim.SetTrigger ("Jump");
		}

		if(m_isInAir)
		{
			m_anim.SetBool ("isInAir", true);
		}
		else
		{
			m_anim.SetBool ("isInAir", false);
		}
	}


	void UpdateUI()
	{
		if(m_checkPointTwo && !m_checkPointTwoDone)
		{
			m_checkPointTwoDone = true;
			StartCoroutine (UpdateUIText ("Checkpoint Reached!", 3.5f));
		}
		if(m_checkPointThree && !m_checkPointThreeDone)
		{
			m_checkPointThreeDone = true;
			StartCoroutine (UpdateUIText ("Checkpoint Reached!", 3.5f));
		}
	}


	void OnTriggerEnter(Collider col)
	{
		if(col.gameObject.tag == "Ice")
		{
			m_isOnIce = true;
			m_physicsMaterial.staticFriction = 0.0f;
			m_physicsMaterial.dynamicFriction = 0.0f;
			Debug.Log ("Hit ice!");
		}

		if(!m_checkPointTwo && col.gameObject.tag == "Checkpoint02")
		{
			m_checkPointTwo = true;
		}

		if(!m_checkPointThree && col.gameObject.tag == "Checkpoint03")
		{
			m_checkPointThree = true;
		}
	}

	void OnTriggerStay(Collider col)
	{
		if(col.gameObject.tag == "Cannon")
		{
			if(Input.GetKeyDown(KeyCode.E))
			{
				Debug.Log ("In cannon zone!");
				this.gameObject.transform.position = cannonTarget.position;
			}
		}
	}


	void OnTriggerExit(Collider col)
	{
		if(col.gameObject.tag == "Ice")
		{
			m_isOnIce = false;
			m_physicsMaterial.staticFriction = 0.6f;
			m_physicsMaterial.dynamicFriction = 0.6f;
			Debug.Log ("Off ice!");
		}

	}


	IEnumerator UpdateUIText(string text, float timeToWait)
	{
		m_playerMessage.text = text;
		m_playerMessageHighlight.text = text;
		m_playerMessageBox.enabled = true;
		yield return new WaitForSeconds (timeToWait);
		m_playerMessage.text = "";
		m_playerMessageHighlight.text = "";
		m_playerMessageBox.enabled = false;
	}
}
