using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public CharacterSO characterSO;
    protected BattleManager battleManager;
    private HealthbarManager healthbarManager;
    private ManabarManager manabarManager;

    public Character target;
    public string characterName;
    public bool isEnemy = false;

    public int maxHealth;
    public int currentHealth;

    public int maxMana;
    public int currentMana;

    public bool dead;
    public List<Move> moves;
    public Move preselectedMove;

    public Collider2D c2D;
    public SpriteRenderer sr;
    public bool tookAction = false;
    public bool thisTurn = false;
    public Vector3 originalPos;

    public virtual void Awake()
    {
        battleManager = FindObjectOfType<BattleManager>();
        healthbarManager = GetComponent<HealthbarManager>();
        manabarManager = GetComponent<ManabarManager>();
        SetCharacter();
    }

    public void SetCharacter()
    {
        gameObject.SetActive(true);
        dead = false;
        originalPos = transform.position;
        maxMana = characterSO.maxMana;
        maxHealth = characterSO.maxHealth;

        characterName = characterSO.characterName;
        sr.sprite = characterSO.characterSprite;
        moves = characterSO.moves;

        currentHealth = maxHealth;
        currentMana = maxMana;

        healthbarManager.SetHealth(currentHealth);
        manabarManager.SetMana(currentMana);
    }

    public virtual void ThisTurn()
    {
        RegenMana();
        battleManager.characterTurn = this;
        battleManager.isActionActive = true;
        tookAction = true;
        thisTurn = true;
    }

    private void PreselectMove(Move move)
    {
        battleManager.PreselectedMove();
        preselectedMove = move;
        if (move.isTargetAlly)
        {
            battleManager.SetTargets(1);
        }
        else
        {
            battleManager.SetTargets(0);
        }
    }

    public virtual void ExecuteMove()
    {
    }

    public virtual void ExecuteMove(Move move)
    {
        /*Character target = BattleManager.Instance.GetTarget();
        move.Execute(this, target);
        BattleManager.Instance.DisableMoveset();
        Invoke(nameof(RevertPosition), .5f);*/
    }

    public void RevertPosition()
    {
        transform.position = originalPos;
    }

    public void Damage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            dead = true;
            currentHealth = 0;
            gameObject.SetActive(false);
            battleManager.CheckGameState();
        }
        healthbarManager.UpdateHealth(currentHealth);
    }

    public virtual void Heal(int heal)
    {
        currentHealth += heal;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        healthbarManager.UpdateHealth(currentHealth);
    }

    public virtual void UpdateMana(int value)
    {
        currentMana += value;
        if (currentMana > maxMana)
        {
            currentMana = maxMana;
        }
        manabarManager.UpdateMana(currentMana);
    }

    public virtual void RegenMana()
    {
        currentMana += 20;
        if (currentMana > maxMana)
        {
            currentMana = maxMana;
        }
        manabarManager.UpdateMana(currentMana);
    }

    public void NextTurn()
    {
        RevertPosition();
        thisTurn = false;
        battleManager.isActionActive = false;
        battleManager.NextTurn();
    }

    public virtual void OnMouseDown()
    {
    }

    public void SetMoveset()
    {
        for (int i = 0; i < moves.Count; i++)
        {
            int index = i;

            battleManager.movesetButtonList[index].onClick.AddListener(() => PreselectMove(moves[index]));
            battleManager.movesetButtonList[index].GetComponentInChildren<TMP_Text>().text = moves[i].moveName;
            if (moves[index].manaCost > currentMana)
            {
                battleManager.movesetButtonList[index].interactable = false;
            }
            else
            {
                battleManager.movesetButtonList[index].interactable = true;
            }
        }
        for (int i = battleManager.movesetButtonList.Count - 1; i > moves.Count - 1; i--)
        {
            battleManager.movesetButtonList[i].gameObject.SetActive(false);
        }
    }

    public void ColorCharacter()
    {
        sr.color = new Color32(255, 255, 255, 255);
    }

    public void FadeCharacter()
    {
        sr.color = new Color32(255, 255, 255, 100);
    }

    public void DisableCollider()
    {
        c2D.enabled = false;
    }

    public void EnableCollider()
    {
        c2D.enabled = true;
    }

    public void EnableSprite()
    {
        sr.gameObject.SetActive(true);
    }

    public void DisableSprite()
    {
        sr.gameObject.SetActive(false);
    }
}