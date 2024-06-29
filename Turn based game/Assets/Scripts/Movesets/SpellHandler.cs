using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpellHandler : MonoBehaviour
{
    [SerializeField] private TMP_Text totalDamageText;
    [SerializeField] private MoveScaling moveScaling;
    [SerializeField] private SpellCircle[] spellCircles;
    [SerializeField, Range(0, 1)] private float offset = 0.5f; // Offset value to keep circles away from edges (0 = no offset, 1 = max offset)
    private int enabledCircle = 0;
    private float multiplier = 0;
    private int multiplierAdded = 0;
    private int power;

    private void OnEnable()
    {
        totalDamageText.text = $"Total Damage:\n{power}";
        StartCoroutine(EnableSpellCircles());
    }

    private IEnumerator EnableSpellCircles()
    {
        yield return new WaitForSeconds(.5f);

        Vector2 randomPosition = GetRandomPositionWithOffset();
        spellCircles[enabledCircle].transform.position = randomPosition;
        spellCircles[enabledCircle].gameObject.SetActive(true);
        enabledCircle++;
        if (enabledCircle < spellCircles.Length) StartCoroutine(EnableSpellCircles());
    }

    public void AddMultiplier(float value)
    {
        multiplierAdded++;
        multiplier += value;
        totalDamageText.text = $"Total Damage:\n{(int)(power * multiplier)}";
        if (multiplierAdded >= spellCircles.Length)
        {
            ScaleMove((int)multiplier);
            enabledCircle = 0;
            multiplier = 0;
            multiplierAdded = 0;

            gameObject.SetActive(false);
        }
    }

    private void ScaleMove(int scaler)
    {
        moveScaling.ScaleMove(scaler);
    }

    private Vector2 GetRandomPositionWithOffset()
    {
        // Get screen bounds
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Calculate the actual offset value based on the screen dimensions and the offset range
        float actualOffsetX = offset * screenWidth / 2;
        float actualOffsetY = offset * screenHeight / 2;

        // Generate random position within bounds and apply offset
        float xPosition = Random.Range(actualOffsetX, screenWidth - actualOffsetX);
        float yPosition = Random.Range(actualOffsetY, screenHeight - actualOffsetY);

        // If offset is at its maximum, position circles in the middle of the screen
        if (offset == 1)
        {
            xPosition = screenWidth / 2;
            yPosition = screenHeight / 2;
        }

        // Convert screen position to world position
        Vector2 screenPosition = new Vector2(xPosition, yPosition);
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        return worldPosition;
    }

    public void SetPower(int power)
    {
        this.power = power;
    }
}