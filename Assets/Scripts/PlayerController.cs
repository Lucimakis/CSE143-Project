using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject playerPrefab;
    private int health;
    private int damageTaken;
    public float damagedTime = 0.3f;
    public float speed = 250.0f;
    public CapsuleCollider2D hitBox;
    private Controller2D controller;
    private Animator animator;
    private SpriteRenderer renderer;
    private float xVelocity;
    private bool jump = false;
    private bool crouch = false;
    private bool roll = false;
    private Vector3 spawn;
    private bool controlLoss;

    public void Awake()
    {
        controller = GetComponent<Controller2D>();
        renderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    public void Start()
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
        xVelocity = Input.GetAxisRaw("Horizontal") * speed;
        animator.SetFloat("Speed", Mathf.Abs(xVelocity));
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
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            roll = true;
            animator.SetBool("Roll", true);
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
        if (!controlLoss)
        {
            controller.Move(xVelocity * Time.deltaTime, crouch, jump, roll);
        }
    }

    void OnTriggerEnter2D(Collider2D hitObject)
    {
        Enemy enemy = hitObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            controlLoss = true;
            health -= damageTaken;
            if (health <= 0)
            {
                Respawn();
            }
            StartCoroutine(switchColor(Color.red));
            controller.bounceBack(hitObject.transform);
        }
    }

    IEnumerator switchColor(Color color)
    {
        renderer.color = color;
        yield return new WaitForSeconds(damagedTime);
        renderer.color = Color.white;
        controlLoss = false;
    }

    void Respawn()
    {
        Instantiate(playerPrefab, spawn, Quaternion.identity);
        Destroy(gameObject);
    } 
}
