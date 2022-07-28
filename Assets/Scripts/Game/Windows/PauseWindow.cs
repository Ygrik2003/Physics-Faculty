using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseWindow : MonoBehaviour
{
    public void setVisible(bool isVisible)
    {
        gameObject.SetActive(isVisible);
    }

    private void Start()
    {
        setVisible(false);
    }

    public void ExitPressed()
    {
        SceneManager.LoadScene("MenuScene");
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            setVisible(false);

    }
}
