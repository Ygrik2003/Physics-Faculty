using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoWindow : MonoBehaviour
{
    private void Awake()
    {
        setVisible(false);
    }

    public void setVisible(bool isVisible)
    {
        gameObject.SetActive(isVisible);
    }

    public void BackClick()
    {
        setVisible(false);
    }
}
