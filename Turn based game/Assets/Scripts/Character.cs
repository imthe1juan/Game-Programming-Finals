using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public HealthbarManager healthbarManager;
    public Transform target;
    public string characterName;
    public float speed;
    public float currentSpeed;

    public int maxHealth;
    public int currentHealth;
    public bool isEnemy;

    public List<Move> moves;
    public Move preselectedMove;
    public Collider2D c2D;
    public SpriteRenderer sr;
    public bool tookAction = false;
    public Vector3 originalPos;

    public virtual void Awake()
    {
        originalPos = transform.position;
        currentSpeed = speed;
        currentHealth = maxHealth;
        healthbarManager.SetHealth(currentHealth);
    }

    private void Update()
    {
    }

    public virtual void ThisTurn()
    {
        BattleManager.Instance.characterTurn = this;
        currentSpeed = speed;
        BattleManager.Instance.isActionActive = true;
        tookAction = true;
    }

    private void PreselectMove(Move move)
    {
        BattleManager.Instance.PreselectedMove();
        preselectedMove = move;
        if (move.isTargetAlly)
        {
            BattleManager.Instance.SetTargets(1);
        }
        else
        {
            BattleManager.Instance.SetTargets(0);
        }
    }

    public virtual void ExecuteMove()
    {
        Invoke(nameof(RevertPosition), .5f);
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
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
        healthbarManager.UpdateHealth(currentHealth);

        Invoke(nameof(NextTurn), .5f);
    }

    public void Heal(int heal)
    {
        currentHealth += heal;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        healthbarManager.UpdateHealth(currentHealth);
        Invoke(nameof(NextTurn), .5f);
    }

    private void NextTurn()
    {
        BattleManager.Instance.isActionActive = false;
        BattleManager.Instance.NextTurn();
    }

    public virtual void OnMouseDown()
    {
    }

    public void SetMoveset()
    {
        for (int i = 0; i < moves.Count; i++)
        {
            int index = i;
            BattleManager.Instance.movesetButtonList[index].onClick.AddListener(() => PreselectMove(moves[index]));
            BattleManager.Instance.movesetButtonList[index].GetComponentInChildren<TMP_Text>().text = moves[i].moveName;
        }
        for (int i = BattleManager.Instance.movesetButtonList.Count - 1; i > moves.Count - 1; i--)
        {
            BattleManager.Instance.movesetButtonList[i].gameObject.SetActive(false);
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