using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Avatar : MonoBehaviour 
{
	public GameManager gameManager;
	public Cannon cannonScript;
	public Text m_playerMessage;
	public Text m_playerMessageHighlight;
	public Image m_playerMessageBox;
	public Text m_coinText;
	public Text m_coinTextHighlight;
	public Transform cannonTarget = null;
	public float m_movementForce = 175.0f;
	public float m_sprintForce = 250.0f;
	public float m_jumpForce = 250.0f;
	public float m_angularSpeed = 1.57f;		// Measured in radians
	public GameObject m_checkPointOneRespawn = null;
	public GameObject m_checkPointTwoRespawn = null;
	public GameObject m_checkPointTwoFire = null;
	public GameObject m_checkPointThreeRespawn = null;
	public GameObject m_checkPointThreeFire = null;
	public ParticleSystem m_respawnParticles = null;
	public ParticleSystem m_cannonParticles = null;

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
	private float m_forwardSpeed;
	private float m_sprintSpeed;
	private float m_sprintMultiplier = 1.005f;
	private float m_backwardSpeed;
	private float m_rightAngularSpeed;
	private float m_leftAngularSpeed;
	private float m_totalSpeed;
	private float m_totalAngularSpeed;
	private float m_lastVelocity = 0.0f;
	private int m_coinCounter;
	private Vector3 m_horizontalVelocity;
	private Vector3 m_angularVelocity;
	private Vector3 m_jumpVelocity;

	void Start () 
	{
		gameManager = FindObjectOfType<GameManager> ();
		cannonScript = GameObject.Find ("Cannon").GetComponent<Cannon> ();

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
			Move ();
		}

		Animate ();
	}

	void Move()
	{
		// Check to see if in air
		m_isInAir = Mathf.Abs (m_rb.velocity.y) > 0.05f || Mathf.Abs (m_rb.velocity.y) < -0.05f;

		// Jump
		if(Input.GetKeyDown (KeyCode.Space) && !m_isInAir)
			m_rb.AddForce (transform.up * m_jumpForce, ForceMode.Impulse);

		// Movement
		if	(Input.GetKey (KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
			m_rb.AddForce (transform.forward * m_sprintForce, ForceMode.Acceleration);
		else if (Input.GetKey (KeyCode.W))
			m_rb.AddForce (transform.forward * m_movementForce, ForceMode.Acceleration);
		else if (Input.GetKey (KeyCode.S))
			m_rb.AddForce (transform.forward * -m_movementForce, ForceMode.Acceleration);

		m_rightAngularSpeed = Input.GetKey (KeyCode.A) ? -m_angularSpeed : 0.0f;
		m_leftAngularSpeed = Input.GetKey (KeyCode.D) ? m_angularSpeed : 0.0f;
		m_totalAngularSpeed = m_rightAngularSpeed + m_leftAngularSpeed;
		m_angularVelocity = m_totalAngularSpeed * transform.up;
		m_rb.angularVelocity = m_angularVelocity;
	}


	void Animate()
	{
		if (m_canMove) 
		{
			if (Input.GetKey (KeyCode.LeftShift)) 
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
	}


	void OnTriggerStay(Collider col)
	{
		if(col.gameObject.tag == "Cannon")
		{
			if(!m_cannonFired && Input.GetKeyDown(KeyCode.E))
			{
				m_cannonFired = true;
				m_cannonParticles.Play ();
				gameManager.StartCoroutine("WaitForCannon");

				m_rb.AddForce(transform.forward * 500.0f, ForceMode.Impulse);

				m_rb.AddForce(transform.up * 500.0f, ForceMode.Impulse);

				//cannonScript.FireCannon ();
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


	//IEnumerator LaunchCannon()
	//{
		
	//}
}
