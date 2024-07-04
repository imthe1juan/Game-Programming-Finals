using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using static UnityEditor.Progress;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;

    private RoundsManager roundsManager;

    [SerializeField] private List<Character> characters;
    [SerializeField] private List<Character> allies;
    [SerializeField] private List<Character> enemies;
    public List<Button> movesetButtonList;

    [SerializeField] private Image background;
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

    public void NextBattle()
    {
        roundOver = false;
        characters.Clear();
        characters.AddRange(allies);
        characters.AddRange(enemies);
        foreach (var item in enemies)
        {
            item.SetCharacter();
        }
        NextTurn();
    }

    public void EnemyTurn(string name)
    {
        if (roundOver) { return; }
        DisableMoveset();

        characterAction.text = name;
        playerName.gameObject.SetActive(false);
    }

    public void PlayerTurn(string name)
    {
        if (roundOver) { return; }
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

        movesetParent.gameObject.SetActive(false);
        characterAction.gameObject.SetActive(true);
        pickTarget.gameObject.SetActive(false);
    }

    public void NextTurn()
    {
        RevertFocus();
        CameraManager.Instance.DefaultCameraPos();

        if (roundOver) { return; }
        int aliveCharacters = 0;

        foreach (var item in characters)
        {
            if (!item.dead)
            {
                aliveCharacters++;
            }
        }

        if (tookAction >= aliveCharacters)
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

    public void InitializeFirstBattle()
    {
        roundOver = false;
        characters.Clear();
        characters.AddRange(allies);
        characters.AddRange(enemies);
    }

    public void PickTarget(Character character)
    {
        targetCharacter = character;
        print("Set target is " + targetCharacter.characterName);
        characterTurn.ExecuteMove();

        InitiateMove();
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

    //If player selects a skill, it determines the selectable targets
    public void SetTargets(int number)
    {
        //Makes everyone tagetable first (reset)
        foreach (var item in allies)
        {
            item.ColorCharacter();
            item.EnableCollider();
        }
        foreach (var item in enemies)
        {
            item.ColorCharacter();
            item.EnableCollider();
        }

        // 0 if enemies are targetable, this disables the allies
        if (number == 0)
        {
            foreach (var item in allies)
            {
                item.FadeCharacter();
                item.DisableCollider();
            }
        }// 1 if allies are targetable, disables enemies
        else if (number == 1)
        {
            foreach (var item in enemies)
            {
                item.FadeCharacter();
                item.DisableCollider();
            }
        }
    }

    public void FocusMove(Character user, Character target)
    {
        background.color = new Color32(150, 150, 150, 255);
        for (int i = 0; i < characters.Count; i++)
        {
            if (characters[i].characterName != user.characterName && characters[i].characterName != target.characterName)
            {
                characters[i].DisableSprite();
            }
        }
    }

    private void InitiateMove()
    {
        DisableCharacterColliders();
        ShowCharacters();
    }

    private void RevertFocus()
    {
        background.color = new Color32(255, 255, 255, 255);
        for (int i = 0; i < characters.Count; i++)
        {
            characters[i].EnableSprite();
        }
    }

    //Disable Colliders so player can't pick a target
    private void DisableCharacterColliders()
    {
        foreach (var item in characters)
        {
            item.DisableCollider();
        }
    }

    private void ShowCharacters()
    {
        foreach (var item in characters)
        {
            item.ColorCharacter();
        }
    }

    public List<Character> ReturnEnemies()
    {
        return enemies;
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