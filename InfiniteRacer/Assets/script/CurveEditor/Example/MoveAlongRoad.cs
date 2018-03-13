using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAlongRoad : MonoBehaviour {
	public float speed = 0.1f;
	public float height = 1.0f;

	public float movementSpacing = 0.05f;
	public float movementResolution = 1.0f;

	//public GameObject roadLight;

	private int currentSegment = 0;
	private CurvePath curvePath;
	private float t = 0.0f;
	private Vector2[] points;
	private int currentPointIndex = 0;
	private Vector3 newPosition;
	private Vector3 nextPosition;
	private Quaternion currentRotation;


	// Use this for initialization
	void Start () 
	{
		curvePath = FindObjectOfType<PathCreator>().curvePath;
		
		
		points = curvePath.CalculateEvenlySpacedPoints(movementSpacing, movementResolution);

		//foreach (Vector2 p in points)
		//{
		//	GameObject.Instantiate(roadLight, new Vector3(p.x, p.y, -1.0f), Quaternion.identity);
			
		//}
		StartCoroutine(UpdateEvenlySpacedPoints());
		
	}

	void Update()
	{
		
		t += speed*Time.deltaTime;
		if (t>=1)
		{
			t-=1;
			currentPointIndex++;
			currentRotation = transform.rotation;
		}
		if (currentPointIndex>points.Length-2)
		{
			currentPointIndex = 0;
		}

		
		//Manual Setting
		//newPosition = new Vector3(points[currentPointIndex].x, points[currentPointIndex].y, -height);
		//transform.position = new Vector3(newPosition.x, newPosition.y, -height);
		//nextPosition = new Vector3(points[(currentPointIndex+1)%points.Length].x, points[(currentPointIndex+1)%points.Length].y, -height);
		//transform.LookAt(new Vector3(nextPosition.x, nextPosition.y, -height), Vector3.back);
		//currentPointIndex++;

		//Attempt at Linear Interpolation with Speed
		newPosition = new Vector3(points[currentPointIndex].x, points[currentPointIndex].y, -height);
		nextPosition = new Vector3(points[(currentPointIndex+1)%points.Length].x, points[(currentPointIndex+1)%points.Length].y, -height);

		transform.position = Vector3.Lerp(newPosition, nextPosition, t);

		
		transform.rotation = Quaternion.Lerp(currentRotation, Quaternion.LookRotation(nextPosition-newPosition, Vector3.back), t);
		//transform.LookAt(new Vector3(nextPosition.x, nextPosition.y, -height), Vector3.back);
	}

	IEnumerator UpdateEvenlySpacedPoints()
	{
		while (true)
		{
			points = curvePath.CalculateEvenlySpacedPoints(movementSpacing, movementResolution);
			yield return new WaitForSeconds(0.3f);
		}
	}
}
