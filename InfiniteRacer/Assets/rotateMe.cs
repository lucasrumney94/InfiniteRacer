using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateMe : MonoBehaviour {

	public bool WorldSpace;
	public bool randomizeStart;
    public Vector3 rotate;

    void Awake()
    {


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
