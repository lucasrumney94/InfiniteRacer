using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generateCurveRoad : MonoBehaviour {

	public float timeBetweenPointGeneration = 0.5f;
	public float forwardDistanceBetweenPoints = 10.0f;
	public float curviness = 10.0f;

	public GameObject roadLight;
	public float roadLightHeight = -0.5f;

	private float generationX = 0.0f; // Monotonically Increasing
	private CurvePath curvePath;
	private RoadCreator roadCreator;


	// Use this for initialization
	void Start () 
	{
		curvePath = FindObjectOfType<PathCreator>().curvePath;
		roadCreator = FindObjectOfType<RoadCreator>();

		
		StartCoroutine(GenerateNextPoint());
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public IEnumerator GenerateNextPoint()
	{
		while (true)
		{
			//Debug.Log("Generate Point!");

			
			generationX += forwardDistanceBetweenPoints;
			float generationY = Random.Range(-curviness, curviness);


			Instantiate(roadLight, new Vector3(generationX, generationY, roadLightHeight), Quaternion.identity);

			// Add new Point
			curvePath.AddSegment(new Vector2(generationX, generationY));

			// Remove Oldest Point Every
			if (Mathf.FloorToInt(Time.time)%Mathf.FloorToInt(6*timeBetweenPointGeneration)==0)
			{
				//curvePath.DeleteSegment(0);
			}

			 
			roadCreator.UpdateRoad();


			yield return new WaitForSeconds(timeBetweenPointGeneration);
		}

	}
}
