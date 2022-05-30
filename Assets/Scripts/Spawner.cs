using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefab;
    public float minTime = 2f;
    public float maxTime = 4f;

    private void Start() 
    {
        Spawn();
    }

    private void Spawn()
    {
        Instantiate(prefab, transform.position, Quaternion.identity); // makes a copy of prefab, spawns wherever spawner is located and can move it wherever, rotation we don't care
        Invoke(nameof(Spawn), Random.Range(minTime, maxTime)); // call again after some random time between min and max
    }
}
