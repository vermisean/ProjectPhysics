using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour 
{
	public AudioSource aSource;

	public AudioClip sfx = null;

	void Start()
	{
		aSource = GetComponent<AudioSource>();
	}

	void OnCollisionEnter(Collision col)
	{
		aSource.PlayOneShot (sfx, 0.25f);
	}
}
