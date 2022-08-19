using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Math;

public class MenuButton : MonoBehaviour
{
    private bool isWiggled = false;
    private Vector3 rotation = new Vector3(0,0,0);

    [SerializeField] private Vector3 rotationSpeed;
    [SerializeField] private Vector3 rotationMax;

    [SerializeField] AudioClip highlightedClip;
    [SerializeField] AudioClip pressedClip;

    [SerializeField] public GameObject objToShow;
    [SerializeField] public bool OverlapingWindow;

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
            transform.Rotate( rotationSpeed * Time.fixedDeltaTime);
            rotation += rotationSpeed * Time.fixedDeltaTime;
            if (Abs(rotation.x) > rotationMax.x){
                rotationSpeed.x *= -1;
                rotation.x = Sign(rotation.x) * rotationMax.x;
            }
            if (Abs(rotation.y) > rotationMax.y){
                rotationSpeed.y *= -1;
                rotation.y = Sign(rotation.y) * rotationMax.y;
            }
            if (Abs(rotation.z) > rotationMax.z){
                rotationSpeed.z *= -1;
                rotation.z = Sign(rotation.z) * rotationMax.z;
            }
        }
    }

    public void setWiggled(bool isWiggled)
    {
        this.isWiggled = isWiggled;
    }
}
