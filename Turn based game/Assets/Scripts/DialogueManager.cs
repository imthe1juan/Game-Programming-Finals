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

    [SerializeField] private int currentConversation = 0;
    [SerializeField] private int currentDialogue = 0;

    public int CurrentConversation
    { get { return currentConversation; } set { currentConversation = value; } }

    private void Awake()
    {
        roundsManager = FindObjectOfType<RoundsManager>();
    }

    public void InitializeFirstRound()
    {
        roundsManager.ResetRound();
        AreaManager.Instance.SetArea();
        AreaManager.Instance.SetEnemy();
        InitalizeDialogue(true);
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
            characterImage[1].SetNativeSize();
        }
        else
        {
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
            characterImage[1].SetNativeSize();
        }
        else
        {
            AreaManager.Instance.NextArea();
            //NextMap
        }
    }

    public virtual void NextDialogue()
    {
        currentDialogue++;
        if (currentDialogue > conversations[currentConversation].dialogue.Length - 1)
        {
            currentDialogue = 0;
            currentConversation++;

            if (roundsManager.Round == 1)
            {
                dialogueObject.SetActive(false);
                roundsManager.FirstRound();
            }
            else if (roundsManager.Round == 3)// Last Round
            {
                dialogueObject.SetActive(false);
                GameStateManager.Instance.IsPlayerWon(true);
            }
            return;
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
            characterImage[0].sprite = conversations[currentConversation].characterSprite[currentSpeaker];
            characterImage[0].color = new Color32(255, 255, 255, 255);
            characterImage[0].transform.localScale = new Vector3(1.7f, 1.7f, 1.7f);

            characterImage[1].color = new Color32(150, 150, 150, 255);
            characterImage[1].transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }
        else if (currentSpeaker == 1)
        {
            characterImage[1].color = new Color32(255, 255, 255, 255);
            characterImage[1].transform.localScale = new Vector3(1.7f, 1.7f, 1.7f);

            characterImage[0].color = new Color32(150, 150, 150, 255);
            characterImage[0].transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }
        else if (currentSpeaker == 2)
        {
            characterImage[0].sprite = conversations[currentConversation].characterSprite[currentSpeaker];
            characterImage[0].color = new Color32(255, 255, 255, 255);
            characterImage[0].transform.localScale = new Vector3(1.7f, 1.7f, 1.7f);

            characterImage[1].color = new Color32(150, 150, 150, 255);
            characterImage[1].transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }
    }
}