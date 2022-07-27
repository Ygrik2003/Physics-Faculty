using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsWindow : MonoBehaviour
{
    [SerializeField] private GameObject settingFSM;//FullScreenMode
    [SerializeField] private GameObject settingResolution;
    [SerializeField] private GameObject settingFPS;
    [SerializeField] private GameObject settingMasterVolume;
    [SerializeField] private GameObject settingMusicVolume;
    [SerializeField] private GameObject settingSoundVolume;

    SettingsManager.Resolution[] resolutions =
    {
        new SettingsManager.Resolution(1920, 1080),
        new SettingsManager.Resolution(1600, 900),
        new SettingsManager.Resolution(1366, 768),
        new SettingsManager.Resolution(1280, 1024),
        new SettingsManager.Resolution(1200, 900),
    };

    private void Awake()
    {
        foreach (SettingsManager.Resolution res in resolutions)
            settingResolution.GetComponentInChildren<TMP_Dropdown>().options.Add(new TMP_Dropdown.OptionData() { text = res.ToString() });
        setVisible(false);
        
    }

    public void setVisible(bool isVisible)
    {
        gameObject.SetActive(isVisible);
        if (isVisible)
        {
            InitSliderEvents(settingFPS, SettingsManager.settings.FPS);
            InitSliderEvents(settingMasterVolume, SettingsManager.settings.volumeMaster);
            InitSliderEvents(settingMusicVolume, SettingsManager.settings.volumeMusic);
            InitSliderEvents(settingSoundVolume, SettingsManager.settings.volumeSound);

            settingResolution.GetComponentInChildren<TMP_Dropdown>().value = Utils.IndexOf(resolutions, SettingsManager.settings.resolution);
            
            settingFSM.GetComponentInChildren<Toggle>().isOn = SettingsManager.settings.isFullSreen;
        }
    }

    private void InitSliderEvents(GameObject obj, float InitValue)
    {
        Slider tempSlider;
        TMP_InputField tempInputField;

        tempSlider = obj.GetComponentInChildren<Slider>();
        tempInputField = obj.GetComponentInChildren<TMP_InputField>();

        tempSlider.value = InitValue;
        tempInputField.text = InitValue.ToString();

        tempSlider.onValueChanged.AddListener((v) =>
        {
            if (tempInputField.contentType == TMP_InputField.ContentType.IntegerNumber)
                tempInputField.text = v.ToString("0");
            else
                tempInputField.text = v.ToString("0.00");
        });
        tempInputField.onValueChanged.AddListener((v) =>
        {
            if ((v[0] == '-') || (v == " "))
            {
                tempInputField.text = tempSlider.minValue.ToString();
                tempSlider.value = tempSlider.minValue;
            }
            else if ((float)Convert.ToDouble(v) > tempSlider.maxValue)
            {
                tempInputField.text = tempSlider.maxValue.ToString();
                tempSlider.value = tempSlider.maxValue;
            }
            else if ((float)Convert.ToDouble(v) < tempSlider.minValue)
            { 
                tempInputField.text = tempSlider.minValue.ToString();
                tempSlider.value = tempSlider.minValue;
            }
            else
            {
                tempSlider.value = (float)Convert.ToDouble(v);
            }
        });
    }
    
    public void SavePressed()
    {
        String resolution;
        resolution = settingResolution.GetComponentInChildren<TMP_Dropdown>().captionText.text;

        SettingsManager.settings.FPS = Convert.ToInt16(settingFPS.GetComponentInChildren<TMP_InputField>().text);

        SettingsManager.settings.resolution.width = Convert.ToInt16(resolution.Split('x')[0]);
        SettingsManager.settings.resolution.height = Convert.ToInt16(resolution.Split('x')[1]);

        SettingsManager.settings.isFullSreen = settingFSM.GetComponentInChildren<Toggle>().isOn;

        SettingsManager.settings.volumeMaster = (float)Convert.ToDouble(settingMasterVolume.GetComponentInChildren<TMP_InputField>().text);
        SettingsManager.settings.volumeSound = (float)Convert.ToDouble(settingSoundVolume.GetComponentInChildren<TMP_InputField>().text);
        SettingsManager.settings.volumeMusic = (float)Convert.ToDouble(settingMusicVolume.GetComponentInChildren<TMP_InputField>().text);

        SettingsManager.saveSettings();
    }
}
