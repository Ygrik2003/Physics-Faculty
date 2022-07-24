using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Math;

public class BackgroundController : MonoBehaviour
{
    [SerializeField] private float scaleX = 1.3f;
    [SerializeField] private float maxRadius = 0.4f;
    [SerializeField] private float stepAngle = 0.01f;
    private float angle = 0;
    private float radius;
    void Start()
    {
        transform.localScale *= 1.1f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        radius = (float)Pow(maxRadius, 2f / angle);
        transform.position = new Vector3(scaleX * maxRadius * (float)Cos(angle), maxRadius * (float)Sin(angle), transform.position.z);
        //transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, stepAngle * Time.fixedDeltaTime);
        angle += stepAngle * Time.fixedDeltaTime; //Can was error after some time
    }

}
