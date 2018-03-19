using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeExploder : MonoBehaviour {

	ObjectPooler objectPooler;

	void Start()
	{
		objectPooler = ObjectPooler.Instance;
	}

	void FixedUpdate()
	{
		objectPooler.SpawnFromPool("cube", transform.position, Quaternion.identity);
	}
}
