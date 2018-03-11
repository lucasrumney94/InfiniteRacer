using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateWedgeRoad : MonoBehaviour {

	private string left = "left";
	private string right = "right";
	private string up = "up";
	private string down = "down";
	private string straight = "straight";


	public Dictionary<string, GameObject> Wedges = new Dictionary<string, GameObject>();
	public string Direction;
	
	public int numberOfRoadSegments = 300;
	public float roadLength = 1.0f;
	public float roadHeight = 0.5f;
	private float currentHeight = 0.0f;


	void Start ()
	{
		Wedges.Add(straight, Resources.Load("placeholder/wedges/ROAD_S") as GameObject);
		Wedges.Add(down, Resources.Load("placeholder/wedges/ROAD_SD") as GameObject);
		Wedges.Add(up, Resources.Load("placeholder/wedges/ROAD_SU") as GameObject);

		Direction = straight;
		for (int i=0; i<numberOfRoadSegments; i++)
		{
			if (i == 20)
			{
				Direction = down;
			}

			if (i == 40)
			{
				Direction= straight;
			}

			if (i == 100)
			{
				Direction = up;
			}

			if (Direction == up)
			{
				currentHeight -= roadHeight;
			}
			if (Direction == down)
			{
				currentHeight += roadHeight;
			}
			Instantiate(Wedges[Direction], i*roadLength*Vector3.forward-Vector3.up*currentHeight, Quaternion.identity);
		}


	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

}
