using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCarCamera : MonoBehaviour {

	public float speed = .3f;
	public float laneWidth = 3.33f;
	public float laneChangeSpeed = 1.0f;

	private Vector3 targetPosition;
	private float xcomponent;
	private float zcomponent;

	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		if (Input.GetButtonDown("Left"))
		{
			//Debug.Log("Left!");
			if (transform.position.x > - laneWidth)
			{ 
				targetPosition -= laneWidth*Vector3.right;
			}
		}
		if (Input.GetButtonDown("Right"))
		{
			//Debug.Log("Right!");
			if (transform.position.x < laneWidth)
			{
				targetPosition += laneWidth*Vector3.right;
			}
		}


		xcomponent = Mathf.Lerp(transform.position.x, targetPosition.x, laneChangeSpeed);
		zcomponent = transform.position.z+speed;

		transform.position = new Vector3(xcomponent,transform.position.y,zcomponent);
		
	}
}
