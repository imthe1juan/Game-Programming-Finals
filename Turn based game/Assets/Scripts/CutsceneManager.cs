using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private FadeController controller;
    [SerializeField] private bool isBossBattle;

    [SerializeField] private GameObject bookObject;
    [SerializeField] private GameObject cutsceneObject;
    [SerializeField] private ConversationSO[] conversations;
    [SerializeField] private Image[] characterImage;

    [SerializeField] private TMP_Text[] characterNameText;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private TMP_Text dialogueText2;
    [SerializeField] private Button nextButton;

    private int currentConversation = 0;
    private int currentDialogue = 0;

    private void Start()
    {
        InitalizeDialogue();
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
            nextButton.gameObject.SetActive(false);
            StartCoroutine(LoadMainScene());
        }
        else
        {
            ShowDialogue();
        }
    }

    private IEnumerator LoadMainScene()
    {
        controller.FadeToBlack();
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(2);
    }

    public virtual void ShowDialogue()
    {
        ConversationSO currentDialogueSceneSO = conversations[currentConversation];
        if (currentDialogue >= 5)
        {
            controller.FadeFromBlack();
            dialogueText2.text = currentDialogueSceneSO.dialogue[currentDialogue].dialogueString;
            dialogueText.gameObject.SetActive(false);
        }
        else
        {
            dialogueText.text = currentDialogueSceneSO.dialogue[currentDialogue].dialogueString;
        }

        if (currentDialogue == 20)
        {
            bookObject.gameObject.SetActive(true);
        }
        else
        {
            bookObject.gameObject.SetActive(false);
        }
        int currentSpeaker = currentDialogueSceneSO.dialogue[currentDialogue].speaker;
        if (currentSpeaker % 2 == 0 || currentSpeaker == 0)
        {
            characterImage[0].sprite = conversations[currentConversation].characterSprite[currentSpeaker];
            characterNameText[0].text = conversations[currentConversation].characterName[currentSpeaker];
            characterImage[0].color = new Color32(255, 255, 255, 255);
            characterImage[0].transform.localScale = Vector3.one;

            characterImage[1].color = new Color32(150, 150, 150, 255);
            characterImage[1].transform.localScale = new Vector3(.8f, .8f, .8f);
        }
        else
        {
            characterImage[1].sprite = conversations[currentConversation].characterSprite[currentSpeaker];
            characterNameText[1].text = conversations[currentConversation].characterName[currentSpeaker];
            characterImage[1].color = new Color32(255, 255, 255, 255);
            characterImage[1].transform.localScale = Vector3.one;

            characterImage[0].color = new Color32(150, 150, 150, 255);
            characterImage[0].transform.localScale = new Vector3(.8f, .8f, .8f);
        }
    }
}