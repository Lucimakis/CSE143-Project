using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 250.0f;
    private Controller2D controller;
    private Animator animator;
    private SpriteRenderer renderer;
    private float xVelocity;
    private bool jump = false;
    private bool crouch = false;
    private bool roll = false;

    public AnimationEvent ae;

    public void Awake()
    {
        controller = GetComponent<Controller2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        xVelocity = Input.GetAxisRaw("Horizontal") * speed;
        animator.SetFloat("Speed", Mathf.Abs(xVelocity));

        /*if (Input.GetKeyDown("down"))
        {
            crouch = true;
            animator.SetBool("Crouch", true);
        } else if (Input.GetKeyUp("down"))
        {
            crouch = false;
            animator.SetBool("Crouch", false);
        }
        if (Input.GetKeyDown("up"))
        {
            jump = true;
            animator.SetBool("Jump", true);
        } else if (Input.GetKeyUp("up"))
        {
            jump = false;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            roll = true;
            animator.SetBool("Roll", true);
        } else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            roll = false;
            animator.SetBool("Roll", false);
        }*/

        if (Input.GetKeyDown("up"))
        {
            jump = true;
            animator.SetBool("Jump", true);
        }
        else if (Input.GetKeyUp("up"))
        {
            jump = false;
        }
        crouch = Input.GetKey("down");
        animator.SetBool("Crouch", crouch);

        /*roll = Input.GetKey(KeyCode.LeftShift);
        animator.SetBool("Roll", roll);*/

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            roll = true;
            animator.SetBool("Roll", true);
            ae.functionName = "rollFinish";
        }
    }

    public void rollFinish()
    {
        roll = false;
        animator.SetBool("Roll", false);
    }

    public void OnLanding()
    {
        animator.SetBool("Jump", false);
    }

    void FixedUpdate()
    {
        controller.Move(xVelocity * Time.deltaTime, crouch, jump, roll);
    }
}
