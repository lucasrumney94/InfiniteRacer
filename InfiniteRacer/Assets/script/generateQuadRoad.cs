using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generateQuadRoad : MonoBehaviour {

	public float timeBetweenPointGeneration = 0.5f;
	public float forwardDistanceBetweenPoints = 10.0f;
	public float curviness = 10.0f;

	public GameObject roadLight;
	public float roadLightHeight = -0.5f;

	private float generationX = 0.0f; // Monotonically Increasing
	private CurvePath curvePath;
	private QuadRoadCreator quadRoadCreator;
//	private GameObject player;

	// Use this for initialization
	void Start () 
	{
		curvePath = FindObjectOfType<PathCreator>().curvePath;
		quadRoadCreator = FindObjectOfType<QuadRoadCreator>();
		//player = GameObject.FindGameObjectWithTag("Player");
		
		StartCoroutine(GenerateNextPoint());
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public IEnumerator GenerateNextPoint()
	{
		//generationX = 
		while (true)
		{

			// Delete any Points behind the player
			//for (int i = 0; i < curvePath.NumSegments/2; i++)
			//{
			//	if (player.transform.position.x > curvePath.GetPointsInSegment(i)[3].x)
			//	{
			//		curvePath.DeleteSegment(i);
			//	}
			//}
//
			
			generationX += forwardDistanceBetweenPoints;
			float generationY = Random.Range(-curviness, curviness);


			Instantiate(roadLight, new Vector3(generationX, generationY, roadLightHeight), Quaternion.identity);

			// Add new Point
			curvePath.AddSegment(new Vector2(generationX, generationY));


			

			 
			quadRoadCreator.UpdateRoad();

			

			yield return new WaitForSeconds(timeBetweenPointGeneration);
		}

	}
}
