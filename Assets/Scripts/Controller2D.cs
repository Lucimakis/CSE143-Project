using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Controller2D : MonoBehaviour
{
    public CameraShake shake; // Camera
    private BoxCollider2D boxCollider; // Collider for terrain
    private Rigidbody2D rb; // Physics controller to move the object
    private float jumpForce; // The jump height multiplier
    private float crouchSpeed; // Speed reduction when crouching
    private bool facingRight; // Whether the model is facing positive x values
    private bool grounded; // Whether the object is standing on the ground
    private Vector3 zeroVelocity; 

    public UnityEvent LandEvent;

    public void Awake()
    {
        jumpForce = 125.0f;
        crouchSpeed = 0.5f;
        facingRight = true;
        zeroVelocity = Vector3.zero;
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Checks if the object is standing on the ground using collisions
    void FixedUpdate()
    {
        bool fromAir = grounded;
        grounded = false;
        Collider2D[] collisions = Physics2D.OverlapBoxAll(transform.position, boxCollider.size, 0); // array of colliders at the current position and collider size
        foreach (Collider2D collision in collisions)
        {
            if (collision != boxCollider && 
                collision != gameObject.GetComponent<CapsuleCollider2D>() && 
                collision.GetComponent<Enemy>() == null) // Checks if the collision is not the player's own collider, hitbox, or an enemy
            {
                grounded = true; // Collision must be with the ground
                if (fromAir) // The player lands on the ground
                {
                    LandEvent.Invoke(); // Stop jumping
                }
            }
        }
    }

    // Moves the character
    public void Move(float xVelocity, bool crouch, bool jump, bool roll)
    {
        if (roll) 
        {
            Roll();
        }
        if (grounded) // Movement is only allowed on the ground
        {
            if (crouch)
            {
                xVelocity *= crouchSpeed;
            }
            Vector3 target = new Vector2(xVelocity, rb.velocity.y); // The target velocity to move towards based on input
            rb.velocity = Vector3.SmoothDamp(rb.velocity, target, ref zeroVelocity, 0f); // Transitions the current velocity to the target smoothly
            if ((xVelocity > 0 && !facingRight) || (xVelocity < 0 && facingRight)) // Swaps the direction the model is facing 
            {
                Flip();
            }
            if (jump)
            {
                Jump();
            }
        }
        else if (crouch) // Crouching in midair slams into the ground
        {
            Crash();
        }
    }

    // Character jumps
    private void Jump()
    {
        grounded = false;
        rb.AddForce(new Vector2(0f, jumpForce));
    }

    // Character rolls
    private void Roll()
    {
        // shrink hitbox logic
    }

    // Drops the character to the ground and adds screen shake
    private void Crash()
    {
        rb.AddForce(new Vector2(0f, int.MinValue));
        StartCoroutine(shake.screenShake(0.2f, 0.2f));
    }

    // Character model flips direction
    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    // Bounces the character in the opposite direction of the damage source
    public void bounceBack(Transform damageSource)
    {
        rb.velocity = new Vector2((transform.position.x - damageSource.position.x) * 5, rb.velocity.y);
    } 
}