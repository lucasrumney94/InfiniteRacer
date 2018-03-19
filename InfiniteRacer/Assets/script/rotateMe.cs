using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateMe : MonoBehaviour {

	public bool WorldSpace;
	public bool randomizeStart;
    public Vector3 rotate;

    void OnEnable()
    {
		if (randomizeStart)
		{
			transform.rotation = new Quaternion(Random.Range(0,1),Random.Range(0,1),Random.Range(0,1),Random.Range(0,1));
		}

    }

    // Update is called once per frame
    void FixedUpdate()
    {


		if (WorldSpace)
		{
        	transform.Rotate(rotate, Space.World);
		}
		else 
		{
        	transform.Rotate(rotate, Space.Self);
		}
    }
}
