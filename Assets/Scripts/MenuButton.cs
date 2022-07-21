using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Math;

public class MenuButton : MonoBehaviour
{
    private bool isWiggled = false;
    private float rotation = 0.0f;
    private AudioSource audioSource;

    [SerializeField] private float rotateStep = 0.01f;
    [SerializeField] private float rotateMax = 3.0f;

    [SerializeField] private AudioClip highlightedSound;
    [SerializeField] private AudioClip pressedSound;

    public void Highlighted()
    {
        audioSource.PlayOneShot(highlightedSound);
    }
    public void Pressed()
    {
        audioSource.PlayOneShot(pressedSound);
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        //audioSource.volume *= settings.volumeSound;
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
