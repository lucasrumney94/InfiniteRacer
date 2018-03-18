using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCarSmoothFollow : MonoBehaviour {

	public Transform target;

	public float laneChangeSpeed = 1.0f;
	public float smoothTurnSpeed = 1.0f;


	Vector3 smoothedPosition;

	Quaternion smoothedRotation;

	void FixedUpdate () 
	{

		// Smooth Lane Changing
		smoothedPosition = Vector3.Lerp(transform.position, target.position, laneChangeSpeed);
		transform.position = smoothedPosition;

		// Smooth Turning
		smoothedRotation = Quaternion.Lerp(transform.rotation, target.rotation, smoothTurnSpeed);
		transform.rotation = smoothedRotation;

	}
}
