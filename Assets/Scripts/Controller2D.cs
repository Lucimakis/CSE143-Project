using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Controller2D : MonoBehaviour
{
    public CameraShake shake;
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb;
    private float jumpForce = 125.0f;
    private float crouchSpeed = 0.5f;
    private bool facingRight = true;
    private bool grounded;
    private Vector3 m_velocity = Vector3.zero;

    public UnityEvent LandEvent;

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void FixedUpdate()
    {
        bool fromAir = grounded;
        grounded = false;
        Collider2D[] collisions = Physics2D.OverlapBoxAll(transform.position, boxCollider.size, 0); 
        foreach (Collider2D collision in collisions)
        {
            if (collision != boxCollider && collision != gameObject.GetComponent<CapsuleCollider2D>() && collision.GetComponent<Enemy>() == null)
            {
                grounded = true;
                if (fromAir)
                {
                    LandEvent.Invoke();
                }
            }
        }
    }

    public void Move(float xVelocity, bool crouch, bool jump, bool roll)
    {
        if (roll)
        {
            Roll();
        }
        if (grounded)
        {
            if (crouch)
            {
                xVelocity *= crouchSpeed;
            }
            Vector3 target = new Vector2(xVelocity, rb.velocity.y);
            rb.velocity = Vector3.SmoothDamp(rb.velocity, target, ref m_velocity, 0f);
            if ((xVelocity > 0 && !facingRight) || (xVelocity < 0 && facingRight))
            {
                Flip();
            }
            if (jump)
            {
                Jump();
            }
        }
        else if (crouch)
        {
            Crash();
        }
    }

    private void Jump()
    {
        grounded = false;
        rb.AddForce(new Vector2(0f, jumpForce));
    }

    private void Roll()
    {
        // shrink hitbox logic
    }

    private void Crash()
    {
        rb.AddForce(new Vector2(0f, int.MinValue));
        StartCoroutine(shake.screenShake(0.2f, 0.2f));
    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    public void bounceBack(Transform damageSource)
    {
        rb.velocity = new Vector2((transform.position.x - damageSource.position.x) * 5, rb.velocity.y);
    } 
}