using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carQuadFlyer : MonoBehaviour {

	public float speed;
	public float height;
	[Range(1,20)]
	public int turnLookAhead = 10;


	private GenerateQuadFlyerRoad flyer;
	[HideInInspector]
	public int currentPointIndex;
	public int numberOfSegmentsBehindPlayer = 5;
	private int numberOfSegmentsToWaitFor;
	private ObjectPooler objectPooler;

	private Vector3 newPosition;
	private Vector3 nextPosition;
	private Quaternion currentRotation;
	private float t = 0;

	//public string[] lanes = new string[3];
	private string[] lanes = { "Left", "Center", "Right"};
	private int currentLane = 1;

	// Use this for initialization
	void Start () 
	{
		// lanes[0] = "Left";
		// lanes[1] = "Center";
		// lanes[2] = "Right";
		flyer = GameObject.FindGameObjectWithTag("Flyer").GetComponent<GenerateQuadFlyerRoad>();

		objectPooler = ObjectPooler.Instance;
		numberOfSegmentsToWaitFor = objectPooler.pools[0].size;

		currentPointIndex = 0;
	}
	
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
		if (Input.GetButtonDown("Left"))
		{
			if (currentLane > 0)
			{
				currentLane -= 1;
			}
		}
		if (Input.GetButtonDown("Right"))
		{
			if (currentLane < lanes.Length-1)
			{
				currentLane += 1;
			}
		}


		if (flyer.LaneLocations[lanes[1]].Count > numberOfSegmentsToWaitFor-numberOfSegmentsBehindPlayer)
		{
			t += speed*Time.deltaTime;
			if (t>=1)
			{
				t-=1;
				currentPointIndex++;
				currentRotation = transform.rotation;
			}
			newPosition = flyer.LaneLocations[lanes[currentLane]][currentPointIndex]+height*Vector3.up;
			nextPosition = flyer.LaneLocations[lanes[currentLane]][currentPointIndex+1]+height*Vector3.up;

			transform.position = Vector3.Lerp(newPosition, nextPosition, t);
			
			transform.rotation = Quaternion.Lerp(currentRotation, Quaternion.LookRotation(flyer.LaneLocations[lanes[currentLane]][currentPointIndex+turnLookAhead]+height*Vector3.up-newPosition, Vector3.up), t);
		}
		
		


	}
}
