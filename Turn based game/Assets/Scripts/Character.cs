using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public Transform target;
    public string characterName;
    public float speed;
    public float currentSpeed;

    public int health;
    public int currentHealth;
    public bool isEnemy;

    public List<Move> moves;
    private Move preselectedMove;
    public Collider2D c2D;
    public SpriteRenderer sr;

    public virtual void Awake()
    {
        currentSpeed = speed;
    }

    private void Update()
    {
        //If someone is attacking, stop the cooldown
        if (!BattleManager.Instance.isActionActive)
        {
            currentSpeed -= Time.deltaTime;
        }
        if (currentSpeed < 0)
        {
            ThisTurn();
        }
    }

    public virtual void ThisTurn()
    {
        BattleManager.Instance.characterTurn = this;
        currentSpeed = speed;
        BattleManager.Instance.isActionActive = true;
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

    public void ExecutePlayerMove()
    {
        Character target = BattleManager.Instance.GetTarget();
        preselectedMove.Execute(this, target);
        BattleManager.Instance.DisableMoveset();
    }

    public virtual void ExecuteMove(Move move)
    {
        Character target = BattleManager.Instance.GetTarget();
        move.Execute(this, target);
        BattleManager.Instance.DisableMoveset();
    }

    public void Damage(int damage)
    {
        Debug.Log(characterName + " received " + damage);
        Invoke(nameof(NextTurn), 1);
    }

    public void Heal(int heal)
    {
        Debug.Log(characterName + " received " + heal + " health");
        Invoke(nameof(NextTurn), 1);
    }

    private void NextTurn()
    {
        BattleManager.Instance.isActionActive = false;
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
        }
        for (int i = BattleManager.Instance.movesetButtonList.Count - 1; i > moves.Count - 1; i--)
        {
            BattleManager.Instance.movesetButtonList[i].gameObject.SetActive(false);
        }
    }

    public virtual void EnableCharacter()
    {
        sr.color = new Color32(155, 89, 182, 255);
        //sr.color = new Color32(255, 255, 255, 255); Original
        c2D.enabled = true;
    }

    public virtual void DisableCharacter()
    {
        sr.color = new Color32(231, 76, 60, 100);
        //sr.color = new Color32(255, 255, 255, 100); Original
        c2D.enabled = false;
    }
}