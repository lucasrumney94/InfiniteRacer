using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class getPlayerScore : MonoBehaviour {

	private PlayerStats playerStats;
	private Text myScoreText;
	

	// Use this for initialization
	void Start ()
	{
		myScoreText = GetComponent<Text>();
		playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		myScoreText.text = playerStats.score.ToString();
	}
}
