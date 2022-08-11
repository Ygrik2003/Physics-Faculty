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

        public Resolution resolution;
        public bool isFullSreen;
        
        public float volumeMaster;
        public float volumeSound;
        public float volumeMusic;
    }
    [Serializable]
    public class Resolution
    {
        public int width;
        public int height;
        public Resolution(int width, int height)
        {
            this.width = width;
            this.height = height;
        }


        public override string ToString()
        {
            return width.ToString() + "x" + height.ToString();
        }
    }

    public static Settings settings;
    public static void saveSettings()
    {
        Debug.Log("Saving settings:\n" + JsonUtility.ToJson(settings));
        PlayerPrefs.SetString("Settings", JsonUtility.ToJson(settings));

        Screen.SetResolution(settings.resolution.width, settings.resolution.height, settings.isFullSreen);
        Time.fixedDeltaTime = 1f / (settings.FPS > 0 ? settings.FPS : 1000);
        Application.targetFrameRate = settings.FPS;
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

    public static void InitManager()
    {
        if (PlayerPrefs.HasKey("Settings"))
        {
            Debug.Log("Import settings from PlayerPrefs");
            settings = JsonUtility.FromJson<Settings>(PlayerPrefs.GetString("Settings"));

            Debug.Log(JsonUtility.ToJson(settings));

            Screen.SetResolution(settings.resolution.width, settings.resolution.height, settings.isFullSreen);
            Time.fixedDeltaTime = settings.FPS==0f ? 0.001f : 1f / settings.FPS; //Temp workaround, needs to be fixed
            Application.targetFrameRate = settings.FPS;
        }
        else
        {
            Debug.Log("Set settings as default");
            settings.FPS = 60;

            settings.resolution = new Resolution(1600, 900);
            settings.isFullSreen = false;

            settings.volumeMaster = 1;
            settings.volumeSound = 1;
            settings.volumeMusic = 1;

            saveSettings();
        }
    }
}
