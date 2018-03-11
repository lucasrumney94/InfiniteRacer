using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Path : MonoBehaviour {

	[SerializeField, HideInInspector]
	List<Vector2> points;

	public Path(Vector2 center)
	{
		points = new List<Vector2>
		{
			
		};
	}
}
