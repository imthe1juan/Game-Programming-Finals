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

    private RoundsManager roundsManager;

    [SerializeField] private List<Character> characters;
    [SerializeField] private List<Character> allies;
    [SerializeField] private List<Character> enemies;
    public List<Button> movesetButtonList;

    [SerializeField] private Transform pickTarget;
    [SerializeField] private TMP_Text playerName;
    [SerializeField] private TMP_Text characterAction;
    [SerializeField] private Transform movesetParent;

    public Character characterTurn;
    [SerializeField] private Character targetCharacter;
    public bool isActionActive = false;
    public bool preselectedMove = false;
    private bool roundOver = false;
    [SerializeField] private int tookAction = 0;
    private int enemiesDead = 0;
    private int alliesDead = 0;

    private void Awake()
    {
        Instance = this;
        roundsManager = FindObjectOfType<RoundsManager>();
    }

    private void Start()
    {
        //NextTurn();
    }

    public void StartBattle()
    {
        characters.Clear();
        characters.AddRange(allies);
        characters.AddRange(enemies);
        NextTurn();
    }

    public void EnemyTurn(string name)
    {
        DisableColliders();
        if (roundOver) { return; }
        characterAction.gameObject.SetActive(false);

        characterAction.text = name;
        playerName.gameObject.SetActive(false);
    }

    public void PlayerTurn(string name)
    {
        if (roundOver) { return; }
        DisableColliders();
        preselectedMove = false;
        targetCharacter = enemies[0];

        characterAction.gameObject.SetActive(false);

        movesetParent.gameObject.SetActive(true);
        playerName.gameObject.SetActive(true);
        playerName.text = name;
    }

    public void AnnounceAction(string action)
    {
        characterAction.text = action;
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
        if (roundOver) { return; }
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
            if (characters[i].dead == false && characters[i].tookAction == false)
            {
                characters[i].ThisTurn();
                tookAction++;
                break;
            }
            else
            {
                continue;
            }
        }
    }

    public void InitializeEnemies()
    {
        roundOver = false;
        foreach (var item in enemies)
        {
            item.SetCharacter();
        }
    }

    public void PickTarget(Character character)
    {
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
        Character character = allies[randomNum];
        while (!character.gameObject.activeInHierarchy)
        {
            randomNum = UnityEngine.Random.Range(0, allies.Count);
            character = allies[randomNum];
        }
        return character;
    }

    public Character PickRandomEnemy()
    {
        int randomNum = UnityEngine.Random.Range(0, enemies.Count);
        Character character = enemies[randomNum];
        while (!character.gameObject.activeInHierarchy)
        {
            randomNum = UnityEngine.Random.Range(0, enemies.Count);
            character = enemies[randomNum];
        }
        return character;
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

    private void DisableColliders()
    {
        foreach (var item in characters)
        {
            item.DisableColliders();
        }
    }

    public void CheckGameState()
    {
        enemiesDead = 0;
        alliesDead = 0;

        foreach (var item in enemies)
        {
            if (item.dead)
            {
                enemiesDead++;
            }
        }
        if (enemiesDead >= enemies.Count)
        {
            //All enemies are dead
            tookAction = 0;
            roundOver = true;
            foreach (var item in characters)
            {
                item.tookAction = false;
            }

            if (roundsManager.Round == 3)
            {
                roundsManager.LastRound();
            }
            else
            {
                roundsManager.NextRound();
            }
        }

        foreach (var item in allies)
        {
            if (item.dead)
            {
                alliesDead++;
            }
        }
        if (alliesDead >= allies.Count)
        {
            roundOver = true;
            GameStateManager.Instance.IsPlayerWon(false);
        }
    }
}