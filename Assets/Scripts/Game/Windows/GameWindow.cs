using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWindow : MonoBehaviour
{
    [SerializeField] GameObject pauseWindow;
    public void setVisible(bool isVisible)
    {
        gameObject.SetActive(isVisible);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            pauseWindow.GetComponent<PauseWindow>().setVisible(true);

    }
}
