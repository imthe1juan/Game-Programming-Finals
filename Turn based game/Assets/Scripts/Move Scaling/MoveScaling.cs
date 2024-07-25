using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoveScaling : MonoBehaviour
{
    private AudioManager audioManager;
    [SerializeField] private TMP_Text totalDamageText;
    [SerializeField] private SpellHandler spellHandler;
    [SerializeField] private BlockHandler blockHandler;
    [SerializeField] private TMP_Text popupText;
    [SerializeField] private Circle circle;

    private int totalDamage;
    private int repetition;
    private Character target;
    private Character user;
    private int power;
    private Vector3 initialPos;
    private bool isEnemy;
    private Move move;

    public GameObject vfx;

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void ScaleSpellMove(Move move, Character user, Character target, int power)
    {
        InitializeMove(move, user, target, power);
        if (isEnemy)
        {
            ConfigureBlockHandler();
        }
        else
        {
            ConfigureSpellHandler();
        }
    }

    public void ScaleMove(Move move, Character user, Character target, int power)
    {
        InitializeMove(move, user, target, power);
        circle.gameObject.SetActive(true);
        MoveCircle();
    }

    private void InitializeMove(Move move, Character user, Character target, int power)
    {
        move.moveOwner = user;
        this.move = move;
        this.user = user;
        this.target = target;
        this.power = power;
        vfx = move.vfx.gameObject;
        repetition = 0;
        initialPos = target.transform.position;
        isEnemy = user.isEnemy;
        circle.isEnemy = isEnemy;
    }

    private void ConfigureBlockHandler()
    {
        blockHandler.SetPower(power);
        blockHandler.moveRepeat = move.moveRepeat;
        blockHandler.SetTotalDamage(power * move.moveRepeat);
        blockHandler.gameObject.SetActive(true);
    }

    private void ConfigureSpellHandler()
    {
        spellHandler.moveRepeat = move.moveRepeat;
        spellHandler.SetPower(power);
        spellHandler.gameObject.SetActive(true);
    }

    private void MoveCircle()
    {
        circle.transform.localPosition = initialPos + new Vector3(Random.Range(-.25f, .25f), Random.Range(-.25f, .25f));
    }

    private void ShowPopupText(int value, Color color, Vector3 offset)
    {
        TMP_Text popupTextClone = Instantiate(popupText, target.transform.position + offset, Quaternion.identity);
        popupTextClone.text = value >= 0 ? $"+{value}" : $"{value}";
        popupTextClone.color = color;
        Destroy(popupTextClone.gameObject, 1f);
    }

    public void ScaleMove(int scaler)
    {
        switch (move)
        {
            case AttackMove:
                if (isEnemy) Defend(scaler); else Attack(scaler);
                break;

            case HealMove:
                Heal(scaler);
                break;

            case RestMove:
                Rest(scaler);
                break;
        }
    }

    public void Attack(int multiplier)
    {
        int damage = CalculateDamage(multiplier);
        MoveCircle();
        repetition++;
        if (multiplier > 0) ShowPopupText(-damage, Color.red, new Vector3(0, 1.3f, 0));
        user.AttackSprite();
        SetTotalDamage(damage);
        target.Damage(damage);
        if (repetition < move.moveRepeat) return;
        FinalizeMove();
    }

    public void Defend(int divider)
    {
        int damage = CalculateDefenseDamage(divider);
        MoveCircle();
        repetition++;
        ShowPopupText(-damage, Color.red, new Vector3(0, 1.3f, 0));
        target.DefendSprite();
        SetTotalDamage(damage);
        target.Damage(damage);
        if (repetition < move.moveRepeat) return;
        FinalizeMove();
    }

    public void Heal(int divider)
    {
        int heal = CalculateHeal(divider);
        MoveCircle();
        repetition++;
        ShowPopupText(heal, Color.green, new Vector3(1, 0, 0));
        target.Heal(heal);
        if (repetition < move.moveRepeat) return;
        FinalizeMove();
    }

    public void Rest(int multiplier)
    {
        int manaRegen = CalculateManaRegen(multiplier);
        MoveCircle();
        repetition++;
        ShowPopupText(manaRegen, Color.blue, new Vector3(1, 0, 0));
        user.RegenMana(manaRegen);
        if (repetition < move.moveRepeat) return;
        FinalizeMove();
    }

    private int CalculateDamage(int multiplier)
    {
        if (multiplier == 0) { audioManager.PlayMissSFX(); return 0; }
        int damage = (multiplier == 2) ? (int)(power * 1.35f) : power;
        if (multiplier == 2) audioManager.PlayCriticalSFX();
        audioManager.PlayHitSFX();
        return damage;
    }

    private int CalculateDefenseDamage(int divider)
    {
        if (divider == 0) { audioManager.PlayHitSFX(); return power * 2; }
        int damage = (divider == 2) ? (int)(power / 1.2f) : power;
        if (divider == 2) audioManager.PlayCriticalSFX();
        audioManager.PlayHitSFX();
        return damage;
    }

    private int CalculateHeal(int divider)
    {
        return divider switch
        {
            0 => power / 2,
            1 => power,
            2 => Mathf.RoundToInt(power * 1.5f),
            _ => power
        };
    }

    private int CalculateManaRegen(int multiplier)
    {
        return multiplier switch
        {
            0 => Mathf.RoundToInt(power / 1.5f),
            1 => power,
            2 => Mathf.RoundToInt(power * 1.5f),
            _ => power
        };
    }

    private void FinalizeMove()
    {
        target.CheckIfDead();
        circle.gameObject.SetActive(false);
        StartCoroutine(ScalingOverDelay());
    }

    private IEnumerator ScalingOverDelay()
    {
        BattleManager.Instance.DefaultCameraView();
        yield return new WaitForSeconds(.5f);
        ScalingOver();
    }

    public void Spell(int totalDamage)
    {
        this.totalDamage = totalDamage;
        StartCoroutine(SpellDelay());
    }

    private IEnumerator SpellDelay()
    {
        yield return new WaitForSeconds(1f);
        ShowPopupText(-totalDamage, Color.red, new Vector3(0, 1.3f, 0));
        user.AttackSprite();
        Instantiate(vfx, target.transform.position, Quaternion.identity);
        target.Damage(totalDamage);
        target.CheckIfDead();
        StartCoroutine(ScalingOverDelay());
    }

    public void ScalingOver()
    {
        repetition = 0;
        circle.gameObject.SetActive(false);
        user.RevertPosition();
        user.NextTurn();
        Invoke(nameof(ResetTotalDamage), .5f);
    }

    public void SetTotalDamage(int addedDamage)
    {
        totalDamage += addedDamage;
        totalDamageText.gameObject.SetActive(true);
        totalDamageText.text = $"Total Damage:\n{totalDamage}";
    }

    public void ResetTotalDamage()
    {
        totalDamage = 0;
        totalDamageText.gameObject.SetActive(false);
    }
}