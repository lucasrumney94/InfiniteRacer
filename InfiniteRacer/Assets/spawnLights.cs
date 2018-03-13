using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnLights : MonoBehaviour {

	public List<GameObject> lightsToSpawn;
	public List<Vector3> positions;


	private bool flag = false;
	private bool done = false;
	// Use this for initialization
	void Start () 
	{
		StartCoroutine(spawnLightsDuringRuntime());
	}
	
	IEnumerator spawnLightsDuringRuntime()
	{
		while (!done)
		{
			if (flag)
			{
				yield return new WaitForSeconds(3.0f);
			}
			else
			{
				for (int i = 0; i < lightsToSpawn.Count; i++)
				{
					
					GameObject g = Instantiate(lightsToSpawn[i], positions[i], Quaternion.identity);
					g.transform.parent = transform;
				}

				done = true;
			}
		}
	}
}
