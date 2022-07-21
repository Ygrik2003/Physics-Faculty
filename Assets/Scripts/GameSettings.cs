using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameSettings : MonoBehaviour
{
    //GameSettings Instance { get; set; }

    [Serializable]
    public struct Settings
    {
        public int FPS;
        
        public int widthScreen;
        public int heightScreen;
        public bool isFullSreen;
        
        public float volumeMaster;
        public float volumeSound;
        public float volumeMusic;
    }

    public Settings settings;
    public void saveSettings()
    {
        Debug.Log(JsonUtility.ToJson(settings));
        PlayerPrefs.SetString("Settings", JsonUtility.ToJson(settings));
    }

    void Start()
    {
        if (PlayerPrefs.HasKey("Settings"))
        {
            settings = JsonUtility.FromJson<Settings>(PlayerPrefs.GetString("Settings"));
        }
        else
        {
            settings.FPS = 60;

            settings.widthScreen = 1600;
            settings.heightScreen = 900;
            settings.isFullSreen = true;

            settings.volumeMaster = 1;
            settings.volumeSound = 1;
            settings.volumeMusic = 1;

            saveSettings();
        }

        Screen.SetResolution(settings.widthScreen, settings.heightScreen, settings.isFullSreen);
        Time.fixedDeltaTime = 1f / settings.FPS;
        Application.targetFrameRate = settings.FPS;

        PlayerPrefs.DeleteAll();

    }
    void Update()
    {
        
    }
}
