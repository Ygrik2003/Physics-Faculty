using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static System.Math;

public class BackgroundController : MonoBehaviour
{
    [SerializeField] private float stepAngle = 0.01f;
    [SerializeField] private float radius = 1f;

    private Transform transformSprite;

    private float angle = 0;
    private float baseScale = 1.05f;
    private float scaleX, scaleY;
    void Start()
    {
        transformSprite = gameObject.transform;
        //transformSprite.localScale *= baseScale;
    }

    void FixedUpdate()
    {
        scaleX = (float)Screen.width / Screen.height;
        scaleY = (float)Screen.height / Screen.width;


        if (scaleX >= 16f / 9f)
        {
            scaleY = baseScale;
            GetComponentInParent<CanvasScaler>().matchWidthOrHeight = 0;
        }
        else
        {
            scaleX = baseScale;
            GetComponentInParent<CanvasScaler>().matchWidthOrHeight = 1;

        }

        transformSprite.localPosition = new Vector3(scaleX * radius * (float)Cos(angle), scaleY * radius * (float)Sin(angle), 0);
        angle += ( (Random.Range(0f, 1f) > 0.5f ? -1 : 1) * stepAngle * Random.Range(0.7f, 1f)  * Time.fixedDeltaTime) % 360;
    }

}
