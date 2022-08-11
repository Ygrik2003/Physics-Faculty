using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Convert;

public class Hero : MonoBehaviour
{
    [SerializeField] private float speed = 0.5f;
    [SerializeField] private float jumpForce = 0.1f;
    Rigidbody2D rb;
    Collider2D[] res = new Collider2D[3];
    ContactFilter2D filter;
    private float lastRun = 0, lastJump = 0;
    [SerializeField] private float runCD = 0.1f, jumpCD = 0.5f;
    private int direction = 0;
    private bool canRun = false, canJump = false, canDoubleJump = true;
    private int count = 0;
    private bool touchesSmth = false;

    public Vector2 moveVector;
    

    void Start()
    {
        filter.NoFilter();
        rb = GetComponent<Rigidbody2D>();
    }

    void walk()
    {
        moveVector.x = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveVector.x * speed, rb.velocity.y);
    }

    /*private void RunToRight()
    {
        if ((Time.timeSinceLevelLoad - lastRun) > runCD){
            rb.velocity += new Vector2(speed, 0);
            lastRun = Time.timeSinceLevelLoad;
        }
    }

    private void RunToLeft()
    {
        if ((Time.timeSinceLevelLoad - lastRun) > runCD){
            rb.velocity -= new Vector2(speed, 0);
            lastRun = Time.timeSinceLevelLoad;
        }
    }*/

    private void Jump()
    {
        if ((Time.timeSinceLevelLoad - lastJump) > jumpCD){
            if (!canJump){
                canDoubleJump = false;
            }
            rb.velocity += new Vector2(0, jumpForce);
            lastJump = Time.timeSinceLevelLoad;
        }
    }

    private void chooseDirection(int newDirection){
        if (newDirection == direction)
            return;

        direction = newDirection;
        transform.RotateAround(this.transform.position, Vector3.up, 180);
    }

    private void CheckGround() 
    {
        for (int i = 0; i < count; i++)
        {
            res[i] = new Collider2D();
        }

        count = rb.OverlapCollider( filter, res );
        touchesSmth = !(count == 0);

        if (!touchesSmth){
            canRun = false;
            canJump = false;
            return;
        }

        for (int i = 0; i < count; i++){
            if (res[i].attachedRigidbody.gameObject.name == "Ground"){
                canRun = true;
                canJump = true;
                canDoubleJump = true;
                break;
            }
            if (res[i].attachedRigidbody.transform.parent.name == "Tiles")
            {
                canRun = true;
                canJump = true;
                canDoubleJump = true;
                break;
            }
        }
    }
    void FixedUpdate()
    {
        if (Input.GetKey("space") && (canJump || canDoubleJump ) )
        {
            Jump();
        }

        walk();

        /*if (Input.GetKey("d"))
        {
            chooseDirection(1);
            if (canRun)
                RunToRight();
        }

        if (Input.GetKey("a"))
        {
            chooseDirection(0);
            if (canRun)
                RunToLeft();
        }*/


        if (rb.velocity.x > 0){
           chooseDirection(1);
        } else if (rb.velocity.x < 0){
            chooseDirection(0);
        }

        CheckGround();
        Debug.Log(canRun);
    }
}
