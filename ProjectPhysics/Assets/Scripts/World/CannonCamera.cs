using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonCamera : MonoBehaviour 
{
	public Transform m_target = null;

	void Start () 
	{
		
	}
	

	void Update () 
	{
		if (m_target) 
		{
			Vector3 targetPosition = new Vector3 (m_target.transform.position.x, this.transform.position.y, m_target.transform.position.z);
			this.transform.LookAt (m_target);
		}
	}
}
