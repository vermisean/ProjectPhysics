using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour 
{
	public Transform m_target;
	public float m_rotationSpeed = 100f;
	public float m_orbitDegrees = 1f;

	void Update () 
	{
		transform.Rotate(Vector3.up, m_rotationSpeed * Time.deltaTime);
		transform.RotateAround(m_target.position, Vector3.up, m_orbitDegrees);
	}
}
