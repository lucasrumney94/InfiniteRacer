using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generate_Straight_Road : MonoBehaviour {

	public GameObject roadSegment;
	public int numberOfRoadSegments = 100;
	public float roadLength = 10.0f;
	

	// Use this for initialization
	void Start () 
	{
		for (int i=0; i<numberOfRoadSegments; i++)
		{
			Instantiate(roadSegment, i*roadLength*Vector3.forward, Quaternion.identity);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
