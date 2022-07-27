using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSource : MonoBehaviour
{
    private static MusicSource instance = null;
    private static AudioSource source;

    private void Start()
    {
        if (instance != null)
            return ;
        instance = this;
        source = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
    }
    public static void Play(AudioClip clip)
    {
        source.volume = SettingsManager.settings.volumeMusic * SettingsManager.settings.volumeMaster;
        source.PlayOneShot(clip);
    }
}
