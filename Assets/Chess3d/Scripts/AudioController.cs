using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioSource audioSource;
    private static float volume = 0.5f;

    public static float Volume
    {
        get => volume;
        set
        {
            volume = value;
            if(audioSource != null)
            {
                audioSource.volume = value;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if(FileHandler.GameData != null)
        {
            Volume = FileHandler.GameData.MusicVolume;
        }
    }

    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Music");

        if (objs.Length > 1)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
}
