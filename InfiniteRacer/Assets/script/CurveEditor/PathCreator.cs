using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathCreator : MonoBehaviour 
{
	[HideInInspector]
	public CurvePath curvePath;
	
	public void CreatePath()
	{
		curvePath = new CurvePath(transform.position);
		 
	}


}
