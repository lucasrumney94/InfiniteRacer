using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtrudeShape : MonoBehaviour 
{
	Vector2[] verts;
	Vector2[] normals;
	float[] us;
	int[] lines = new int[]{
		0, 1,
		2, 3,
		3, 4,
		4, 5
	};
}
