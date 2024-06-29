using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private AudioClip[] clips;

    [Space]
    [SerializeField] private AudioSource musicAudioSource;

    [SerializeField] private AudioSource sfxAudioSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    private void Update()
    {
        if (!musicAudioSource.isPlaying)
        {
            PlayMusic();
        }
    }

    private void PlayMusic()
    {
        musicAudioSource.clip = clips[0];
        musicAudioSource.Play();
    }
}