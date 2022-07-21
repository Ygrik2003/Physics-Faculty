using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Math;

public class BackgroundController : MonoBehaviour
{
    [SerializeField] private float scaleX = 1.3f;
    [SerializeField] private float radius = 1.0f;
    [SerializeField] private float stepAngle = 0.01f;
    private float angle = 0;
    void Start()
    {
        transform.localScale = transform.localScale + new Vector3(3, 3, 3);
        transform.position += new Vector3(scaleX * -2 * radius, 0, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += new Vector3(scaleX * 2 * radius * (float)Sin(angle) * stepAngle, 2 * radius * (float)Cos(angle) * stepAngle, 0);
        angle += stepAngle;
    }

}
