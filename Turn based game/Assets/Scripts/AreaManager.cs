using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class AreaManager : MonoBehaviour
{
    private DialogueManager dialogueManager;
    public static AreaManager Instance;

    [SerializeField] private Image backgroundImage;
    [SerializeField] private Sprite[] backgrounds;

    [SerializeField] private CharacterSO[] forestEnemies;
    [SerializeField] private CharacterSO[] waterEnemies;
    [SerializeField] private CharacterSO[] earthEnemies;
    [SerializeField] private CharacterSO[] fireEnemies;
    [SerializeField] private CharacterSO[] finalEnemies;

    [SerializeField] private int currentArea;
    public int AccessedMoves { get; private set; }
    private bool setArea = false;

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
    }

    private void Start()
    {
        dialogueManager.InitializeFirstRound();
    }

    public void SetArea()
    {
        //Set Enemies
        List<CharacterSO> availableEnemies;
        backgroundImage.sprite = backgrounds[currentArea];

        switch (currentArea)
        {
            case 0:
                AccessedMoves = 1;
                availableEnemies = new List<CharacterSO>(forestEnemies);
                break;

            case 1:
                AccessedMoves = 2;
                availableEnemies = new List<CharacterSO>(waterEnemies);
                break;

            case 2:
                AccessedMoves = 2;
                availableEnemies = new List<CharacterSO>(earthEnemies);
                break;

            case 3:
                AccessedMoves = 2;
                availableEnemies = new List<CharacterSO>(fireEnemies);
                break;

            case 4:
                AccessedMoves = 3;
                availableEnemies = new List<CharacterSO>(finalEnemies);
                break;

            default:
                AccessedMoves = 1;
                availableEnemies = new List<CharacterSO>(forestEnemies);
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