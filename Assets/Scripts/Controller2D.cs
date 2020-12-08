using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Controller2D : MonoBehaviour
{
    [SerializeField] private LayerMask tileLayer; // Layermask so that we only look at tiles when checking for the ground

    private BoxCollider2D boxCollider; // Collider for terrain
    private Rigidbody2D rb; // Physics controller to move the object
    private float jumpForce; // The jump height multiplier
    private float crouchSpeed; // Speed reduction when crouching
    private float castError; // Leeway for the ground hitbox
    private bool facingRight; // Whether the model is facing positive x values
    public bool grounded; // Whether the object is standing on the ground
    private Vector3 zeroVelocity; 

    public UnityEvent LandEvent; // Stops the jumping animation

    public void Awake()
    {
        jumpForce = 200.0f;
        crouchSpeed = 0.5f;
        castError = 0.1f;
        facingRight = true;
        grounded = true;
        zeroVelocity = Vector3.zero;
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Called for every physics frame
    void FixedUpdate()
    {
        checkColliders(); // Checks if the player is on the ground
    }

    // Checks if the object is standing on the ground 
    // If the player is landing after jumping, the jumping animation is stopped
    private void checkColliders()
    {
        bool prevFrameInAir = grounded; // Previous frame state
        grounded = false;
        RaycastHit2D[] collisions = Physics2D.BoxCastAll(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, castError, tileLayer); // Array of collisions around the character
        if (collisions != null && collisions.Length != 0) // If the array is not empty or null
        {
            grounded = true;
            if (prevFrameInAir) // Character is landing
            {
                LandEvent.Invoke();
            }
        }
    }

    // Moves the character
    public void Move(float xVelocity, bool crouch, bool jump)
    {
        if (crouch)
        {
            if (!grounded) // In the air
            {
                Crash(); // Land very quickly
            }
            else
            {
                xVelocity *= crouchSpeed; // Movement speed is halved
            }   
        }
        if ((xVelocity > 0 && !facingRight) || (xVelocity < 0 && facingRight)) // Going left while facing right and vice versa
        {
            Flip(); // Flip the player model
        }
        if (grounded && jump)
        {
            Jump(); 
        }
        Vector3 target = new Vector2(xVelocity, rb.velocity.y); // The target velocity to move towards based on input.
        rb.velocity = Vector3.SmoothDamp(rb.velocity, target, ref zeroVelocity, 0.1f); // Transitions the current velocity to the target smoothly
    }

    // Character jumps
    private void Jump()
    {
        grounded = false;
        rb.AddForce(new Vector2(0f, jumpForce)); // Applies force upwards to the character
    }

    // Drops the character to the ground
    private void Crash()
    {
        rb.AddForce(new Vector2(0f, int.MinValue)); // Applies maximum downward force 
    }

    // Character model flips direction
    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f); // Rotates the model 180 degrees
    }

    // Bounces the character in the opposite direction of the damage source
    public void bounceBack(Transform damageSource)
    {
        rb.velocity = new Vector2((transform.position.x - damageSource.position.x) * 5, rb.velocity.y); 
    } 

    // Stops the character's movement
    public void stopMovement()
    {
        rb.velocity = Vector2.zero;
    }
}