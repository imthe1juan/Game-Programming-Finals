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

    [SerializeField] private FadeController controller;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Sprite[] backgrounds;

    public List<CharacterSO> availableEnemies = new List<CharacterSO>();
    public List<CharacterSO> availableEnemiesClone = new List<CharacterSO>();

    [SerializeField] private Character[] finalAllies;
    [SerializeField] private CharacterSO[] bossEnemies;
    [SerializeField] private CharacterSO[] forestEnemies;
    [SerializeField] private CharacterSO[] waterEnemies;
    [SerializeField] private CharacterSO[] windEnemies;
    [SerializeField] private CharacterSO[] fireEnemies;
    [SerializeField] private CharacterSO[] finalEnemies;

    [SerializeField] private int currentArea;
    public int AccessedMoves { get; private set; }
    private bool alliesSet = false;

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
        availableEnemies.Clear();
        availableEnemiesClone.Clear();
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

                dialogueManager.CurrentConversation = 2;
                break;

            case 2:
                AccessedMoves = 3;
                availableEnemies = new List<CharacterSO>(windEnemies);

                dialogueManager.CurrentConversation = 4;
                break;

            case 3:
                AccessedMoves = 3;
                availableEnemies = new List<CharacterSO>(fireEnemies);

                dialogueManager.CurrentConversation = 6;
                break;

            case 4:
                AccessedMoves = 3;
                availableEnemies = new List<CharacterSO>(finalEnemies);

                dialogueManager.CurrentConversation = 8;
                break;

            default:
                AccessedMoves = 1;
                availableEnemies = new List<CharacterSO>(forestEnemies);
                dialogueManager.CurrentConversation = 0;
                break;
        }
    }

    public void SetEnemy()
    {
        if (roundsManager.Round == 3)
        {
            Character boss = BattleManager.Instance.ReturnEnemies()[2];
            boss.characterSO = bossEnemies[currentArea];
            boss.gameObject.SetActive(true);
            boss.SetCharacter();
        }

        if (alliesSet) return;

        alliesSet = true;

        if (currentArea >= 1)
        {
            if (!finalAllies[0].isActiveAndEnabled)
            {
                finalAllies[0].gameObject.SetActive(true);
                finalAllies[0].SetCharacter();
            }
        }
        if (currentArea >= 4)
        {
            if (!finalAllies[1].isActiveAndEnabled)
            {
                finalAllies[1].gameObject.SetActive(true);
                finalAllies[1].SetCharacter();
            }
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
                item.gameObject.SetActive(true);
                item.SetCharacter();
                availableEnemiesClone.RemoveAt(randomIndex); // Remove the assigned enemy
            }
        }
    }

    public void NextArea()
    {
        alliesSet = false;
        currentArea++;
        GameStateManager.Instance.DisableGameStateObject();
        dialogueManager.InitializeFirstRound();
    }

    public void PandoraDefeated()
    {
        backgroundImage.sprite = backgrounds[currentArea + 1];
    }
}