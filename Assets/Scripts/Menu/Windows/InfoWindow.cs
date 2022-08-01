using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoWindow : MonoBehaviour
{
    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void BackClick()
    {
        gameObject.SetActive(false);
    }
}
