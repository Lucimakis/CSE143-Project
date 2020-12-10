using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") // Object that triggers is the player
        {
            SceneManager.LoadScene(3); // Adds victory screens and buttons
            SceneManager.LoadScene(2, LoadSceneMode.Additive);
        }
    }
}
