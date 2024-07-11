using BayatGames.SaveGameFree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetMusicVolume : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        if (!SaveGame.Exists("MusicVol"))
        {
            SaveGame.Save<float>("MusicVol", .5f);
        }
    }

    private void Start()
    {
        slider.value = SaveGame.Load<float>("MusicVol");
        mixer.SetFloat("MusicVol", Mathf.Log10(slider.value) * 20);
    }

    private void OnEnable()
    {
        slider.value = SaveGame.Load<float>("MusicVol");
    }

    public void SetLevel(float sliderValue)
    {
        mixer.SetFloat("MusicVol", Mathf.Log10(sliderValue) * 20);
        SaveGame.Save<float>("MusicVol", sliderValue);
    }
}