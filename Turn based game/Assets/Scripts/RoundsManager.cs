using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoundsManager : MonoBehaviour
{
    private DialogueManager dialogueManager;
    [SerializeField] private GameObject roundObject;
    [SerializeField] private TMP_Text roundText;
    private int round = 1;

    public int Round
    { get { return round; } }

    private void Awake()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    private void Start()
    {
        roundObject.SetActive(true);
        roundText.text = $"Round\n{round}/3";
        Invoke(nameof(InitializeDialogue), 1);
    }

    public void NextRound()
    {
        round++;
        roundObject.SetActive(true);
        roundText.text = $"Round\n{round}/3";
        Invoke(nameof(InitializeDialogue), 2);
    }

    public void LastRound()
    {
        dialogueManager.InitalizeDialogue(true);
    }

    public void InitializeDialogue()
    {
        roundObject.SetActive(false);
        if (round == 1)
        {
            dialogueManager.InitalizeDialogue(false);
        }
        else
        {
            dialogueManager.InitalizeDialogue(false);
        }
    }
}