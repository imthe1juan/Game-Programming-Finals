using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseCanvas;
    private bool paused;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!paused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }

    public void PauseGame()
    {
        paused = true;
        pauseCanvas.SetActive(true);
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        paused = false;
        pauseCanvas.SetActive(false);
        Time.timeScale = 1;
    }

    public void MenuGame()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
}