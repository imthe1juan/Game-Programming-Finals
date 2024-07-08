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

    public int currentEnemies;
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
        backgroundImage.sprite = backgrounds[currentArea];
        List<CharacterSO> availableEnemies;
        switch (currentArea)
        {
            case 0:
                AccessedMoves = 1;
                availableEnemies = new List<CharacterSO>(forestEnemies);
                if (roundsManager.Round == 3)
                {
                    availableEnemies.Add(bossEnemies[0]);
                }
                currentEnemies = forestEnemies.Length;
                break;

            case 1:
                AccessedMoves = 2;
                availableEnemies = new List<CharacterSO>(waterEnemies);
                if (roundsManager.Round == 3)
                {
                    availableEnemies.Add(bossEnemies[1]);
                }
                break;

            case 2:
                AccessedMoves = 2;
                availableEnemies = new List<CharacterSO>(earthEnemies);
                if (roundsManager.Round == 3)
                {
                    availableEnemies.Add(bossEnemies[2]);
                }
                break;

            case 3:
                AccessedMoves = 2;
                availableEnemies = new List<CharacterSO>(fireEnemies);
                if (roundsManager.Round == 3)
                {
                    availableEnemies.Add(bossEnemies[3]);
                }
                break;

            case 4:
                AccessedMoves = 3;
                availableEnemies = new List<CharacterSO>(finalEnemies);
                if (roundsManager.Round == 3)
                {
                    availableEnemies.Add(bossEnemies[4]);
                }
                break;

            default:
                AccessedMoves = 1;
                availableEnemies = new List<CharacterSO>(forestEnemies);
                if (roundsManager.Round == 3)
                {
                    availableEnemies.Add(bossEnemies[0]);
                }
                break;
        }

        foreach (var item in BattleManager.Instance.ReturnEnemies())
        {
            if (availableEnemies.Count > 0)
            {
                int randomIndex = Random.Range(0, availableEnemies.Count);
                item.characterSO = availableEnemies[randomIndex];
                item.SetCharacter();
                availableEnemies.RemoveAt(randomIndex); // Remove the assigned enemy
            }
        }
    }

    public void NextArea()
    {
        currentArea++;
        GameStateManager.Instance.DisableGameStateObject();
        dialogueManager.InitializeFirstRound();
    }
}