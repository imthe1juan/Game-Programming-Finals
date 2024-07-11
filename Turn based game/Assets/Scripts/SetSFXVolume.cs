using BayatGames.SaveGameFree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetSFXVolume : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        if (!SaveGame.Exists("SFXVol"))
        {
            SaveGame.Save<float>("SFXVol", .5f);
        }
    }

    private void Start()
    {
        slider.value = SaveGame.Load<float>("SFXVol");
        mixer.SetFloat("SFXVol", Mathf.Log10(slider.value) * 20);
    }

    private void OnEnable()
    {
        slider.value = SaveGame.Load<float>("SFXVol");
    }

    public void SetLevel(float sliderValue)
    {
        mixer.SetFloat("SFXVol", Mathf.Log10(sliderValue) * 20);
        SaveGame.Save<float>("SFXVol", sliderValue);
    }
}