using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour 
{
	public float m_speed = 1.5f;


	void Update () 
	{
		transform.Rotate (Vector3.up, m_speed * Time.deltaTime);
	}
}
