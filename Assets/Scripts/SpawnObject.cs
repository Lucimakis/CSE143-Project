using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour {

    // Array of possible tiles
    public GameObject[] objects;

    private void Start()
    {
        // Pick and instantiate a random tile
        int rand = Random.Range(0, objects.Length);
        GameObject instance = (GameObject) Instantiate(objects[rand], transform.position, Quaternion.identity);
        instance.transform.parent = transform;
    }
}
