using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public float maxLifetime = 60.0f;

    private float lifetime;

    void Update()
    {
        lifetime += Time.deltaTime;
        if (lifetime > maxLifetime)
        {
            Destroy(gameObject);
        }
    }

}
