using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private int health; // Amount of health the player has
    private int damageTaken; // Amount of damage enemies give to the player
    private float damagedTime; // Time the character flashes red on damage
    private float invincibleStart; // Time the character is invincible upon spawn
    private float speed; // Speed of the character movement
    private BoxCollider2D hitBox; // The player's hitbox
    private Controller2D controller; // Controller for movement
    private Animator animator; // Player animation controller 
    private SpriteRenderer renderer; // Player model manager
    private float xVelocity; // Movement speed based on input
    private bool jump; // Whether player is jumping
    private bool crouch; // Whether player is crouching
    private bool roll; // Whether player is rolling
    private bool controlLoss; // Amount of time the player cannot be controlled
    private bool invincible; // Player does not take damage
    private int maxDistance; // The maximum distance before the player is respawned
    private Vector2 spawn; // Where the player is spawned

    void Awake()
    {
        jump = false;
        crouch = false;
        roll = false;
        damagedTime = 0.15f;
        invincibleStart = 3f;
        speed = 250.0f;
        maxDistance = 100;
        spawn = transform.position;
        hitBox = GetComponent<BoxCollider2D>();
        controller = GetComponent<Controller2D>();
        renderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        health = 100;
        damageTaken = 40;
        StartCoroutine(switchColor(Color.blue, invincibleStart));
    }

    // Called every frame
    void Update()
    {
        if (Vector2.Distance(transform.position, spawn) >= maxDistance)
        {
            transform.position = spawn;
        }
        xVelocity = Input.GetAxisRaw("Horizontal") * speed; // Movement based on input side-to-side
        animator.SetFloat("Speed", Mathf.Abs(xVelocity)); // Changes the flag in the animator for speed
        if (Input.GetKeyDown("up"))
        {
            jump = true; // Jumping input 
            animator.SetBool("Jump", true); // Starts the jump animation
        }
        else if (Input.GetKeyUp("up")) // Ends jumping input but not the animation
        {
            jump = false; 
        }
        crouch = Input.GetKey("down"); // Whether or not to crouch
        animator.SetBool("Crouch", crouch); // Changes the animation to crouch
        if (Input.GetKeyDown(KeyCode.LeftShift)) // Rolls and plays animation
        {
            roll = true;
            animator.SetBool("Roll", true);
        }
    }

    // Plays the entire roll animation before ending
    // Triggers on animation event finish
    void rollFinish() 
    {
        roll = false;
        animator.SetBool("Roll", false);
    }

    // Ends the jump animation when landing
    void OnLanding() 
    {
        animator.SetBool("Jump", false);
    }

    // Moves the character based on input
    // Called every physics frame
    void FixedUpdate()
    {
        if (!controlLoss)
        {
            controller.Move(xVelocity * Time.fixedDeltaTime, crouch, jump);
        }

        Collider2D[] collisions = Physics2D.OverlapBoxAll(transform.position, hitBox.size, 0); // Gets all colliders around the character
        foreach (Collider2D collision in collisions) { // Checks if the collider belongs to an enemy
            if (collision.tag == "Enemy" && !roll)
            {
                Damage(collision);
            }
        }
    }

    private void Damage(Collider2D collision)
    {
        if (!invincible)
        {
            controlLoss = true;
            health -= damageTaken;
            if (health <= 0) // Nenu screen if player is dead
            {
                Death();
            }
            else
            {
                StartCoroutine(switchColor(Color.red, damagedTime)); // Flashes the color of the character
                controller.bounceBack(collision.transform); // Bounces character away from the damage source
            }
        }
    }

    // Changes the color when damaged 
    IEnumerator switchColor(Color color, float time)
    {
        renderer.color = color;
        invincible = true;
        yield return new WaitForSeconds(time);
        renderer.color = Color.white;
        invincible = false;
        controlLoss = false;
    }

    // Loads death screen 
    void Death()
    {
        if (!SceneManager.GetSceneByName("Death").isLoaded)
        {
            SceneManager.LoadScene(2, LoadSceneMode.Additive);
        }
    } 
}
