using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private GameObject cutsceneObject;
    [SerializeField] private Image blackOverlay;
    [SerializeField] private ConversationSO[] conversations;
    [SerializeField] private Image[] characterImage;

    [SerializeField] private TMP_Text[] characterNameText;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private TMP_Text dialogueText2;
    [SerializeField] private Button nextButton;
    private int currentConversation = 0;
    private int currentDialogue = 0;
    private float alpha = 255;
    private bool isFadeOverlay;
    private bool isEndCutscene;

    private void Start()
    {
        InitalizeDialogue();
    }

    private void Update()
    {
        if (!isFadeOverlay)
        {
            if (alpha < 255)
            {
                alpha += Time.deltaTime * 150;
                blackOverlay.color = new Color32(0, 0, 0, (byte)alpha);
            }
            else if (isEndCutscene && alpha >= 255)
            {
                LoadMainScene();
            }
        }
        else
        {
            if (alpha > 0)
            {
                alpha -= Time.deltaTime * 150;
                blackOverlay.color = new Color32(0, 0, 0, (byte)alpha);
            }
        }
    }

    public virtual void InitalizeDialogue()
    {
        cutsceneObject.SetActive(true);
        ShowDialogue();
        characterImage[0].sprite = conversations[currentConversation].characterSprite[0];
        characterImage[1].sprite = conversations[currentConversation].characterSprite[1];
        characterNameText[0].text = conversations[currentConversation].characterName[0];
        characterNameText[1].text = conversations[currentConversation].characterName[1];
    }

    public virtual void NextDialogue()
    {
        currentDialogue++;
        if (currentDialogue > conversations[currentConversation].dialogue.Length - 1)
        {
            isFadeOverlay = false;
            nextButton.gameObject.SetActive(false);
            isEndCutscene = true;
        }
        else
        {
            ShowDialogue();
        }
    }

    private void LoadMainScene()
    {
        SceneManager.LoadScene(2);
    }

    public virtual void ShowDialogue()
    {
        ConversationSO currentDialogueSceneSO = conversations[currentConversation];
        if (currentDialogue >= 2)
        {
            isFadeOverlay = true;
            dialogueText2.text = currentDialogueSceneSO.dialogue[currentDialogue].dialogueString;
            dialogueText.gameObject.SetActive(false);
        }
        else
        {
            dialogueText.text = currentDialogueSceneSO.dialogue[currentDialogue].dialogueString;
        }
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