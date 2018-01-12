using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilHeads : MonoBehaviour 
{
	public Transform m_target = null;
	public Transform m_projectileSpawner = null;
	public GameObject m_projectilePrefab = null;
	public float m_fireRate = 5.0f;
	public float m_fireForce = 75.0f;
	public Vector3 m_fireOffset = new Vector3 (0.0f, 0.5f, 1.0f);
	public AudioSource m_aSource = null;
	public AudioClip m_attackSFX = null;

	private bool m_playerInRange = false;
	private bool m_isShooting = false;


	void Start () 
	{
		m_aSource = GetComponent<AudioSource> ();
	}
	

	void Update () 
	{
		if (m_target) 
		{
			Vector3 targetPosition = new Vector3 (m_target.transform.position.x, this.transform.position.y, m_target.transform.position.z);
			this.transform.LookAt (m_target);
		}

		if(m_playerInRange && !m_isShooting)
		{
			m_isShooting = true;
			InvokeRepeating ("Shoot", 1.0f, m_fireRate);
		}
	}


	void OnTriggerEnter(Collider col)
	{
		if(col.gameObject.tag == "Player")
		{
			m_playerInRange = true;
		}
	}


	void OnTriggerExit(Collider col)
	{
		if(col.gameObject.tag == "Player")
		{
			m_playerInRange = false;
			m_isShooting = false;
		}
	}


	private void Shoot()
	{
		if(m_isShooting)
		{
			GameObject newProjectile = Instantiate(m_projectilePrefab, m_projectileSpawner.position, m_projectileSpawner.rotation);
			//m_projectilePrefab.GetComponent<Rigidbody>().velocity = (m_target.position - m_target.position).normalized * m_fireSpeed;
			newProjectile.GetComponent<Rigidbody> ().AddForce (this.transform.forward * m_fireForce, ForceMode.Impulse);
			m_aSource.PlayOneShot (m_attackSFX, 0.8f);

			//newProjectile.GetComponent<Rigidbody>().velocity = CalculateVelocity(m_target, 0.0f);
			Destroy(newProjectile, 10.0f);
		}
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
