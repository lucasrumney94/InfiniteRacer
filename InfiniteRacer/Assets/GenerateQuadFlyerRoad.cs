using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateQuadFlyerRoad : MonoBehaviour {

	public GameObject flyerRoadQuad;
	public float stepDistance = 1.0f;

	public float maxTurnTime = 8.0f;
	public float minTurnTime = 2.0f;
	public float turnSpeed = 0.8f;

	public bool turns = true;
	public bool left = false;
	public bool right = false;

	public List<GameObject> roadQuadPool;
	public int numberOfQuadsToPool;

	public GameObject RoadLight;
	public float roadLightPeriod = 5.0f;

	[HideInInspector]
	public List<Vector3> middleLaneLocations = new List<Vector3>();

	private string[] directions = new string[3];
	private string direction = "";
	


	// Use this for initialization
	void Start () 
	{
		directions[0] = "straight";
		directions[1] = "left";
		directions[2] = "right";

		for (int i = 0; i < numberOfQuadsToPool; i++)
		{
			GameObject g = Instantiate(flyerRoadQuad) as GameObject;
			g.SetActive(false);
			roadQuadPool.Add(g);
		}


		StartCoroutine(SwitchDirection());
		StartCoroutine(SpawnRoadLight());
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (left)
		{
			transform.Rotate(0,-turnSpeed,0);
		}
		if (right)
		{
			transform.Rotate(0,turnSpeed,0);
		}

		transform.Translate(stepDistance*Vector3.forward, Space.Self);
		transform.Translate(Vector3.up*0.001f, Space.Self);
		Instantiate(flyerRoadQuad, transform.position, transform.rotation);
		middleLaneLocations.Add(transform.position);
	}

	IEnumerator SwitchDirection()
	{
		while (turns)
		{
			direction = directions[Random.Range(0,directions.Length)];

			//Debug.Log("Changed Direction to "+direction);

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
			

			yield return new WaitForSeconds(Random.Range(minTurnTime, maxTurnTime));

		}
	}

	IEnumerator SpawnRoadLight()
	{
		while (true)
		{

			Instantiate(RoadLight, transform.position, transform.rotation);
			yield return new WaitForSeconds(roadLightPeriod);
		}
	}
}
