﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Avatar : MonoBehaviour 
{
	[Header ("Scripts")]
	[Space(2)]
	public GameManager m_gameManager;
	public Cannon m_cannonScript;
	[Space(10)]

	[Header ("UI")]
	[Space(2)]
	public Text m_playerMessage;
	public Text m_playerMessageHighlight;
	public Image m_playerMessageBox;
	public Text m_coinText;
	public Text m_coinTextHighlight;
	[Space(10)]

	[Header ("Cannon")]
	[Space(2)]
	public Transform cannonTarget = null;
	public float m_time = 3.5f;

	private bool m_inCannonZone = false;
	[Space(10)]

	[Header ("Movement")]
	[Space(2)]
	public float m_movementSpeed = 5.7f;
	public float m_sprintMultiplier = 1.005f;
	public float m_jumpSpeed = 9.0f;
	public float m_angularSpeed = 1.57f;		// Measured in radians
	[Space(10)]

	[Header ("Checkpoints")]
	[Space(2)]
	public GameObject m_checkPointOneRespawn = null;
	public GameObject m_checkPointTwoRespawn = null;
	public GameObject m_checkPointTwoFire = null;
	public GameObject m_checkPointThreeRespawn = null;
	public GameObject m_checkPointThreeFire = null;
	[Space(10)]

	[Header ("Particle Systems")]
	[Space(2)]
	public ParticleSystem m_respawnParticles = null;
	public ParticleSystem m_cannonParticles = null;
	[Space(10)]

	private Animator m_anim;
	private Rigidbody m_rb = null;
	private PhysicMaterial m_physicsMaterial = null;
	private bool m_canMove = false;
	private bool m_isAlive = true;
	private bool m_isDead = false;
	private bool m_isInAir = false;
	private bool m_isOnIce = false;
	private bool m_cannonFired = false;
	private bool m_boxTutorialComplete = false;
	private bool m_boxTutorialDone = false;
	private bool m_cannonTutorialComplete = false;
	private bool m_cannonTutorialDone = false;
	private bool m_checkPointOne = true;
	private bool m_checkPointTwo = false;
	private bool m_checkPointTwoDone = false;
	private bool m_checkPointThree = false;
	private bool m_checkPointThreeDone = false;
	private int m_coinCounter;
	private Vector3 m_horizontalVelocity;
	private Vector3 m_angularVelocity;
	private Vector3 m_jumpVelocity;


	void Start () 
	{
		m_gameManager = FindObjectOfType<GameManager> ();

		m_rb = GetComponent<Rigidbody> ();
		m_anim = GetComponent<Animator> ();
		m_physicsMaterial = this.GetComponent<CapsuleCollider> ().material;
		m_playerMessage.text = "";
		m_playerMessageHighlight.text = "";
		m_playerMessageBox.enabled = false;
		m_checkPointTwoFire.SetActive(false);
		m_checkPointThreeFire.SetActive(false);

		m_coinCounter = 0;

		StartCoroutine (StartGame ());
	}
	

	void Update () 
	{
		UpdateUI ();
	}


	void FixedUpdate () 
	{
		CheckLife ();

		if (m_canMove) 
		{
			MovePhysics ();
			Animate ();
		}


	if (m_inCannonZone && Input.GetKeyDown (KeyCode.E)) {
			m_cannonFired = true;
			m_cannonParticles.Play ();

			m_canMove = false;
			m_rb.velocity = CalculateBallistics (this.transform.position, cannonTarget.position, 3.5f);
			//m_gameManager.WaitForCannon ();
			m_gameManager.StartCoroutine ("WaitForCannon");
			StartCoroutine (CannonTime ());
		}
	}


	void MovePhysics()
	{
		bool isInAir = Mathf.Abs(m_rb.velocity.y) > 0.02f;
		float jumpSpeed = Input.GetKeyDown(KeyCode.Space) && !isInAir ? m_jumpSpeed : m_rb.velocity.y;
		Vector3 jumpVelocity = jumpSpeed * transform.up;

		float sprintSpeed = Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift) ? m_movementSpeed * m_sprintMultiplier : 0.0f;
		float forwardSpeed = Input.GetKey(KeyCode.W) ? m_movementSpeed : 0.0f;
		float backwardSpeed = Input.GetKey(KeyCode.S) ? -m_movementSpeed : 0.0f;
		float totalSpeed = forwardSpeed + sprintSpeed + backwardSpeed;

		Vector3 horizontalVelocity = totalSpeed * transform.forward;
		m_rb.velocity = horizontalVelocity + jumpVelocity;

		float rightAngularSpeed = Input.GetKey(KeyCode.D) ? m_angularSpeed : 0.0f;
		float leftAngularSpeed = Input.GetKey(KeyCode.A) ? -m_angularSpeed : 0.0f;
		float totalAngularSpeed = rightAngularSpeed + leftAngularSpeed;
		Vector3 angularVelocity = totalAngularSpeed * transform.up;
		m_rb.angularVelocity = angularVelocity;
	}


	void Animate()
	{
		if (m_canMove) 
		{
			if (Input.GetKey (KeyCode.W) && Input.GetKey (KeyCode.LeftShift)) 
			{
				m_anim.SetFloat ("Speed", 2f);
			} 
			else if (Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.S)) 
			{
				m_anim.SetFloat ("Speed", 1f);
			} 
			else if (Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.D)) 
			{
				m_anim.SetFloat ("Speed", 1f);
			} 
			else
			{
				m_anim.SetFloat ("Speed", 0f);
			}

			if (Input.GetKeyDown (KeyCode.Space)) 
			{
				m_anim.SetTrigger ("Jump");
			}

			if (m_isInAir) 
			{
				m_anim.SetBool ("isInAir", true);
			} 
			else 
			{
				m_anim.SetBool ("isInAir", false);
			}
		}
	}


	void UpdateUI()
	{
		if(m_checkPointTwo && !m_checkPointTwoDone)
		{
			m_checkPointTwoDone = true;
			StartCoroutine (UpdateUIText ("Checkpoint Reached!", 3.5f, false));
		}

		if(m_checkPointThree && !m_checkPointThreeDone)
		{
			m_checkPointThreeDone = true;
			StartCoroutine (UpdateUIText ("Checkpoint Reached!", 3.5f, false));
		}

		if(m_boxTutorialComplete && !m_boxTutorialDone)
		{
			m_boxTutorialDone = true;
			StartCoroutine (UpdateUIText ("You can push crates by walking into them", 5.0f, false));
		}
			
		if(m_cannonTutorialComplete && !m_cannonTutorialDone)
		{
			m_cannonTutorialDone = true;
			StartCoroutine (UpdateUIText ("Interact with the cannon using 'E'", 5.0f, false));
		}
	}


	void CheckLife()
	{
		if(!m_isAlive && !m_isDead)
		{
			m_isDead = true;
			m_canMove = false;
			m_isInAir = false;
			m_anim.SetBool ("isInAir", false);
			m_anim.SetTrigger ("Death");
			StartCoroutine (UpdateUIText ("You died!", 3.7f, false));
		}
	}


	void OnTriggerEnter(Collider col)
	{
		if(col.gameObject.tag == "Respawn")
		{
			m_isAlive = false;
			StartCoroutine (Death(3.5f));
		}

		if(col.gameObject.tag == "Coin")
		{
			m_coinCounter++;
			Destroy (col.gameObject);
			m_coinText.text = "Coins: " + m_coinCounter.ToString() + "/75";
			m_coinTextHighlight.text = "Coins: " + m_coinCounter.ToString() + "/75";
		}

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
			m_checkPointTwoFire.SetActive(true);
		}

		if(!m_checkPointThree && col.gameObject.tag == "Checkpoint03")
		{
			m_checkPointThree = true;
			m_checkPointThreeFire.SetActive(true);
		}

		if(!m_boxTutorialComplete && col.gameObject.tag == "BoxTutorial")
		{
			m_boxTutorialComplete = true;
		}

		if(!m_cannonTutorialComplete && col.gameObject.tag == "Cannon")
		{
			m_cannonTutorialComplete = true;
		}

		if(col.gameObject.tag == "Cannon")
		{
			m_inCannonZone = true;
		}
	}


	void OnTriggerStay(Collider col)
	{
		if(col.gameObject.tag == "Cannon")
		{
			if(!m_cannonFired && Input.GetKeyDown(KeyCode.E))
			{
				m_cannonFired = true;
				m_cannonParticles.Play ();
				m_gameManager.StartCoroutine("WaitForCannon");

				//m_rb.AddForce(transform.forward * 500.0f, ForceMode.VelocityChange);

				//m_rb.AddForce(transform.up * 500.0f, ForceMode.VelocityChange);

				m_cannonScript.FireCannon ();
				//this.gameObject.transform.position = cannonTarget.position;
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

		if(col.gameObject.tag == "Cannon")
		{
			m_inCannonZone = false;
		}
	}


	void OnCollisionEnter(Collision col)
	{
		if(col.gameObject.tag == "Projectile")
		{
			m_isAlive = false;
			StartCoroutine (Death(3.5f));
		}
	}


	IEnumerator UpdateUIText(string text, float timeToWait, bool isBoxEnabled)
	{
		m_playerMessage.text = text;
		m_playerMessageHighlight.text = text;
		if(isBoxEnabled)
		{
			m_playerMessageBox.enabled = true;
		}
		yield return new WaitForSeconds (timeToWait);
		m_playerMessage.text = "";
		m_playerMessageHighlight.text = "";
		if(isBoxEnabled)
		{
			m_playerMessageBox.enabled = false;
		}
	}


	IEnumerator Death(float timeToRespawn)
	{
		yield return new WaitForSeconds (timeToRespawn);

		m_anim.SetTrigger ("Respawn");

		if(m_checkPointThree)
		{
			this.transform.position = m_checkPointThreeRespawn.transform.position;
			m_respawnParticles.Play ();

		}
		else if(m_checkPointTwo)
		{
			this.transform.position = m_checkPointTwoRespawn.transform.position;
			m_respawnParticles.Play ();

		}
		else
		{
			this.transform.position = m_checkPointOneRespawn.transform.position;
			m_respawnParticles.Play ();

		}

		yield return new WaitForSeconds (2.0f);
		m_canMove = true;
		m_isAlive = true;
		m_isDead = false;
	}


	IEnumerator StartGame()
	{
		yield return new WaitForSeconds (2.0f);
		m_canMove = true;
	}


	IEnumerator CannonTime()
	{
		yield return new WaitForSeconds (5.1f);
		m_canMove = true;
	}


	Vector3 CalculateBallistics(Vector3 startPoint, Vector3 endPoint, float dispY)
	{
		float dispX = Vector3.Distance (startPoint, endPoint);

		float initVelY = Mathf.Sqrt(-2.0f * Physics.gravity.y * dispX);
		//Debug.Log ("init vel y: " + initVelY);

		float time = (0.0f - initVelY) / Physics.gravity.y;
		//Debug.Log ("time: " + time);

		float initVelX = dispX / time;
		//Debug.Log ("init vel x: " + initVelX);

		Vector3 newVelocity = new Vector3(-initVelX, initVelY, 0.0f);
		return newVelocity;
	}
}
