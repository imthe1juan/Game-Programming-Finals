using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private FadeController controller;
    [SerializeField] private Animator anim;
    private RoundsManager roundsManager;

    [SerializeField] private GameObject dialogueObject;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private ConversationSO[] conversations;
    [SerializeField] private Image[] characterImage;

    [SerializeField] private TMP_Text[] characterNameText;
    [SerializeField] private TMP_Text dialogueText;

    [SerializeField] private int currentConversation = 0;
    [SerializeField] private int currentDialogue = 0;

    public int CurrentConversation
    { get { return currentConversation; } set { currentConversation = value; } }

    private int testing = 0;

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

            //FOR TESTING!!!
            if (testing >= 1)// Last Round
            {
                dialogueObject.SetActive(false);
                GameStateManager.Instance.IsPlayerWon(true);
                testing = 0;
            }
            else
            {
                dialogueObject.SetActive(false);
                roundsManager.FirstRound();
                testing++;
            }

            // ORIGINAL
            /*      if (roundsManager.Round == 1)
                  {
                      dialogueObject.SetActive(false);
                      roundsManager.FirstRound();
                  }
                  else if (roundsManager.Round == 3)// Last Round
                  {
                      dialogueObject.SetActive(false);
                      GameStateManager.Instance.IsPlayerWon(true);
                  }*/
            return;
        }
        ShowDialogue();
    }

    public virtual void ShowDialogue()
    {
        ConversationSO currentDialogueSceneSO = conversations[currentConversation];
        dialogueText.text = currentDialogueSceneSO.dialogue[currentDialogue].dialogueString;
        int currentSpeaker = currentDialogueSceneSO.dialogue[currentDialogue].speaker;

        if (currentConversation == 6 && currentDialogue == 3) //Fading to introduce Veronna
        {
            nextButton.SetActive(false);
            controller.Blink();
            StartCoroutine(NextDialogueCoroutine());
        }
        if (currentConversation == 9 && currentDialogue == 7) //Fading after defeating Pandora
        {
            nextButton.SetActive(false);
            controller.Blink();
            StartCoroutine(NextDialogueCoroutine());
        }

        if (currentSpeaker % 2 == 0 || currentSpeaker == 0)
        {
            characterImage[0].sprite = conversations[currentConversation].characterSprite[currentSpeaker];
            characterNameText[0].text = conversations[currentConversation].characterName[currentSpeaker];
            characterImage[0].color = new Color32(255, 255, 255, 255);
            characterImage[0].transform.localScale = Vector3.one;
            characterImage[0].SetNativeSize();

            characterImage[1].color = new Color32(150, 150, 150, 255);

            if (currentConversation == 6 && currentDialogue <= 3)
            {
                characterImage[1].color = new Color32(0, 0, 0, 255);
            }

            characterImage[1].transform.localScale = new Vector3(.8f, .8f, .8f);
        }
        else
        {
            if (roundsManager.Round == 3 && testing >= 1 && currentConversation == 0)
            {
                //Boss Defeated
                anim.SetTrigger("Defeat");
            }

            characterImage[1].sprite = conversations[currentConversation].characterSprite[currentSpeaker];
            characterNameText[1].text = conversations[currentConversation].characterName[currentSpeaker];
            characterImage[1].color = new Color32(255, 255, 255, 255);
            characterImage[1].transform.localScale = Vector3.one;
            characterImage[1].SetNativeSize();

            characterImage[0].color = new Color32(150, 150, 150, 255);
            characterImage[0].transform.localScale = new Vector3(.8f, .8f, .8f);
        }
    }

    private IEnumerator NextDialogueCoroutine()
    {
        yield return new WaitForSeconds(2);
        if (currentConversation == 9)
        {
            AreaManager.Instance.PandoraDefeated();
            characterImage[1].gameObject.SetActive(false);
            AudioManager.Instance.PlayEndMusic();
        }
        NextDialogue();
        nextButton.SetActive(true);
    }
}