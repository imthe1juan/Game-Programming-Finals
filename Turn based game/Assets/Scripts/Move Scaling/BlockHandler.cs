using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BlockHandler : MonoBehaviour
{
    [SerializeField] private MoveScaling moveScaling;
    [SerializeField] private TMP_Text totalDamageText;
    [SerializeField] private BlockCircle[] blockCircles;

    [SerializeField, Range(0, 1)] private float offset = 0.5f; // Offset value to keep circles away from edges (0 = no offset, 1 = max offset)
    private int totalDamage = 0;
    private int enabledCircle = 0;
    private float division = 0;
    private int divisionAdded = 0;
    private int power;

    private List<Vector2> usedPositions = new List<Vector2>(); // Store used positions
    public float minDistance = 1.0f; // Minimum distance between circles

    public int moveRepeat; // Maximum of 10

    private void OnEnable()
    {
        totalDamageText.text = $"Total Damage:\n{totalDamage}";
        StartCoroutine(EnableSpellCircles());
    }

    private IEnumerator EnableSpellCircles()
    {
        if (enabledCircle >= moveRepeat) yield break;
        yield return new WaitForSeconds(.5f);

        Vector2 randomPosition = GetRandomPositionWithOffset();
        blockCircles[enabledCircle].transform.position = randomPosition;
        blockCircles[enabledCircle].gameObject.SetActive(true);
        usedPositions.Add(randomPosition); // Add the new position to the list
        enabledCircle++;
        if (enabledCircle < blockCircles.Length) StartCoroutine(EnableSpellCircles());
    }

    public void DivideTotalDamage(float value)
    {
        divisionAdded++;
        division = value;
        if (division == 0)
        {
            totalDamage -= 0;
        }
        else
        {
            totalDamage -= Mathf.RoundToInt(power / division);
        }
        totalDamageText.text = $"Total Damage:\n{totalDamage}";
        AudioManager.Instance.PlayCastingSFX();
        if (divisionAdded >= moveRepeat)
        {
            moveScaling.Spell(totalDamage);

            enabledCircle = 0;
            division = 0;
            divisionAdded = 0;
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

    public void SetTotalDamage(int totalDamage)
    {
        this.totalDamage = totalDamage;
    }
}