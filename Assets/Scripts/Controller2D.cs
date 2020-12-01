using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller2D : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool grounded;
    private bool facingRight;
    private BoxCollider2D boxCollider;
    private float crouchSpeed;
    private Vector3 m_velocity = Vector3.zero;

    // Start is called before the first frame update
    public void Awake()
    {
        grounded = false;
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        facingRight = true;
    }
    void FixedUpdate()
    {
        Collider2D[] collisions = Physics2D.OverlapBoxAll(transform.position, boxCollider.size, 0);
        for (int i = 0; i < collisions.Length; i++)
        {
            if (collisions[i] != boxCollider)
            {
                grounded = true;
            }
        }
    }

    public void Move(Vector2 velocity, bool crouch, bool dash, bool jump) 
    {
        if (crouch)
        {
            velocity = velocity / 2;
        }
        Vector3 target = new Vector2(velocity.x * 10f, rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, target, ref m_velocity, 0f);
        if ((velocity.x > 0 && !facingRight) || (velocity.x < 0 && facingRight)) 
        {
            Flip();
        }
        if (grounded && jump)
        {
            grounded = false;
            rb.AddForce(new Vector2(0f, 1000f));
        }
        if (dash)
        {
            int direction = -1;
            if (facingRight)
            {
                direction = 1;
            } 
            rb.AddForce(new Vector2(20f * direction, 0f), ForceMode2D.Impulse);
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}
