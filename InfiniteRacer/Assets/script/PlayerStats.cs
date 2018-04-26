using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

	public float score = 0;

	public float collectibleScoreValue = 5.0f;


	private float timeSinceLastCollectible = 0.0f;
	private float lastCollectibleTime;
	private float chainMultiplier;
	//private float 

	// Use this for initialization
	void Start () 
	{
		timeSinceLastCollectible = 0.0f;
		lastCollectibleTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () 
	{
		timeSinceLastCollectible += Time.deltaTime;
	}

	public void collectibleScoreIncrement()
	{
		
		timeSinceLastCollectible = Time.time - lastCollectibleTime;
		chainMultiplier = 1.0f / (timeSinceLastCollectible + 0.1f); 
		score += collectibleScoreValue * chainMultiplier;
		Debug.Log(chainMultiplier);

		// Reset the time 
		timeSinceLastCollectible = 0.0f;
		lastCollectibleTime = Time.time;
	}
}
