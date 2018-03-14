using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carQuadFlyer : MonoBehaviour {

	public float speed;
	public float height;


	private GenerateQuadFlyerRoad flyer;
	public int i = -1000;

	// Use this for initialization
	void Start () 
	{
		flyer = GameObject.FindGameObjectWithTag("Flyer").GetComponent<GenerateQuadFlyerRoad>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		i++;
		if (i>0)
		{
			transform.position = flyer.middleLaneLocations[i]+Vector3.up*height;
			transform.LookAt(flyer.middleLaneLocations[i+10]);
		}
	}
}
