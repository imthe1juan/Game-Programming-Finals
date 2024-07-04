using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Space]
    [SerializeField] private AudioSource musicAudioSource;

    [SerializeField] private AudioSource sfxAudioSource;
    [SerializeField] private AudioClip[] musicClips;

    [Space]
    [SerializeField] private AudioClip hitClip;

    [SerializeField] private AudioClip criticalClip;
    [SerializeField] private AudioClip blockClip;
    [SerializeField] private AudioClip missClip;

    [Space]
    [SerializeField] private AudioClip castingClip;

    [SerializeField] private AudioClip healClip;
    [SerializeField] private AudioClip earthSpellClip;

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
        musicAudioSource.clip = musicClips[0];
        musicAudioSource.Play();
    }

    public void PlayHitSFX()
    {
        sfxAudioSource.PlayOneShot(hitClip);
    }

    public void PlayCriticalSFX()
    {
        sfxAudioSource.PlayOneShot(criticalClip);
    }

    public void PlayBlockSFX()
    {
        sfxAudioSource.PlayOneShot(blockClip);
    }

    public void PlayMissSFX()
    {
        sfxAudioSource.PlayOneShot(missClip);
    }

    public void PlayHealSFX()
    {
        sfxAudioSource.PlayOneShot(healClip);
    }

    public void PlayCastingSFX()
    {
        sfxAudioSource.PlayOneShot(castingClip);
    }

    public void PlayEarthSpellSFX()
    {
        sfxAudioSource.PlayOneShot(earthSpellClip);
    }
}