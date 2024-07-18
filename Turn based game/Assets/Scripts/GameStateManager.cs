using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;
    [SerializeField] private FadeController controller;
    [SerializeField] private Animator gemBookAnim;

    [SerializeField] private GameObject restartButton;
    [SerializeField] private GameObject nextButton;

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
                if (AreaManager.Instance.CurrentArea == 3)
                {
                    StartCoroutine(TransitionToPandora());
                }
            }
        }
        else
        {
            gameStateObject.SetActive(true);
            stateText.text = "You Lose!";
            nextButton.SetActive(true);
            restartButton.SetActive(true);
        }
    }

    public void DisableGameStateObject()
    {
        gameStateObject.SetActive(false);
    }

    public void PlayerWon()
    {
        if (AreaManager.Instance.CurrentArea == 4)
        {
            StartCoroutine(TransitionToEndScene());
            return;
        }

        nextButton.SetActive(true);
        gameStateObject.SetActive(true);
        gemBookObject.SetActive(false);

        string gemGot = "";
        switch (AreaManager.Instance.CurrentArea)
        {
            case 0:
                gemGot = "Earth Gem";
                break;

            case 1:
                gemGot = "Water Gem";
                break;

            case 2:
                gemGot = "Wind Gem";
                break;
        }
        stateText.text = "You got the " + gemGot + "!";
    }

    private IEnumerator TransitionToPandora()
    {
        yield return new WaitForSeconds(5);
        controller.FadeToBlack();
        yield return new WaitForSeconds(1);
        AreaManager.Instance.NextArea();
        gemBookObject.SetActive(false);
        yield return new WaitForSeconds(1);
        controller.FadeFromBlack();
    }

    public void NextButton()
    {
        nextButton.SetActive(false);

        if (won)
        {
            StartCoroutine(TransitionToNextArea());
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }

    public void RestartGame()
    {
        DisableGameStateObject();
        nextButton.SetActive(false);
        restartButton.SetActive(false);
        AreaManager.Instance.RestartArea();
    }

    private IEnumerator TransitionToNextArea()
    {
        controller.FadeToBlack();
        yield return new WaitForSeconds(1);
        AreaManager.Instance.NextArea();
        yield return new WaitForSeconds(1);
        controller.FadeFromBlack();
    }

    private IEnumerator TransitionToEndScene()
    {
        controller.FadeToBlack();
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("EndScene");
    }
}