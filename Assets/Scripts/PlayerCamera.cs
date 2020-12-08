using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private Transform playerLocation; // Where the player currently is
    private Vector3 depth; // Added depth for the camera to see the whole scene

    void Start()
    {
        depth = new Vector3(0, 0, -1);
    }
    // Update is called once per frame
    void Update()
    {
        if (playerLocation == null) // Gets the player's initial position
        {
            playerLocation = GameObject.FindWithTag("Player").transform;
        } else
        {
            transform.position = playerLocation.position + depth; // Camera stays on the character
        }
    }
}
