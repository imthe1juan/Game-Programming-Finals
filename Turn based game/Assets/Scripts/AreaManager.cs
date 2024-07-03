using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private int currentArea;

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
        foreach (var item in BattleManager.Instance.ReturnEnemies())
        {
            switch (currentArea)
            {
                case 0:
                    item.characterSO = forestEnemies[Random.Range(0, forestEnemies.Length)];
                    break;

                case 1:
                    item.characterSO = forestEnemies[Random.Range(0, waterEnemies.Length)];
                    break;

                case 2:
                    item.characterSO = forestEnemies[Random.Range(0, earthEnemies.Length)];
                    break;

                case 3:
                    item.characterSO = forestEnemies[Random.Range(0, fireEnemies.Length)];
                    break;

                default:
                    item.characterSO = forestEnemies[Random.Range(0, forestEnemies.Length)];
                    break;
            }
        }
        backgroundImage.sprite = backgrounds[currentArea];
    }

    public void NextArea()
    {
        currentArea++;
        GameStateManager.Instance.DisableGameStateObject();
        dialogueManager.InitializeFirstRound();
    }
}