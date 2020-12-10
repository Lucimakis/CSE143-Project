using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public int height = 10;
    public int wide = 18;
    public int numOfRoom = 5;
    public GameObject[] startRooms;
    public GameObject[] Rooms;
    public GameObject[] endRooms;

    public GameObject[] enemies;
    public GameObject player;
    


    private Transform outer;
    private Vector3 lastNormalRoom;

    //Set up the whole level
   public void setUpScene()
    {
        string startTag = setupStart();
        string lastTag = setupRoom(startTag, numOfRoom, 0, 0);
        setupEnd(lastTag, lastNormalRoom);
    }
    
   
    //Set up the first room.
    private string setupStart()
    {
        GameObject toInstantiate = startRooms[Random.Range(0, startRooms.Length)];
        GameObject instance = Instantiate(toInstantiate, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        Instantiate(player, new Vector2(0, 0), Quaternion.identity);
        return instance.tag;
    }

    // Set up normal rooms. 
    // lastTag is the tag of the previous room. 
    // times is the total number of the room. 
    // x, y are the position of the last room.
    // it will return the tag of last room.
    private string setupRoom(string lastTag, int times, float x, float y)
    {

        if(times > 0)
        {
            string enter = lastTag.Substring(lastTag.Length - 1);
            //string prePos = lastTag.Substring(0, 1);
            GameObject toInstantiate = Rooms[Random.Range(0, Rooms.Length)];
            string localPos = toInstantiate.tag.Substring(0, 1);
            string exit = toInstantiate.tag.Substring(lastTag.Length - 1);
            while (!localPos.Equals(enter) || (enter.Equals("E") && exit.Equals("W")) || (enter.Equals("W") && exit.Equals("E")))
            {
                toInstantiate = Rooms[Random.Range(0, Rooms.Length)];
                localPos = toInstantiate.tag.Substring(0, 1);
                exit = toInstantiate.tag.Substring(lastTag.Length - 1);
            }
            Vector3 pos = getPos(enter, x, y);
            GameObject instance = Instantiate(toInstantiate, pos, Quaternion.identity) as GameObject;
            Instantiate(enemies[Random.Range(0, enemies.Length)], pos, Quaternion.identity);
            return setupRoom(instance.tag, times - 1, pos.x, pos.y);
        }
        else
        {
            lastNormalRoom = new Vector3(x, y, 0);
            return lastTag;
        }
    }

    // Set up end room.
    // tag is the tag of connected room.
    // postion is 
    private void setupEnd(string tag, Vector3 position)
    {
        string enter = tag.Substring(tag.Length - 1);
        GameObject toInstantiate = endRooms[Random.Range(0, endRooms.Length)];
        string localPos = toInstantiate.tag;
        while (!localPos.Equals(enter))
        {
            toInstantiate = endRooms[Random.Range(0, endRooms.Length)];
            localPos = toInstantiate.tag;
        }
        
        GameObject instance = Instantiate(toInstantiate, getPos(enter, position.x, position.y), Quaternion.identity) as GameObject;
        Instantiate(enemies[Random.Range(0, enemies.Length)], getPos(enter, position.x, position.y), Quaternion.identity);
    }

    // Used to get the room position
    private Vector3 getPos(string tag, float x, float y)
    {
        Vector3 pos;
        if (tag.Equals("E"))
        {
            pos = new Vector3(x + wide, y, 0);
        }
        else if (tag.Equals("W"))
        {
            pos = new Vector3(x - wide, y, 0);
        }
        else
        {
            pos = new Vector3(x, y - height, 0);
        }
        return pos;
    }
}
