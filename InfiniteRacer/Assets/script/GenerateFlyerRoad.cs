using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateFlyerRoad : MonoBehaviour {

	public int numberOfRoadSegments = 300;
	public float roadLength = 1.0f;

	public float AltitudeVariationAmount = 1.0f;
	public float AltitudeVariationSpeed = .05f;
	public float TurnVariationAmount = 10.0f;	
	public float TurnVariationSpeed = 0.01f;


	private GameObject RoadSegment;

	void Start () 
	{
		RoadSegment = Resources.Load("placeholder/FlyerRoads/RoadSegment") as GameObject;

		for (int i = 0; i<numberOfRoadSegments; i++)
		{
			this.transform.position = new Vector3(TurnVariationAmount*Mathf.Sin(TurnVariationSpeed*i),AltitudeVariationAmount*Mathf.Sin(AltitudeVariationSpeed*i),i*roadLength); 
			GameObject.Instantiate(RoadSegment, this.transform.position, this.transform.rotation);
		}
	}
	

	void Update ()
	{
		
	}
}
