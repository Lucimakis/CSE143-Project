using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 2.0f;
    private Controller2D controller;
    private Vector2 velocity;
    private bool jump;
    private bool crouch;

    public void Awake()
    {
        controller = GetComponent<Controller2D>();
    }

    void FixedUpdate()
    {
        velocity = new Vector2(Input.GetAxis("Horizontal") + speed * Time.deltaTime, 0);
        jump = Input.GetKey("up");
        crouch = Input.GetKey("down");
        controller.Move(velocity, crouch, false, jump);
    }
}
