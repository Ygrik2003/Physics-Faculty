using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Convert;

public class Hero : MonoBehaviour
{
    [SerializeField] private float speed = 0.5f;
    [SerializeField] private float jumpForce = 0.1f;
    Rigidbody2D rb;
    Collider2D[] res;
    private float lastRun = 0, lastJump = 0;
    [SerializeField] private float runCD = 0.1f, jumpCD = 0.5f;
    private bool mirrored = false;
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void RunToRight()
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
    }

    private void Jump()
    {
        if ((Time.timeSinceLevelLoad - lastJump) > jumpCD){
            rb.velocity += new Vector2(0, jumpForce);
            lastJump = Time.timeSinceLevelLoad;
        }
    }

    private void Mirror(){
        mirrored = !mirrored;
        transform.RotateAround(this.transform.position, Vector3.up, 180);
    }

    private void CheckGround() 
    {

    }
    void FixedUpdate()
    {
        if (Input.GetKey("space"))
        {
            Jump();
        }

        if (Input.GetKey("d"))
        {
            RunToRight();
        }

        if (Input.GetKey("a"))
        {
            RunToLeft();
        }


        if ( ( (rb.velocity.x > 0) && !mirrored ) || ( (rb.velocity.x < 0) && mirrored ) ){
           Mirror();
        } 

    }
}
