using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Math;

public class MenuButton : MonoBehaviour
{
    public enum BtnType
    {
        Start,
        Settings,
        Exit,
        Info
    }
    
    private bool isWiggled = false;
    private float rotation = 0.0f;

    [SerializeField] private float rotateStep = 1f;
    [SerializeField] private float rotateMax = 3.0f;

    [SerializeField] AudioClip highlightedClip;
    [SerializeField] AudioClip pressedClip;

    [SerializeField] public BtnType type;



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
            //transform.RotateAround(new Vector3(0.5f, 0.5f, 0.5f), new Vector3(0, 0, 1), 20 * Time.fixedDeltaTime); //wtf
            transform.Rotate(0, 5 * Abs(rotateStep) * Time.fixedDeltaTime, rotateStep * Time.fixedDeltaTime);
            rotation += rotateStep * Time.fixedDeltaTime;
            if (Abs(rotation) > rotateMax)
                rotateStep = -rotateStep;
        }
    }

    public void setWiggled(bool isWiggled)
    {
        print(type.ToString() + " is rotate: " + isWiggled);
        this.isWiggled = isWiggled;
    }
}
