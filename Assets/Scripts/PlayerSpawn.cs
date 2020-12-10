using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    private void Awake()
    {
        GameObject instance = Instantiate(player, transform.position, Quaternion.identity) as GameObject;
    }
}
