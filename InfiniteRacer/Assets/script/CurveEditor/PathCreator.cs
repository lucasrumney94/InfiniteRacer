using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathCreator : MonoBehaviour 
{
	[HideInInspector]
	public CurvePath curvePath;

	public Color anchorCol = Color.red;
	public Color controlCol = Color.white;
	public Color segmentCol = Color.green;
	public Color selectedSegmentCol = Color.yellow;

	public float anchorDiameter = 0.1f;
	public float controlDiameter = 0.075f;
	public bool displayControlPoints = true;

	
	public void CreatePath()
	{
		curvePath = new CurvePath(transform.position);
		 
	}

	void Reset()
	{
		CreatePath();
	}

}
