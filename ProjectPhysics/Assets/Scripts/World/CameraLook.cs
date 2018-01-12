using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
	public Transform player;
	public float camPosX = 0.0f;
	public float camPosY = 5.0f;
	public float camPosZ = -3.0f;

	public float turnSpeed = 3.0f;

	private Vector3 offset;

	private void Start()
	{
		offset = new Vector3(player.position.x + camPosX, player.position.y + camPosY, player.position.z + camPosZ);
		//transform.rotation = Quaternion.Euler(camRotationX, camRotationY, camRotationZ);
	}


	private void LateUpdate()
	{
		offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * turnSpeed, Vector3.up) * Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * -turnSpeed, Vector3.right) * offset;
		transform.position = player.position + offset;
		transform.LookAt(player.position);
	}
}
