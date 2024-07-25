using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public CharacterSO characterSO;
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
        healthbarManager = GetComponent<HealthbarManager>();
        manabarManager = GetComponent<ManabarManager>();
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
        tookAction = false;
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
        RegenMana(10); // Base mana
        tookAction = true;
        thisTurn = true;
    }

    private void PreselectMove(Move move)
    {
        BattleManager battleManager = BattleManager.Instance;
        battleManager.PreselectedMove(move);
        preselectedMove = move;

        if (move.isTargetSelf)
        {
            battleManager.PickTarget(this);
            return;
        }
        if (move.isTargetAlly)
        {
            battleManager.SetTargetable(1);
        }
        else
        {
            battleManager.SetTargetable(0);
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
            BattleManager.Instance.CheckGameState();
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

    public virtual void RegenMana(int manaAmount)
    {
        currentMana += manaAmount;
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
        BattleManager.Instance.NextTurn();
    }

    public virtual void OnMouseDown()
    {
        BattleManager.Instance.PickTarget(this);
    }

    public void SetMoveset()
    {
        int accessedMoves = moves.Count;
        MovesetManager movesetManager = MovesetManager.Instance;
        if (isMC)
        {
            accessedMoves = AreaManager.Instance.AccessedMoves;
        }

        for (int i = 0; i < accessedMoves; i++)
        {
            int index = i;

            movesetManager.movesetButtonList[index].onClick.AddListener(() => PreselectMove(moves[index]));
            movesetManager.movesetButtonList[index].transform.GetChild(0).GetComponent<Image>().sprite = moves[index].moveSprite; //Gets the move name TMP_Text
            if (moves[index].isTargetAlly)
            {
                movesetManager.movesetButtonList[index].transform.GetChild(1).GetComponentInChildren<TMP_Text>().text = $"{moves[index].moveName}\n{moves[index].moveDescription}" +
              $"\nMana Cost: {moves[index].manaCost}\nBase Recovery: {moves[index].power * moves[index].moveRepeat}"; //Gets the description TMP_Text
            }
            else
            {
                movesetManager.movesetButtonList[index].transform.GetChild(1).GetComponentInChildren<TMP_Text>().text = $"{moves[index].moveName}\n{moves[index].moveDescription}" +
              $"\nMana Cost: {moves[index].manaCost}\nBase Damage: {moves[index].power * moves[index].moveRepeat}"; //Gets the description TMP_Text
            }

            if (moves[index].manaCost > currentMana)
            {
                movesetManager.movesetButtonList[index].interactable = false;
            }
            else
            {
                movesetManager.movesetButtonList[index].interactable = true;
            }
        }

        for (int i = 0; i < movesetManager.movesetButtonList.Count; i++)
        {
            movesetManager.movesetButtonList[i].gameObject.SetActive(true);
        }
        for (int i = movesetManager.movesetButtonList.Count - 1; i > accessedMoves - 1; i--)
        {
            movesetManager.movesetButtonList[i].gameObject.SetActive(false);
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