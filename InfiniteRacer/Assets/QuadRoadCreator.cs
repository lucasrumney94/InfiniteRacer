using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadRoadCreator : MonoBehaviour {

	public GameObject roadQuadPrefab;

	public float spacing = 1.0f;
	public float roadWidth = 1.0f;
	public bool autoUpdate;

	private Vector3[] normals;
	private GameObject g;
	private Vector3 quadScale;

	public List<GameObject> roadQuadsList;

	public void Start()
	{
		quadScale = new Vector3(roadQuadPrefab.transform.localScale.x, roadWidth, 1.0f);
		roadQuadsList = new List<GameObject>();
	}

	public void UpdateRoad()
	{
		if (roadQuadsList.Count > 0)
		{
			for(int i = roadQuadsList.Count-1; i >=0; i--)
			{
				Destroy(roadQuadsList[i].gameObject);
			}
		}

		CurvePath curvePath = GetComponent<PathCreator>().curvePath;
		Vector2[] points = curvePath.CalculateEvenlySpacedPoints(spacing);

		for (int i = 1; i < points.Length; i++)
		{
			g = Instantiate(roadQuadPrefab, points[i], Quaternion.identity) as GameObject;
			g.transform.localScale = quadScale;
			roadQuadsList.Add(g);
			//g.transform.rotation = Quaternion.LookRotation( Vector3.forward, new Vector3(points[i-1].x,points[i-1].y,0));
		}
	}


}
