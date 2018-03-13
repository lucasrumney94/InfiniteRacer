
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class CreateQuad : MonoBehaviour {



	private Vector3[] verts;

	private Vector3[] normals;

	private Vector2[] uvs;

	private int[] tris;

	private MeshFilter mf;

	private Mesh mesh;


	// Use this for initialization
	void Start () 
	{
		// Get Current Mesh Filter
		mf = GetComponent<MeshFilter>();

		// Create new Mesh if Null
		if (mf.sharedMesh == null)
		{
			mf.sharedMesh = new Mesh();
		}
		mesh = mf.sharedMesh;

		verts = new Vector3[] 
		{
			new Vector3(  1, 0,  1),
			new Vector3( -1, 0,  1),
			new Vector3(  1, 0, -1),
			new Vector3( -1, 0, -1),
		};

		normals = new Vector3[] 
		{
			new Vector3(  0, 1, 0),
			new Vector3(  0, 1, 0),
			new Vector3(  0, 1, 0),
			new Vector3(  0, 1, 0),
		};

		uvs = new Vector2[]
		{
			new Vector2(0, 1),
			new Vector2(0, 0),
			new Vector2(1, 1),
			new Vector2(1, 0),
		};

		tris = new int[]
		{
			0, 2, 3,
			3, 1, 0
		};



		// 
		mesh.Clear();
		mesh.vertices = verts;
		mesh.normals = normals;
		mesh.uv = uvs;
		mesh.triangles = tris;
		


	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
