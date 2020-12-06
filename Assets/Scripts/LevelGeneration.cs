using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    public Transform[] startingPositions;
    public GameObject[] rooms;

    // Direction to move after initial room
    private int direction;
    // Distance between centers of rooms
    public float moveAmount;

    private float timeBetweenRoom;
    // Time between generating rooms
    private float startTimeBetweenRoom = 0.25f;
    
    private void Start()
    {
        // Pick & instantiate a random room
        int randStartingPos = Random.Range(0, startingPositions.Length);
        transform.position = startingPositions[randStartingPos].position;
        Instantiate(rooms[0], transform.position, Quaternion.identity);

        // Pick a random direction out of 4: up, down, left, right.
        // 40% right, 40% left, 20% down
        direction = Random.Range(1, 6);
    }

    private void Update()
    {
        if(timeBetweenRoom <= 0)
        {
            Move();
            // Waiting before generating the next room
            timeBetweenRoom = startTimeBetweenRoom;
        }
        else
        {
            timeBetweenRoom -= Time.deltaTime;
        }
    }

    private void Move()
    {
        // Move right
        if (direction == 1 || direction == 2)
        {
            Vector2 newPos = new Vector2(transform.position.x + moveAmount, transform.position.y);
            transform.position = newPos;
        } 
        // Move left
        else if (direction == 3 || direction == 4)
        {
            Vector2 newPos = new Vector2(transform.position.x - moveAmount, transform.position.y);
            transform.position = newPos;
        } 
        // Move down
        else if (direction == 5)
        {
            Vector2 newPos = new Vector2(transform.position.x, transform.position.y - moveAmount);
            transform.position = newPos;
        }

        Instantiate(rooms[0], transform.position, Quaternion.identity);
        direction = Random.Range(1, 6);
    }
}
