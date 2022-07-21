using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSource : MonoBehaviour
{
    AudioSource source;
    [SerializeField] public AudioClip clip;
    GameSettings gameSettings;

    void Start()
    {
        source = GetComponent<AudioSource>();
        if (Resources.FindObjectsOfTypeAll<GameSettings>().Length == 1)
            gameSettings = Resources.FindObjectsOfTypeAll<GameSettings>()[0];
        else if (Resources.FindObjectsOfTypeAll<GameSettings>().Length == 0)
            Debug.Log("This scene haven't object GameSettings");
        else if (Resources.FindObjectsOfTypeAll<GameSettings>().Length > 1)
        {
            gameSettings = Resources.FindObjectsOfTypeAll<GameSettings>()[0];
            Debug.Log($"This scene have several object's GameSettings");

        }
        gameSettings.settings.volumeMaster = 0.5f; gameSettings.saveSettings(); 
        source.volume = gameSettings.settings.volumeMaster;
        Debug.Log(source.volume);
    }
    void Play()
    {
        source.volume *= gameSettings.settings.volumeSound;
        source.PlayOneShot(clip);
        source.volume = gameSettings.settings.volumeMaster;
    }
}
