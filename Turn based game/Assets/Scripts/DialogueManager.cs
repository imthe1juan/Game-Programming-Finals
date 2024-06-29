using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private RoundsManager roundsManager;
    [SerializeField] private GameObject dialogueObject;
    [SerializeField] private ConversationSO[] conversations;
    [SerializeField] private Image[] characterImage;

    [SerializeField] private TMP_Text[] characterNameText;
    [SerializeField] private TMP_Text dialogueText;

    private int currentConversation = 0;
    private int currentDialogue = 0;

    private void Awake()
    {
        roundsManager = FindObjectOfType<RoundsManager>();
    }

    private void Start()
    {
        if (roundsManager.Round == 1)
        {
            InitalizeDialogue(true);
        }
    }

    public virtual void InitalizeDialogue(bool hasDialogue)
    {
        if (hasDialogue)
        {
            dialogueObject.SetActive(true);
            ShowDialogue();
            characterImage[0].sprite = conversations[currentConversation].characterSprite[0];
            characterImage[1].sprite = conversations[currentConversation].characterSprite[1];
            characterNameText[0].text = conversations[currentConversation].characterName[0];
            characterNameText[1].text = conversations[currentConversation].characterName[1];
        }
        else
        {
            BattleManager.Instance.InitializeEnemies();
            roundsManager.FirstRound();
        }
    }

    public void InitalizeEndDialogue(bool hasDialogue)
    {
        if (hasDialogue)
        {
            dialogueObject.SetActive(true);
            ShowDialogue();
            characterImage[0].sprite = conversations[currentConversation].characterSprite[0];
            characterImage[1].sprite = conversations[currentConversation].characterSprite[1];
            characterNameText[0].text = conversations[currentConversation].characterName[0];
            characterNameText[1].text = conversations[currentConversation].characterName[1];
        }
        else
        {
            //NextMap
        }
    }

    public virtual void NextDialogue()
    {
        currentDialogue++;
        if (currentDialogue > conversations[currentConversation].dialogue.Length - 1)
        {
            currentDialogue = 0;
            if (roundsManager.Round == 1)
            {
                dialogueObject.SetActive(false);
                roundsManager.FirstRound();
            }
            else
            {
                dialogueObject.SetActive(false);
                GameStateManager.Instance.IsPlayerWon(true);
            }
        }
        ShowDialogue();
    }

    public virtual void ShowDialogue()
    {
        ConversationSO currentDialogueSceneSO = conversations[currentConversation];
        dialogueText.text = currentDialogueSceneSO.dialogue[currentDialogue].dialogueString;
        int currentSpeaker = currentDialogueSceneSO.dialogue[currentDialogue].speaker;
        if (currentSpeaker == 0)
        {
            characterImage[0].color = new Color32(255, 255, 255, 255);
            characterImage[0].transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
            characterImage[1].color = new Color32(150, 150, 150, 255);
            characterImage[1].transform.localScale = Vector3.one;
        }
        else
        {
            characterImage[1].color = new Color32(255, 255, 255, 255);
            characterImage[1].transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
            characterImage[0].color = new Color32(150, 150, 150, 255);
            characterImage[0].transform.localScale = Vector3.one;
        }
    }
}