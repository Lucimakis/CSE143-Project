using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject playerPrefab; // The player model and all attached scripts
    private int health; // Amount of health the player has
    private int damageTaken; // Amount of damage enemies give to the player
    private float damagedTime; // Time the character flashes red on damage
    private float speed; // Speed of the character movement
    private CapsuleCollider2D hitBox; // The player's hitbox
    private Controller2D controller; // Controller for movement
    private Animator animator; // Player animation controller 
    private SpriteRenderer renderer; // Player model manager
    private float xVelocity; // Movement speed based on input
    private bool jump; // Whether player is jumping
    private bool crouch; // Whether player is crouching
    private bool roll; // Whether player is rolling
    private Vector3 spawn; // The spawn point 
    private bool controlLoss; // Amount of time the player cannot be controlled

    void Awake()
    {
        jump = false;
        crouch = false;
        roll = false;
        damagedTime = 0.3f;
        speed = 250.0f;
        hitBox = GetComponent<CapsuleCollider2D>();
        controller = GetComponent<Controller2D>();
        renderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        health = 100;
        damageTaken = 40;
        spawn = new Vector2(0f, 5f);
        if (gameObject == null)
        {
            Instantiate(gameObject, spawn, transform.rotation);
        }
    }

    void Update()
    {
        xVelocity = Input.GetAxisRaw("Horizontal") * speed; // Movement based on input side-to-side
        animator.SetFloat("Speed", Mathf.Abs(xVelocity)); // Changes the flag in the animator for speed
        if (Input.GetKeyDown("up")) // Jumps and changes the animation 
        {
            jump = true;
            animator.SetBool("Jump", true);
        }
        else if (Input.GetKeyUp("up")) // No longer jumping but does not change the animation 
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
    void FixedUpdate()
    {
        if (!controlLoss)
        {
            controller.Move(xVelocity * Time.deltaTime, crouch, jump, roll);
        }
    }

    // If the player collides with an enemy, damage is taken
    void OnTriggerEnter2D(Collider2D hitObject)
    {
        Enemy enemy = hitObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            controlLoss = true;
            health -= damageTaken;
            if (health <= 0) // Respawn the player if died
            {
                Respawn();
            }
            StartCoroutine(switchColor(Color.red)); // Flashes the color of the character
            controller.bounceBack(hitObject.transform); // Bounces it backwards away from the damage source
        }
    }

    // Changes the color when damaged 
    IEnumerator switchColor(Color color)
    {
        renderer.color = color;
        yield return new WaitForSeconds(damagedTime);
        renderer.color = Color.white;
        controlLoss = false;
    }

    // Creates a new game object and destroys the old object
    void Respawn()
    {
        Instantiate(playerPrefab, spawn, Quaternion.identity);
        Destroy(gameObject);
    } 
}
