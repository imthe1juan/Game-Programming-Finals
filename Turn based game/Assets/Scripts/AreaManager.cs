using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class AreaManager : MonoBehaviour
{
    private DialogueManager dialogueManager;
    private RoundsManager roundsManager;
    public static AreaManager Instance;

    [SerializeField] private Image backgroundImage;
    [SerializeField] private Sprite[] backgrounds;

    private List<CharacterSO> availableEnemies = new List<CharacterSO>();
    private List<CharacterSO> availableEnemiesClone = new List<CharacterSO>();
    [SerializeField] private CharacterSO[] bossEnemies;
    [SerializeField] private CharacterSO[] forestEnemies;
    [SerializeField] private CharacterSO[] waterEnemies;
    [SerializeField] private CharacterSO[] earthEnemies;
    [SerializeField] private CharacterSO[] fireEnemies;
    [SerializeField] private CharacterSO[] finalEnemies;

    [SerializeField] private int currentArea;
    public int AccessedMoves { get; private set; }

    public int CurrentArea
    {
        get
        {
            return currentArea;
        }
        private set
        {
            currentArea = value;
        }
    }

    private void Awake()
    {
        Instance = this;
        dialogueManager = FindObjectOfType<DialogueManager>();
        roundsManager = FindObjectOfType<RoundsManager>();
    }

    private void Start()
    {
        dialogueManager.InitializeFirstRound();
    }

    public void SetArea()
    {
        //Set Enemies
        availableEnemies.Clear();
        AudioManager.Instance.PlayBattleMusic(currentArea);
        backgroundImage.sprite = backgrounds[currentArea];
        switch (currentArea)
        {
            case 0:
                AccessedMoves = 2;
                availableEnemies = new List<CharacterSO>(forestEnemies);
                dialogueManager.CurrentConversation = 0;
                break;

            case 1:
                AccessedMoves = 3;
                availableEnemies = new List<CharacterSO>(waterEnemies);
                if (roundsManager.Round == 3)
                {
                    availableEnemies.Add(bossEnemies[1]);
                }
                dialogueManager.CurrentConversation = 2;
                break;

            case 2:
                AccessedMoves = 3;
                availableEnemies = new List<CharacterSO>(earthEnemies);
                if (roundsManager.Round == 3)
                {
                    availableEnemies.Add(bossEnemies[2]);
                }
                dialogueManager.CurrentConversation = 4;
                break;

            case 3:
                AccessedMoves = 3;
                availableEnemies = new List<CharacterSO>(fireEnemies);
                if (roundsManager.Round == 3)
                {
                    availableEnemies.Add(bossEnemies[3]);
                }
                dialogueManager.CurrentConversation = 6;
                Debug.Log("Should Test");
                break;

            case 4:
                AccessedMoves = 3;
                availableEnemies = new List<CharacterSO>(finalEnemies);
                if (roundsManager.Round == 3)
                {
                    availableEnemies.Add(bossEnemies[4]);
                }
                dialogueManager.CurrentConversation = 8;
                break;

            default:
                AccessedMoves = 1;
                availableEnemies = new List<CharacterSO>(forestEnemies);
                if (roundsManager.Round == 3)
                {
                    availableEnemies.Add(bossEnemies[0]);
                }
                dialogueManager.CurrentConversation = 0;
                break;
        }
    }

    public void SetEnemy()
    {
        if (roundsManager.Round == 3)
        {
            availableEnemies.Add(bossEnemies[currentArea]);
        }
        foreach (var item in availableEnemies)
        {
            availableEnemiesClone.Add(item);
        }

        foreach (var item in BattleManager.Instance.ReturnEnemies())
        {
            if (availableEnemiesClone.Count > 0)
            {
                int randomIndex = Random.Range(0, availableEnemiesClone.Count);
                item.characterSO = availableEnemiesClone[randomIndex];
                item.SetCharacter();
                availableEnemiesClone.RemoveAt(randomIndex); // Remove the assigned enemy
            }
        }
    }

    public void NextArea()
    {
        if (currentArea < 4)
        {
            currentArea++;
            GameStateManager.Instance.DisableGameStateObject();
            dialogueManager.InitializeFirstRound();
        }
        else
        {
            AudioManager.Instance.PlayEndMusic();
            SceneManager.LoadScene("EndScene");
        }
    }
}