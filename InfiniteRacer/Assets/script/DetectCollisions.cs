using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollisions : MonoBehaviour {


	PlayerStats playerStats;
	ObjectPooler objectPooler;

	// Use this for initialization
	void Start () 
	{
		playerStats = GetComponent<PlayerStats>();
		objectPooler = ObjectPooler.Instance;
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	void OnTriggerEnter(Collider other)
	{

		if (other.CompareTag("Collectible"))
		{
			playerStats.collectibleScoreIncrement();
			Destroy(other.gameObject);
		}
	}
}
