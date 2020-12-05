using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject death; // Enemy death object
    private SpriteRenderer sr; // Sprite model manager
    private int health; // Amount of health points
    private float damagedTime; // The time the enemy shows damage for
    private float deathAnim; // Length of time the body stays on the screen

    void Awake()
    {
        health = 100;
        damagedTime = 0.2f;
        deathAnim = 1.0f;
    }

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Damages the object by the given amount
    public void Damage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die(); 
        }
        StartCoroutine(switchColor()); // Flashes color to red when damaged
    }

    // Destroys the object and shows the body
    void Die()
    {
        Destroy(gameObject);
        GameObject body = (GameObject)Instantiate(death, transform.position, transform.rotation);
        Destroy(body, deathAnim);
    }

    // Switches the color for a number of seconds
    IEnumerator switchColor()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(damagedTime);
        sr.color = Color.white;
    }
}
