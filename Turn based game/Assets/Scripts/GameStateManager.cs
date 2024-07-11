using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;
    [SerializeField] private Animator gemBookAnim;

    [SerializeField] private GameObject gemBookObject;
    [SerializeField] private GameObject gameStateObject;
    [SerializeField] private TMP_Text stateText;
    private bool won;

    private void Awake()
    {
        Instance = this;
    }

    public void IsPlayerWon(bool won)
    {
        this.won = won;
        if (won)
        {
            if (AreaManager.Instance.CurrentArea == 4)
            {
                PlayerWon();
            }
            else
            {
                gemBookObject.SetActive(true);
                gemBookAnim.SetInteger("CurrentArea", AreaManager.Instance.CurrentArea);
            }
        }
        else
        {
            gameStateObject.SetActive(true);
            stateText.text = "You Lose!";
        }
    }

    public void DisableGameStateObject()
    {
        gameStateObject.SetActive(false);
    }

    public void PlayerWon()
    {
        gemBookObject.SetActive(false);
        gameStateObject.SetActive(true);
        stateText.text = "You Won!";
    }

    public void NextButton()
    {
        if (won)
        {
            AreaManager.Instance.NextArea();
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }
}