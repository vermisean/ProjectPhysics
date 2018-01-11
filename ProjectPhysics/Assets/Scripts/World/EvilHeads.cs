using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilHeads : MonoBehaviour 
{
	public Transform m_target = null;
	public Transform m_projectileSpawner = null;
	public GameObject m_projectilePrefab = null;
	public float m_fireRate = 3.5f;
	public float m_fireForce = 200.0f;
	public Vector3 m_fireOffset = new Vector3 (0.0f, 0.5f, 1.0f);

	private bool m_playerInRange = false;
	private bool m_isShooting = false;


	void Start () {
		
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

			//newProjectile.GetComponent<Rigidbody>().velocity = CalculateVelocity(m_target, 0.0f);
			Destroy(newProjectile, 10.0f);
		}
	}

	private Vector3 CalculateVelocity(Transform target, float angle)
	{
			Vector3 direction = target.position - transform.position;  // get target direction
			float h = direction.y;  // get height difference
			direction.y = 0;  // retain only the horizontal direction
			float dist = direction.magnitude ;  // get horizontal distance
			float a = angle * Mathf.Deg2Rad;  // convert angle to radians
			direction.y = dist * Mathf.Tan(a);  // set dir to the elevation angle
			dist += h / Mathf.Tan(a);  // correct for small height differences
			// calculate the velocity magnitude
			float vel = Mathf.Sqrt(dist * Physics.gravity.magnitude / Mathf.Sin(2 * a));
			return vel * direction.normalized;
	}
}
