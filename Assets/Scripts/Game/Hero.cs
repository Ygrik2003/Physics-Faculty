using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    public Vector2 moveVector;
    public int speed = 5;

    public int jumpForce = 10;
    private bool jumpControl;
    private int jumpIteration = 0;
    public int jumpValueIteration = 60; //fps

    public int lungeImpulse = 5000;
    private bool lockLunge = false;

    public bool faceRight = true;
    public Rigidbody2D rb;

    //We will have animations later
    //public Animator anim;

    public bool onGround;
    public Transform GroundCheck;
    private float GroundCheckRadius;
    public LayerMask Ground;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //anim = GetComponent<Animator>();
        GroundCheckRadius = GroundCheck.GetComponent<CircleCollider2D>().radius;
    }

    private void Update()
    {
        Walk();
        Reflect();
        Jump();
        Lunge();
        CheckingGround();
    }

    void Walk()
    {
        moveVector.x = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveVector.x * speed, rb.velocity.y);

        //anim.SetFloat("moveX", Mathf.Abs(moveVector.x));
    }

    void Reflect()
    {
        if ((moveVector.x > 0 && !faceRight) || (moveVector.x < 0 && faceRight))
        {
            transform.localScale *= new Vector2(-1, 1);
            faceRight = !faceRight;
        }
    }

    void Jump()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            //rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            if (onGround)
            {
                jumpControl = true;
            }
        }
        else
        {
            jumpControl = false;
        }
        if (jumpControl)
        {
            if (jumpIteration++ < jumpValueIteration)
            {
                rb.AddForce(Vector2.up * jumpForce / jumpIteration);
            }
        }
        else
        {
            jumpIteration = 0;
        }
    }

    void Lunge()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !lockLunge)
        {
            //anim.StopPlayback();
            //anim.Play("lunge");

            lockLunge = true;
            Invoke("LungeLock", 2f);

            rb.velocity = new Vector2(0, 0);

            if (!faceRight)
            {
                rb.AddForce(Vector2.left * lungeImpulse);
            }
            else
            {
                rb.AddForce(Vector2.right * lungeImpulse);
            }
        }
    }

    void LungeLock()
    {
        lockLunge = false;
    }

    void CheckingGround()
    {
        onGround = Physics2D.OverlapCircle(GroundCheck.position, GroundCheckRadius, Ground);
        //anim.SetBool("onGround", onGround);
    }
}