﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateQuadFlyerRoad : MonoBehaviour {

	// Road Generation
	public GameObject flyerRoadQuad;
	public float stepDistance = 1.0f;
	public float laneWidth = 2.5f;
	[HideInInspector]
	public Dictionary<string, List<Vector3>> LaneLocations = new Dictionary<string,List<Vector3>>();
	private bool firstGen = true;
	private string[] lanes = {"Left", "Center", "Right"};

	// Turning
	public int minTurnSegments = 10;
	public int maxTurnSegments = 30;
	private int turnSegmentCounter = 0;
	private int numberSegmentsInThisTurn;

	public float minTurnSpeed = 0.1f;
	public float maxTurnSpeed = 0.4f;
	private float turnSpeedForThisTurn;

	private string[] directions = new string[3];
	private string direction = "";
	public bool turns = true;
	public bool left = false;
	public bool right = false;

	// Road Lights
	public GameObject RoadLightRight;
	public GameObject RoadLightLeft;
	public float roadLightPeriod = 5.0f;

	//Collectibles
	public GameObject collectiblePrefab;
	public int collectibleMinStringLength = 3;
	public int collectibleMaxStringLength = 7;
	public int collectibleStringOccurence = 20;
	public int collectibleStringOccurenceRandomness = 4;
	public float collectibleHeight = 1.0f;
	private int segmentsSinceLastCollectible = 0;
	private int numberCollectiblesThisString = 1;
	private bool placeCollectible = false;
	private string laneThisString;

	// Buildings
	public List<GameObject> buildings;
	public float buildingPeriod = 5.0f;

	// Object Pooler
	[HideInInspector]
	public ObjectPooler objectPooler;

	// Player References
	private carQuadFlyer playerCar;
	private int playerCarLastIndex = 0;

	void Start () 
	{
		LaneLocations["Left"] = new List<Vector3>();
		LaneLocations["Center"] = new List<Vector3>();
		LaneLocations["Right"] = new List<Vector3>();
		
		playerCar = GameObject.FindGameObjectWithTag("LaneFollower").GetComponent<carQuadFlyer>();
		objectPooler = ObjectPooler.Instance;
		directions[0] = "straight";
		directions[1] = "left";
		directions[2] = "right";

		StartCoroutine(SpawnRoadLight());
		StartCoroutine(SpawnTestBuilding());
		
	}
	

	void Update () 
	{


		if (firstGen)
		{
			// Move Forward
			transform.Translate(stepDistance*Vector3.forward, Space.Self);
			transform.Translate(Vector3.down*0.005f, Space.Self);

			// Turn
			TurnUpdate(); 
			if (left)
			{
				transform.Rotate(0,-turnSpeedForThisTurn,0);
			}
			if (right)
			{
				transform.Rotate(0,turnSpeedForThisTurn,0);
			}

			// Spawn Road
			objectPooler.SpawnFromPool("flyerQuad", transform.position, transform.rotation);
		
			// Add Lane Positions
			LaneLocations["Center"].Add(transform.position);
			LaneLocations["Right"].Add(transform.position+transform.right*laneWidth);
			LaneLocations["Left"].Add(transform.position-transform.right*laneWidth);

			// Spawn Collectibles
			//SpawnCollectibles();

			// If we've generated all of the pooled roads at first, switch to generating only what's needed 
			if (LaneLocations["Center"].Count > objectPooler.poolDictionary["flyerQuad"].Count)
			{
				firstGen = false;
			}
		}
		else
		{
			if (playerCarLastIndex != playerCar.currentPointIndex)
			{
				
				transform.Translate(stepDistance*Vector3.forward, Space.Self);
				transform.Translate(Vector3.down*0.005f, Space.Self);


				TurnUpdate();
				if (left)
				{
					transform.Rotate(0, -turnSpeedForThisTurn, 0);
				}
				if (right)
				{
					transform.Rotate(0, turnSpeedForThisTurn, 0);
				}


				objectPooler.SpawnFromPool("flyerQuad", transform.position, transform.rotation);
			
				LaneLocations["Center"].Add(transform.position);
				LaneLocations["Right"].Add(transform.position+transform.right*laneWidth);
				LaneLocations["Left"].Add(transform.position-transform.right*laneWidth);

				// Spawn Collectibles
				SpawnCollectibles();

				playerCarLastIndex = playerCar.currentPointIndex;



			}
		}
		
	}

	void TurnUpdate()
	{
		if (turns)
		{
			turnSegmentCounter++;
			if (turnSegmentCounter > numberSegmentsInThisTurn)
			{
				numberSegmentsInThisTurn = Random.Range(minTurnSegments, maxTurnSegments);
				turnSpeedForThisTurn = Random.Range(minTurnSpeed, maxTurnSpeed);
				turnSegmentCounter = 0;
				direction = directions[Random.Range(0,directions.Length)];

				//Debug.Log("Changed Direction to " + direction);

				if (direction.Contains("straight"))
				{
					left = false;
					right = false;
				}
				if (direction.Contains("left"))
				{
					left = true;
					right = false;
				}
				if (direction.Contains("right"))
				{
					left = false;
					right = true;
				}
			}
		}
	}

	void SpawnCollectibles()
	{
		//if (LaneLocations[laneThisString].Count)
		if (segmentsSinceLastCollectible <= collectibleStringOccurence)
		{
			segmentsSinceLastCollectible++;

		}

		if (segmentsSinceLastCollectible >= collectibleStringOccurence && !placeCollectible)
		{
			placeCollectible=true;
			numberCollectiblesThisString = Random.Range(collectibleMinStringLength, collectibleMaxStringLength);
			laneThisString = lanes[Random.Range(0,3)];
		}

		if (placeCollectible)
		{
			numberCollectiblesThisString--;
			Vector3 location = LaneLocations[laneThisString][LaneLocations[laneThisString].Count-1] +Vector3.up*collectibleHeight;

			Instantiate(collectiblePrefab, location, Quaternion.identity);

			if (numberCollectiblesThisString<0)
			{
				placeCollectible = false;
				numberCollectiblesThisString = 0;
				segmentsSinceLastCollectible = Random.Range(-collectibleStringOccurenceRandomness, collectibleStringOccurenceRandomness);
			}
		}
	}

	IEnumerator SpawnRoadLight()
	{
		while (true)
		{

			Instantiate(RoadLightRight, transform.position, transform.rotation);

			Vector3 myRotation = transform.rotation.eulerAngles;
			myRotation = new Vector3(myRotation.x, myRotation.y+180, myRotation.z);

			Instantiate(RoadLightRight, transform.position-0.8f*transform.right, Quaternion.Euler(myRotation));

			yield return new WaitForSeconds(roadLightPeriod);
		}
	}

	IEnumerator SpawnTestBuilding()
	{
		while (true)
		{

			Instantiate(buildings[Random.Range(0,buildings.Count)], transform.position+8.0f*transform.right, transform.rotation);

			Vector3 myRotation = transform.rotation.eulerAngles;
			myRotation = new Vector3(myRotation.x, myRotation.y+180, myRotation.z);

			Instantiate(buildings[Random.Range(0,buildings.Count)], transform.position-8.0f*transform.right, Quaternion.Euler(myRotation));

			yield return new WaitForSeconds(buildingPeriod);

		}
	}
}
