using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject options;
    [SerializeField] private GameObject credits;

    private void Start()
    {
        AudioManager.Instance.PlayMenuMusic();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
        AudioManager.Instance.PlayCutsceneMusic();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        mainMenu.SetActive(true);
        options.SetActive(false);
        credits.SetActive(false);
    }

    public void Options()
    {
        options.SetActive(true);
        mainMenu.SetActive(false);
        credits.SetActive(false);
    }

    public void Credits()
    {
        credits.SetActive(true);
        mainMenu.SetActive(false);
        options.SetActive(false);
    }
}