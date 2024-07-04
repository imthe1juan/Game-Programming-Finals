using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoveScaling : MonoBehaviour
{
    private AudioManager audioManager;
    [SerializeField] private TMP_Text totalDamageText;
    [SerializeField] private SpellHandler spellHandler;

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

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void ScaleSpellMove(Move move, Character user, Character target, int power)
    {
        this.move = move;
        this.user = user;
        this.target = target;
        this.power = power;
        initialPos = target.transform.position;
        isEnemy = user.isEnemy;
        spellHandler.gameObject.SetActive(true);
        spellHandler.SetPower(power);
    }

    public void ScaleMove(Move move, Character user, Character target, int power)
    {
        this.move = move;
        this.user = user;
        this.target = target;
        this.power = power;
        initialPos = target.transform.position;
        isEnemy = user.isEnemy;
        circle.gameObject.SetActive(true);
        circle.transform.localPosition = initialPos + new Vector3(Random.Range(-.5f, .5f), Random.Range(-.5f, .5f));
    }

    public void Attack(int multiplier)
    {
        circle.transform.localPosition = initialPos + new Vector3(Random.Range(-.5f, .5f), Random.Range(-.5f, .5f));
        initialPos = circle.transform.position;

        repetition++;
        int damage = 0;

        if (multiplier == 0)
        {
            //Missed
            audioManager.PlayMissSFX();
            damage = 0;
        }
        else if (multiplier == 1)
        {
            damage = power;
            audioManager.PlayHitSFX();
        }
        else if (multiplier == 2)
        {
            damage = (int)(power * 1.5f);
            audioManager.PlayCriticalSFX();
        }

        user.AttackSprite();

        TMP_Text popupTextClone = Instantiate(popupText, target.transform.position + new Vector3(0, 1.3f, 0), Quaternion.identity);
        Destroy(popupTextClone.gameObject, .5f);
        popupTextClone.text = $"-{damage}";
        SetTotalDamage(damage);

        target.Damage(damage);

        if (repetition < 3) return;
        target.CheckIfDead();

        repetition = 0;
        circle.gameObject.SetActive(false);

        StartCoroutine(ScalingOverDelay());
    }

    private IEnumerator ScalingOverDelay()
    {
        yield return new WaitForSeconds(.5f);

        ScalingOver();
    }

    public void Defend(int divider)
    {
        circle.transform.localPosition = initialPos + new Vector3(Random.Range(-.5f, .5f), Random.Range(-.5f, .5f));

        initialPos = circle.transform.position;

        repetition++;

        int damage = 0;
        if (divider == 0)
        {
            //Block Failed
            audioManager.PlayCriticalSFX();
            damage = power * 2;
        }
        else if (divider == 1)
        {
            audioManager.PlayHitSFX();
            damage = power;
        }
        else if (divider == 2)
        {
            audioManager.PlayBlockSFX();

            damage = (int)(power / 1.5f);
        }

        target.DefendSprite();

        TMP_Text popupTextClone = Instantiate(popupText, target.transform.position + new Vector3(0, 1.3f, 0), Quaternion.identity);
        popupTextClone.text = $"-{damage}";
        Destroy(popupTextClone.gameObject, .5f);

        SetTotalDamage(damage);
        target.Damage(damage);

        if (repetition < 3) return;
        target.CheckIfDead();

        repetition = 0;
        circle.gameObject.SetActive(false);

        StartCoroutine(ScalingOverDelay());
    }

    public void Heal(int divider)
    {
        circle.transform.localPosition = initialPos + new Vector3(Random.Range(-.5f, .5f), Random.Range(-.5f, .5f));
        TMP_Text popUpTextClone = Instantiate(popupText, transform.position + new Vector3(0, 1.3f, 0), Quaternion.identity);
        Destroy(popUpTextClone.gameObject, .5f);
        initialPos = circle.transform.position;

        repetition++;
        int totalHeal = 0;

        if (divider == 0)
        {
            //Missed

            totalHeal = power / 2;
        }
        else if (divider == 1)
        {
            totalHeal = power;
        }
        else if (divider == 2)
        {
            totalHeal = power * 2;
        }
        target.Heal(totalHeal);
        popUpTextClone.text = $"+{totalHeal}";

        if (repetition < 1) return;
        ScalingOver();
    }

    public void Spell(int multiplier)
    {
        totalDamage = power * multiplier;

        StartCoroutine(SpellDelay());
    }

    private IEnumerator SpellDelay()
    {
        yield return new WaitForSeconds(1f);
        TMP_Text popupTextClone = Instantiate(popupText, target.transform.position + new Vector3(0, 1.3f, 0), Quaternion.identity);
        Destroy(popupTextClone.gameObject, 1);

        popupTextClone.text = $"-{totalDamage}";

        user.AttackSprite();

        audioManager.PlayEarthSpellSFX();
        target.Damage(totalDamage);
        target.CheckIfDead();

        StartCoroutine(ScalingOverDelay());
    }

    public void ScaleMove(int scaler)
    {
        switch (move)
        {
            case AttackMove:
                {
                    if (isEnemy)
                    {
                        Defend(scaler);
                    }
                    else
                    {
                        Attack(scaler);
                    }
                    return;
                }
            case HealMove:
                {
                    Heal(scaler);

                    return;
                }
            case SpellMove:
                {
                    Spell(scaler);

                    return;
                }
        }
    }

    public void ScalingOver()
    {
        circle.gameObject.SetActive(false);
        user.RevertPosition();
        user.NextTurn();

        Invoke(nameof(ResetTotalDamage), 1f);
    }

    public void SetTotalDamage(int addedDamage)
    {
        totalDamage += addedDamage;
        totalDamageText.gameObject.SetActive(true);
        totalDamageText.text = $"Total Damage:\n{(int)(totalDamage)}";
    }

    public void ResetTotalDamage()
    {
        totalDamage = 0;
        totalDamageText.gameObject.SetActive(false);
    }
}