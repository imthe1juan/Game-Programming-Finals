using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoundsManager : MonoBehaviour
{
    private DialogueManager dialogueManager;
    [SerializeField] private GameObject roundOutputObject;
    [SerializeField] private TMP_Text roundText;
    private int round = 1;

    public int Round
    { get { return round; } }

    private void Awake()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    public void FirstRound()
    {
        roundOutputObject.SetActive(true);
        roundText.text = $"Round\n{round}/3";
        Invoke(nameof(StartBattle), 1);
    }

    public void NextRound()
    {
        round++;
        roundOutputObject.SetActive(true);
        roundText.text = $"Round\n{round}/3";
        Invoke(nameof(StartBattle), 2);
    }

    public void LastRound()
    {
        dialogueManager.InitalizeEndDialogue(true);
    }

    public void StartBattle()
    {
        roundOutputObject.SetActive(false);
        BattleManager.Instance.StartBattle();
    }
}