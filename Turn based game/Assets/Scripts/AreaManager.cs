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
    private int accessedMoves;
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
    }

    private void Start()
    {
        dialogueManager.InitializeFirstRound();
    }

    public void SetArea()
    {
        //Set Enemies
        foreach (var item in BattleManager.Instance.ReturnEnemies())
        {
            switch (currentArea)
            {
                case 0:
                    item.characterSO = forestEnemies[Random.Range(0, forestEnemies.Length)];
                    item.SetCharacter();
                    break;

                case 1:
                    item.characterSO = waterEnemies[Random.Range(0, waterEnemies.Length)];
                    item.SetCharacter();
                    break;

                case 2:
                    item.characterSO = earthEnemies[Random.Range(0, earthEnemies.Length)];
                    item.SetCharacter();
                    break;

                case 3:
                    item.characterSO = fireEnemies[Random.Range(0, fireEnemies.Length)];
                    item.SetCharacter();
                    break;

                case 4:
                    item.characterSO = finalEnemies[Random.Range(0, finalEnemies.Length)];
                    item.SetCharacter();
                    break;

                default:
                    item.characterSO = forestEnemies[Random.Range(0, forestEnemies.Length)];
                    item.SetCharacter();
                    break;
            }
        }

        switch (currentArea)
        {
            case 0:
                AccessedMoves = 1;
                break;

            case 1:
                AccessedMoves = 2;

                break;

            case 4:
                AccessedMoves = 3;

                break;

            default:

                break;
        }
    }

    public void NextArea()
    {
        currentArea++;
        GameStateManager.Instance.DisableGameStateObject();
        dialogueManager.InitializeFirstRound();
    }
}