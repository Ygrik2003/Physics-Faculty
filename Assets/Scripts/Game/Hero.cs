using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Math;
using static System.Convert;

public class Hero : MonoBehaviour
{
    [SerializeField] private float speed = 0.7f;
    [SerializeField] private float jumpForce = 0.1f;
    [SerializeField] private float runCD = 0.1f, jumpCD = 0.5f;

    private Rigidbody2D rb;
    private Collider2D[] res = new Collider2D[3];
    private ContactFilter2D filter;
    private SpriteRenderer sprite;

    private bool canRun = false, canJump = false, canDoubleJump = true;
    private int count = 0;
    private bool touchesSmth = false;


    void Start()
    {
        filter.NoFilter();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }


    private void Run()
    {
        rb.velocity = new Vector2(speed * Input.GetAxis("Horizontal"), rb.velocity.y);

        sprite.flipX = Input.GetAxis("Horizontal") < 0;
    }

    private void Jump()
    {
        rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        if (!canJump)
        {
            canDoubleJump = false;
        }

    }

    private void CheckGround()
    {
        for (int i = 0; i < count; i++)
        {
            res[i] = new Collider2D();
        }

        count = rb.OverlapCollider(filter, res);
        touchesSmth = !(count == 0);

        if (!touchesSmth)
        {
            canRun = false;
            canJump = false;
            return;
        }

        for (int i = 0; i < count; i++)
        {
            if (res[i].attachedRigidbody.gameObject.name == "Ground")
            {
                canRun = true;
                canJump = true;
                canDoubleJump = true;
                break;
            }
        }
    }
    void Update()
    {
        if (Input.GetKeyDown("space") && (canJump || canDoubleJump))
        {
            Jump();
        }

        if (Abs(Input.GetAxis("Horizontal")) > 0.0f && canRun)
            Run();


        CheckGround();
    }
}