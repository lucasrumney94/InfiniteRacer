using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wigglePosition : MonoBehaviour {

	public Vector3 wiggleAmount;
	public float height;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.position = new Vector3(wiggleAmount.x*Mathf.Sin(Time.time), height, wiggleAmount.z*Mathf.Cos(Time.time));
	}
}
