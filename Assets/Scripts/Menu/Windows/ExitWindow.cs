using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitWindow : MonoBehaviour
{

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void CancelClick()
    {
        gameObject.SetActive(false);
    }

    public void ExitClick()
    {
        Application.Quit();
    }
}
