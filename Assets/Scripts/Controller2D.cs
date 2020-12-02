using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Controller2D : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private float jumpForce = 125.0f;
    private float dashForce = 2f;
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
            if (collision != boxCollider)
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
        /*int direction = -1;
        if (facingRight)
        {
            direction = 1;
        }
        rb.AddForce(new Vector2(dashForce * direction, 0f), ForceMode2D.Impulse);*/

    }

    private void Crash()
    {
        rb.AddForce(new Vector2(0f, int.MinValue));
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}