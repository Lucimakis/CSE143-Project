using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Gives the bullet a speed out of the player and reduces the health of any hit enemies */
public class BulletScript : MonoBehaviour
{
    private float speed; // Speed of the bullet
    private Rigidbody2D rb; // Physics object that is modified to move the bullet
    private int damage; // The damage the bullet does on contact

    void Awake()
    {
        speed = 20f; 
        damage = 40;
        rb = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.right * speed; // Starts the bullet with the speed in the direction it is fired in.
    }

    // Enters when the bullet collides with an object
    // Damages the collision if it is an enemy and deletes the bullet.
    void OnTriggerEnter2D(Collider2D hitObject)
    {
        if (hitObject.gameObject.tag != "ProjectileIgnore")
        {
            Enemy enemy = hitObject.GetComponent<Enemy>(); // Gets the enemy script
            if (enemy != null)
            {
                enemy.Damage(damage);
            }
            Destroy(gameObject);
        }
    }
}
