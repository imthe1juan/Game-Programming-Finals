using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManabarManager : MonoBehaviour
{
    [SerializeField] private Slider manaSlider;
    [SerializeField] private TMP_Text manaText;
    private int maxMana;

    public void UpdateMana(int currentHealth)
    {
        manaSlider.value = currentHealth;
        manaText.text = $"{currentHealth}/{maxMana}";
    }

    public void SetMana(int value)
    {
        maxMana = value;
        manaSlider.maxValue = maxMana;
        manaSlider.value = value;
        manaText.text = $"{value}/{value}";
    }
}