using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    public GameObject death; // Enemy death object
    private float speed; // Speed the enemies move at
    private Controller2D controller; // Movement controller
    private BoxCollider2D hitbox; // Enemy zone to deal damage to player
    private SpriteRenderer sr; // Sprite model manager
    private int health; // Amount of health points
    private float damagedTime; // The time the enemy shows damage for
    private float deathAnim; // Length of time the body stays on the screen
    private int damageAmount; // Amount of damage done

    void Awake()
    {
        health = 100;
        damagedTime = 0.2f;
        deathAnim = 1.0f;
        speed = 100f;
        damageAmount = 40;
    }

    void Start()
    {
        hitbox = GetComponent<BoxCollider2D>();
        controller = GetComponent<Controller2D>();
        sr = GetComponent<SpriteRenderer>();
        StartCoroutine(Patrol()); // Enemies walk back and forth
    }

    // Destroys redundant enemies
    // Enemies attack each other
    void Update()
    {
        Collider2D[] collisions = Physics2D.OverlapBoxAll(transform.position, hitbox.size, 0);
        foreach(Collider2D collision in collisions)
        {
            if (collision != hitbox && collision.tag == "Enemy") // Collision should not be their own hitbox
            {
                Damage(damageAmount);
            } 
        }
    }

    // Damages the enemy by the given amount
    public void Damage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die(); 
        }
        else
        {
            StartCoroutine(switchColor()); // Flashes color to red when damaged
        }
    }

    // Destroys the object and shows the body
    void Die()
    {
        Destroy(gameObject);
        GameObject body = Instantiate(death, transform.position, transform.rotation) as GameObject;
        Destroy(body, deathAnim);
    }

    // Switches the color for a number of seconds
    IEnumerator switchColor()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(damagedTime);
        sr.color = Color.white;
    }

    // Enemy patrols an area until it dies
    private IEnumerator Patrol()
    {
        while (health > 0) // True until the enemy dies
        {
            float xVelocity;
            if (Random.value > 0.5f)
            {
                xVelocity = speed;
            }
            else
            {
                xVelocity = -speed;
            }
            controller.Move(xVelocity, false, false);
            yield return new WaitForSeconds(Random.Range(0f, 2f)); // Random.Range is inclusive for both numbers when using floats
        }
    }
}
