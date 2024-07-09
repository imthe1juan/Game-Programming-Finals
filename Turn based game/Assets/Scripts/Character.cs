using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Character : MonoBehaviour
{
    public CharacterSO characterSO;
    protected BattleManager battleManager;
    private HealthbarManager healthbarManager;
    private ManabarManager manabarManager;

    public Character target;
    public string characterName;
    public bool isMC;
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

    private float transitionProgress = 0f;
    private Color32 targetColor = new Color32(255, 0, 0, 0);
    private Color32 initialColor = new Color32(255, 255, 255, 255);

    public virtual void Awake()
    {
        battleManager = FindObjectOfType<BattleManager>();
        healthbarManager = GetComponent<HealthbarManager>();
        manabarManager = GetComponent<ManabarManager>();
        SetCharacter();
    }

    private void Update()
    {
        if (!dead) return;

        transitionProgress += Time.deltaTime / 1;
        sr.color = Color32.Lerp(initialColor, targetColor, transitionProgress);

        if (transitionProgress >= 1f)
        {
            gameObject.SetActive(false);
        }
    }

    public void SetCharacter()
    {
        gameObject.SetActive(true);
        dead = false;
        originalPos = transform.position;
        maxMana = characterSO.maxMana;
        maxHealth = characterSO.maxHealth;

        characterName = characterSO.characterName;
        sr.sprite = characterSO.characterDefaultSprite;
        sr.color = initialColor;
        transitionProgress = 0;
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
            currentHealth = 0;
        }
        healthbarManager.UpdateHealth(currentHealth);
    }

    public void CheckIfDead()
    {
        if (currentHealth <= 0)
        {
            dead = true;
            battleManager.CheckGameState();
        }
    }

    public virtual void Heal(int heal)
    {
        currentHealth += heal;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        healthbarManager.UpdateHealth(currentHealth);
        AudioManager.Instance.PlayHealSFX();
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
        int accessedMoves = moves.Count;
        if (isMC)
        {
            accessedMoves = AreaManager.Instance.AccessedMoves;
            Debug.Log(accessedMoves);
        }

        for (int i = 0; i < accessedMoves; i++)
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
        for (int i = 0; i < battleManager.movesetButtonList.Count; i++)
        {
            battleManager.movesetButtonList[i].gameObject.SetActive(true);
        }
        for (int i = battleManager.movesetButtonList.Count - 1; i > accessedMoves - 1; i--)
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

    private Coroutine moveCorutine;

    public void AttackSprite()
    {
        sr.sprite = characterSO.characterAttackSprite;

        if (moveCorutine != null)
        {
            StopCoroutine(moveCorutine);
        }

        moveCorutine = StartCoroutine(ResetSpriteAfterCooldown(0.5f));
    }

    private IEnumerator ResetSpriteAfterCooldown(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        RevertSprite();
    }

    public void DefendSprite()
    {
        sr.sprite = characterSO.characterDefendSprite;

        if (moveCorutine != null)
        {
            StopCoroutine(moveCorutine);
        }

        moveCorutine = StartCoroutine(ResetSpriteAfterCooldown(0.5f));
    }

    private void RevertSprite()
    {
        sr.sprite = characterSO.characterDefaultSprite;
    }
}