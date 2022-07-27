using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Math;

public class MenuButton : MonoBehaviour
{
    private bool isWiggled = false;
    private float rotation = 0.0f;

    [SerializeField] private float rotateStep = 0.01f;
    [SerializeField] private float rotateMax = 3.0f;

    [SerializeField] AudioClip highlightedClip;
    [SerializeField] AudioClip pressedClip;



    public void Highlighted()
    {
        SoundSource.Play(highlightedClip);
    }
    public void Pressed()
    {
        SoundSource.Play(pressedClip);
    }

    void Start()
    {
        //SoundSource.source = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        if (isWiggled)
        {
            transform.Rotate(0, 0, rotateStep);
            rotation += rotateStep;
            if (Abs(rotation) > rotateMax)
                rotateStep = -rotateStep;
        }
    }

    public void setWiggled(bool isWiggled)
    {
        this.isWiggled = isWiggled;
    }
}
