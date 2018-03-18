using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateMe : MonoBehaviour {

	public bool WorldSpace;
	public bool randomizeStart;
    public Vector3 rotate;

    private Vector3 originalEuler;
    private float randomOffset = 1.0f;

    private bool doOnceFlag = true;

    void Awake()
    {
        if (randomizeStart)
        {
            randomOffset = Random.Range(0.0f, 360.0f);
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (doOnceFlag)
        {
			if (WorldSpace)
			{
				originalEuler = transform.eulerAngles;
			}
			else
			{
            	originalEuler = transform.localEulerAngles;
			}
            doOnceFlag = false;
        }

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
