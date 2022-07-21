using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitWindow : MonoBehaviour
{

    private void Awake()
    {
        setVisible(false);
    }

    public void setVisible(bool isVisible)
    {
        gameObject.SetActive(isVisible);
    }

    public void CancelClick()
    {
        setVisible(false);
    }

    public void ExitClick()
    {
        Application.Quit();
    }
}
