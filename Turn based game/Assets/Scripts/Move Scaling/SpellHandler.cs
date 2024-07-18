using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpellHandler : MonoBehaviour
{
    [SerializeField] private MoveScaling moveScaling;
    [SerializeField] private TMP_Text totalDamageText;
    [SerializeField] private SpellCircle[] spellCircles;

    [SerializeField, Range(0, 1)] private float offset = 0.5f; // Offset value to keep circles away from edges (0 = no offset, 1 = max offset)
    private int totalDamage = 0;
    private int enabledCircle = 0;
    private float multiplier = 0;
    private int multiplierAdded = 0;
    private int power;

    private List<Vector2> usedPositions = new List<Vector2>(); // Store used positions
    public float minDistance = 1.0f; // Minimum distance between circles

    public int moveRepeat; // Maximum of 5

    private void OnEnable()
    {
        totalDamageText.text = $"Total Damage:\n0";
        StartCoroutine(EnableSpellCircles());
    }

    private IEnumerator EnableSpellCircles()
    {
        if (enabledCircle >= moveRepeat) yield break;
        yield return new WaitForSeconds(.5f);

        Vector2 randomPosition = GetRandomPositionWithOffset();
        spellCircles[enabledCircle].transform.position = randomPosition;
        spellCircles[enabledCircle].gameObject.SetActive(true);
        usedPositions.Add(randomPosition); // Add the new position to the list
        enabledCircle++;
        if (enabledCircle < spellCircles.Length) StartCoroutine(EnableSpellCircles());
    }

    public void AddMultiplier(float value)
    {
        multiplierAdded++;
        multiplier += value;
        totalDamage = Mathf.RoundToInt(power * multiplier);
        totalDamageText.text = $"Total Damage:\n{totalDamage}";
        AudioManager.Instance.PlayCastingSFX();
        if (multiplierAdded >= moveRepeat)
        {
            moveScaling.Spell(totalDamage);

            enabledCircle = 0;
            multiplier = 0;
            multiplierAdded = 0;
            usedPositions.Clear();

            Invoke(nameof(DisableThis), .5f);
        }
    }

    private void DisableThis()
    {
        gameObject.SetActive(false);
    }

    private Vector2 GetRandomPositionWithOffset()
    {
        // Get screen bounds
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Calculate the actual offset value based on the screen dimensions and the offset range
        float actualOffsetX = offset * screenWidth / 2;
        float actualOffsetY = offset * screenHeight / 2;

        Vector2 worldPosition;
        bool positionIsValid;

        do
        {
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
            worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

            // Check if the position is valid (not too close to existing positions)
            positionIsValid = true;
            foreach (Vector2 usedPosition in usedPositions)
            {
                if (Vector2.Distance(worldPosition, usedPosition) < minDistance)
                {
                    positionIsValid = false;
                    break;
                }
            }
        } while (!positionIsValid);

        return worldPosition;
    }

    public void SetPower(int power)
    {
        this.power = power;
    }
}