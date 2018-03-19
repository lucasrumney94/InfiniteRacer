using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour 
{


	
	[System.Serializable]
	public class Pool
	{
		public string tag;
		public GameObject prefab;
		public int size;
	}

	#region Singleton
	public static ObjectPooler Instance;
	private void Awake()
	{
		Instance = this;
	}
	#endregion

	public List<Pool> pools;
	public Dictionary<string, Queue<GameObject>> poolDictionary;

	private Vector3 AwayFromEverything;

	void Start () 
	{
		AwayFromEverything = new Vector3(100,100,100);
		poolDictionary = new Dictionary<string, Queue<GameObject>>();

		foreach (Pool pool in pools)
		{
			Queue<GameObject> objectPool = new Queue<GameObject>();

			for (int i = 0; i < pool.size; i++)
			{
				GameObject obj = Instantiate(pool.prefab, AwayFromEverything, Quaternion.identity);
				obj.SetActive(false);
				objectPool.Enqueue(obj);
			}

			poolDictionary.Add(pool.tag, objectPool);
		}
	}
	
	public GameObject SpawnFromPool (string tag, Vector3 position, Quaternion rotation)
	{
		if (!poolDictionary.ContainsKey(tag))
		{
			Debug.LogWarning("Pool with tag " + tag + " doesn't exist");
			return null;
		}

		GameObject objectToSpawn = poolDictionary[tag].Dequeue(); 

		objectToSpawn.SetActive(true);
		objectToSpawn.transform.position = position;
		objectToSpawn.transform.rotation = rotation;

		IPooledObject pooledObj = objectToSpawn.GetComponent<IPooledObject>();

		if (pooledObj != null)
		{
			pooledObj.OnObjectSpawn();
		}

		poolDictionary[tag].Enqueue(objectToSpawn);

		return objectToSpawn;
	}

	public GameObject ManualSpawnFromPool (string tag, Vector3 position, Quaternion rotation)
	{
		if (!poolDictionary.ContainsKey(tag))
		{
			Debug.LogWarning("Pool with tag " + tag + " doesn't exist");
			return null;
		}

		GameObject objectToSpawn = poolDictionary[tag].Dequeue(); 

		objectToSpawn.SetActive(true);
		objectToSpawn.transform.position = position;
		objectToSpawn.transform.rotation = rotation;

		IPooledObject pooledObj = objectToSpawn.GetComponent<IPooledObject>();

		if (pooledObj != null)
		{
			pooledObj.OnObjectSpawn();
		}


		return objectToSpawn;
	}
	
	public void ManualDespawn (string tag, GameObject objToDespawn)
	{
		
		objToDespawn.SetActive(false);
		poolDictionary[tag].Enqueue(objToDespawn);


		//poolDictionary[tag].Dequeue();
	}



	void Update () 
	{
		
	}
}
