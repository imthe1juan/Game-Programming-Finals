using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;
    [SerializeField] private GameObject gameStateObject;
    [SerializeField] private TMP_Text stateText;

    private void Awake()
    {
        Instance = this;
    }

    public void IsPlayerWon(bool won)
    {
        gameStateObject.SetActive(true);
        if (won)
        {
            stateText.text = "You Won!";
        }
        else
        {
            stateText.text = "You Lose!";
        }
    }

    public void DisableGameStateObject()
    {
        gameStateObject.SetActive(false);
    }
}