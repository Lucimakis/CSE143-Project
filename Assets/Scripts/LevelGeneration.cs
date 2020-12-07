﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    public Transform[] startingPositions;
    // index 0 ==> LR
    // index 1 ==> LRB
    // index 2 ==> LRT
    // index 3 ==> LRBT
    public GameObject[] rooms; 

    // Direction to move after initial room
    private int direction;
    // Distance between centers of rooms
    public float moveAmount;

    private float timeBetweenRoom;
    // Time between generating rooms
    private float startTimeBetweenRoom = 0.25f;
    public bool stopGeneration;

    public float minX;
    public float maxX;
    public float minY;

    public LayerMask room;

    private int downCounter;
    
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
        if (timeBetweenRoom <= 0 && stopGeneration == false)
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
            if (transform.position.x < maxX)
            {
                downCounter = 0;

                Vector2 newPos = new Vector2(transform.position.x + moveAmount, transform.position.y);
                transform.position = newPos;

                // Picking random rooms
                int rand = Random.Range(0, rooms.Length);
                Instantiate(rooms[rand], transform.position, Quaternion.identity);

                // Can only go right or down after going right;
                // Not sure why the tutorial did it this way
                // This RNG weighting is uneven in a 3:2 ratio
                // Also note, can reduce this redundancy if we change subweights
                // to 2:1--if down is set to 3, then just do 
                // Random.Range(1,4);
                direction = Random.Range(1, 6);
                if (direction == 3)
                {
                    direction = 2;
                } 
                else if (direction == 4)
                {
                    direction = 5;
                }
            } 
            // If x position is at maxX, change direction to go down.
            else
            {
                direction = 5;
            }
        } 
        // Move left
        else if (direction == 3 || direction == 4)
        {
            if (transform.position.x > minX)
            {
                downCounter = 0;
                Vector2 newPos = new Vector2(transform.position.x - moveAmount, transform.position.y);
                transform.position = newPos;

                // Picking random rooms
                int rand = Random.Range(0, rooms.Length);
                Instantiate(rooms[rand], transform.position, Quaternion.identity);

                // Can only go left or down after going left
                // More inconsistence, this is weighing left and down in a 2:1 ratio
                direction = Random.Range(3, 6);
            }
            // If x position is at minX, change direction to down;
            else
            {
                direction = 5;
            }
        } 
        // Move down
        else if (direction == 5)
        {
            downCounter++;

            if (transform.position.y > minY)
            {
                Collider2D roomDetection = Physics2D.OverlapCircle(transform.position, 1, room);
                RoomType roomAbove = roomDetection.GetComponent<RoomType>();
                if(roomAbove.type != 1 || roomAbove.type != 3)
                {
                    if (downCounter >= 2)
                    {
                        roomAbove.RoomDestruction();
                        Instantiate(rooms[3], transform.position, Quaternion.identity);
                    }
                    else
                    {
                        roomAbove.RoomDestruction();

                        // Once again, ineffcient and strange weighting
                        int randBottomOpening = Random.Range(1, 4);
                        if (randBottomOpening == 2)
                        {
                            randBottomOpening = 1;
                        }
                        Instantiate(rooms[randBottomOpening], transform.position, Quaternion.identity);
                    }
                }

                Vector2 newPos = new Vector2(transform.position.x, transform.position.y - moveAmount);
                transform.position = newPos;

                // Room 2 & 3 have top openings
                int rand = Random.Range(2, 4);
                Instantiate(rooms[rand], transform.position, Quaternion.identity);

                direction = Random.Range(1, 6);
            }
            else
            {
                stopGeneration = true;
            }
        }
    }
}
