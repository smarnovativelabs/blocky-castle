using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public Transform player;
//	public Transform child;
	// Use this for initialization
	void Start ()
	{
		targetRott = transform.rotation;
		targetRot = player.eulerAngles;
//		child = transform.GetChild (0);
	}
	
	// Update is called once per frame
	Vector3 targetPos;
	public Vector3 targetRot;
	Quaternion targetRott;
	void LateUpdate () 
	{
		targetPos = new Vector3 (transform.position.x, player.position.y + 1, transform.position.z);
		transform.position = Vector3.Lerp (transform.position, targetPos, 0.05f);
//		print (Vector3.Distance (transform.eulerAngles, targetRot));
//		if (Vector3.Distance (transform.eulerAngles, targetRot) > 50)
//		{
//			transform.eulerAngles = Vector3.Lerp (transform.eulerAngles, targetRot, 0.01f);
//		}

		targetRot.y= player.eulerAngles.y;
		targetRott.eulerAngles = targetRot;
//		transform.eulerAngles = Vector3.Lerp (transform.eulerAngles, targetRot, 0.05f);
		transform.rotation= Quaternion.Slerp(transform.rotation, targetRott,0.1f);
//		child.LookAt (new Vector3(player.position.x, transform.position.y, player.position.z))
	}
}
