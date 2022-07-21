using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainWindow : MonoBehaviour
{

    public void setVisible(bool isVisible)
    {
        gameObject.SetActive(isVisible);
    }
    public void StartPressed()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void InfoPressed()
    {
        setVisible(false);
    }
    public void SettingsPressed()
    {
        setVisible(false);
    }
}
