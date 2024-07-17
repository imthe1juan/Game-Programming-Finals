using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndManager : MonoBehaviour
{
    [SerializeField] private FadeController controller;

    private void Start()
    {
        controller.FadeFromBlack();
    }

    public void MenuScene()
    {
        SceneManager.LoadScene(0);
    }
}