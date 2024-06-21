using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;
    [SerializeField] private List<Character> characters;
    [SerializeField] private List<Character> allies;
    [SerializeField] private List<Character> enemies;
    public List<Button> movesetButtonList;

    [SerializeField] private Transform pickTarget;
    [SerializeField] private TMP_Text playerName;
    [SerializeField] private TMP_Text characterAction;
    [SerializeField] private Transform movesetParent;

    public Character characterTurn;
    public bool isActionActive = false;
    public bool isAllyTurn;
    public bool preselectedMove = false;
    private int tookAction = 0;
    [SerializeField] private Character targetCharacter;

    private void Awake()
    {
        Instance = this;
        characters.AddRange(allies);
        characters.AddRange(enemies);
    }

    private void Start()
    {
        NextTurn();
    }

    public void EnemyTurn(string name)
    {
        characterAction.gameObject.SetActive(false);

        characterAction.text = name;
        playerName.gameObject.SetActive(false);
    }

    public void PlayerTurn(string name, string moveset)
    {
        DisableEnemyColliders();
        preselectedMove = false;
        isAllyTurn = true;
        targetCharacter = enemies[0];

        characterAction.gameObject.SetActive(false);

        movesetParent.gameObject.SetActive(true);
        playerName.gameObject.SetActive(true);
        playerName.text = name;

        characterAction.text = moveset;
    }

    public void DisableMoveset()
    {
        for (int i = 0; i < movesetButtonList.Count; i++)
        {
            movesetButtonList[i].onClick.RemoveAllListeners();
        }
        SetTargets(2);
        movesetParent.gameObject.SetActive(false);
        characterAction.gameObject.SetActive(true);
        pickTarget.gameObject.SetActive(false);
    }

    public void NextTurn()
    {
        if (tookAction >= characters.Count)
        {
            tookAction = 0;
            foreach (var item in characters)
            {
                item.tookAction = false;
            }
        }
        for (int i = 0; i < characters.Count; i++)
        {
            if (characters[i].tookAction == false)
            {
                tookAction++;
                characters[i].ThisTurn();
                break;
            }
        }
    }

    public void ShowCharacterAction()
    {
    }

    public void PickTarget(Character character)
    {
        if (!isAllyTurn) return;

        targetCharacter = character;
        print("Set target is " + targetCharacter.characterName);
        characterTurn.ExecuteMove();
    }

    public Character GetTarget()
    {
        return targetCharacter;
    }

    public Character PickRandomAlly()
    {
        int randomNum = UnityEngine.Random.Range(0, allies.Count);
        return allies[randomNum];
    }

    public Character PickRandomEnemy()
    {
        int randomNum = UnityEngine.Random.Range(0, allies.Count);
        return enemies[randomNum];
    }

    public void PreselectedMove()
    {
        preselectedMove = true;
        pickTarget.gameObject.SetActive(true);
    }

    public void SetTargets(int number)
    {
        foreach (var item in allies)
        {
            item.EnableCharacter();
        }
        foreach (var item in enemies)
        {
            item.EnableCharacter();
        }
        if (number == 0)
        {
            foreach (var item in allies)
            {
                item.DisableCharacter();
            }
        }
        else if (number == 1)
        {
            foreach (var item in enemies)
            {
                item.DisableCharacter();
            }
        }
    }

    private void DisableEnemyColliders()
    {
        foreach (var item in enemies)
        {
            item.DisableColliders();
        }
    }

    //0 = Disable enemy collider, 1 = Disable ally collider, 2 = enable all
}