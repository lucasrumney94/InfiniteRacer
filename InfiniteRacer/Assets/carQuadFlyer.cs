﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carQuadFlyer : MonoBehaviour {

	public float speed;
	public float height;
	[Range(1,20)]
	public int turnLookAhead = 10;


	private GenerateQuadFlyerRoad flyer;
	private int currentPointIndex = 0;

	private Vector3 newPosition;
	private Vector3 nextPosition;
	private Quaternion currentRotation;
	private float t;

	// Use this for initialization
	void Start () 
	{
		flyer = GameObject.FindGameObjectWithTag("Flyer").GetComponent<GenerateQuadFlyerRoad>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		//Manual Movement 
		// i++;
		// if (i>0)
		// {
		// 	transform.position = flyer.middleLaneLocations[i]+Vector3.up*height;
		// 	transform.LookAt(flyer.middleLaneLocations[i+20]);
		// }


		//Smooth Movement
		t += speed*Time.deltaTime;
		if (t>=1)
		{
			t-=1;
			currentPointIndex++;
			currentRotation = transform.rotation;
		}
		//if (currentPointIndex>flyer.middleLaneLocations.Count-2)
		//{
		//	currentPointIndex = 0;
		//}

		if (flyer.middleLaneLocations.Count > turnLookAhead)
		{
			newPosition = flyer.middleLaneLocations[currentPointIndex]+height*Vector3.up;
			nextPosition = flyer.middleLaneLocations[currentPointIndex+1]+height*Vector3.up;

			transform.position = Vector3.Lerp(newPosition, nextPosition, t);
			
			transform.rotation = Quaternion.Lerp(currentRotation, Quaternion.LookRotation(flyer.middleLaneLocations[currentPointIndex+turnLookAhead]+height*Vector3.up-newPosition, Vector3.up), t);
		}


	}
}
