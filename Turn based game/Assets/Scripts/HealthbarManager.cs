using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarManager : MonoBehaviour
{
    [SerializeField] private Slider healthbarSlider;
    [SerializeField] private TMP_Text healthText;
    private int maxHealth;

    public void UpdateHealth(int currentHealth)
    {
        healthbarSlider.value = currentHealth;
        healthText.text = $"{currentHealth}/{maxHealth}";
    }

    public void SetHealth(int value)
    {
        maxHealth = value;
        healthbarSlider.maxValue = maxHealth;
        healthbarSlider.value = value;
        healthText.text = $"{value}/{value}";
    }
}