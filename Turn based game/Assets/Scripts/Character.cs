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
    public HealthbarManager healthbarManager;
    public Transform target;
    public string characterName;
    public bool isEnemy = false;

    public int maxHealth;
    public int currentHealth;
    public bool dead;
    public List<Move> moves;
    public Move preselectedMove;

    public Collider2D c2D;
    public SpriteRenderer sr;
    public bool tookAction = false;
    public Vector3 originalPos;

    public virtual void Awake()
    {
        battleManager = FindObjectOfType<BattleManager>();
        SetCharacter();
    }

    public void SetCharacter()
    {
        gameObject.SetActive(true);
        dead = false;
        originalPos = transform.position;
        maxHealth = characterSO.maxHealth;
        characterName = characterSO.characterName;
        sr.sprite = characterSO.characterSprite;
        moves = characterSO.moves;

        currentHealth = maxHealth;
        healthbarManager.SetHealth(currentHealth);
    }

    public virtual void ThisTurn()
    {
        battleManager.characterTurn = this;
        battleManager.isActionActive = true;
        tookAction = true;
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
            //battleManager.RemoveCharacter(this);
            battleManager.CheckGameState();
        }
        healthbarManager.UpdateHealth(currentHealth);
    }

    public void Heal(int heal)
    {
        currentHealth += heal;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        healthbarManager.UpdateHealth(currentHealth);
        if (!isEnemy) return;
        Invoke(nameof(NextTurn), .5f);
    }

    public void NextTurn()
    {
        RevertPosition();
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
        }
        for (int i = battleManager.movesetButtonList.Count - 1; i > moves.Count - 1; i--)
        {
            battleManager.movesetButtonList[i].gameObject.SetActive(false);
        }
    }

    public virtual void EnableCharacter()
    {
        sr.color = new Color32(255, 255, 255, 255);
        c2D.enabled = true;
    }

    public virtual void DisableCharacter()
    {
        sr.color = new Color32(255, 255, 255, 100);
        c2D.enabled = false;
    }

    public virtual void DisableColliders()
    {
        c2D.enabled = false;
    }
}