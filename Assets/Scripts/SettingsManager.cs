using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SettingsManager : MonoBehaviour
{
    private static SettingsManager instance = null;

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

    public static Settings settings;
    public static void saveSettings()
    {
        Debug.Log("Saving settings:\n" + JsonUtility.ToJson(settings));
        PlayerPrefs.SetString("Settings", JsonUtility.ToJson(settings));
    }

    void Start()
    {
        if (instance != null)
            return ;
        instance = this;
        
        DontDestroyOnLoad(gameObject);

        InitManager();
    }
    void Update()
    {
        //PlayerPrefs.DeleteAll();
    }

    private void InitManager()
    {
        if (PlayerPrefs.HasKey("Settings"))
        {
            Debug.Log("Import settings from PlayerPrefs");
            settings = JsonUtility.FromJson<Settings>(PlayerPrefs.GetString("Settings"));
        }
        else
        {
            Debug.Log("Set settings as default");
            settings.FPS = 60;

            settings.widthScreen = 900;
            settings.heightScreen = 450;
            settings.isFullSreen = false;

            settings.volumeMaster = 1;
            settings.volumeSound = 1;
            settings.volumeMusic = 1;

            saveSettings();
        }

        Screen.SetResolution(settings.widthScreen, settings.heightScreen, settings.isFullSreen);
        Time.fixedDeltaTime = 1f / settings.FPS;
        Application.targetFrameRate = settings.FPS;
    }
}
