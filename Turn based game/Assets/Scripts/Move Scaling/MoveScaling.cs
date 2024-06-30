using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoveScaling : MonoBehaviour
{
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
        TMP_Text damageTextClone = Instantiate(popupText, transform.position, Quaternion.identity);
        damageTextClone.transform.localPosition = circle.transform.position + new Vector3(0, 1, 0);
        Destroy(damageTextClone.gameObject, 1);
        repetition++;
        int totalDamage = 0;
        if (multiplier == 0)
        {
            //Missed
            print("Missed");
            totalDamage = 0;
        }
        else if (multiplier == 1)
        {
            print("Hit");
            totalDamage = power;
        }
        else if (multiplier == 2)
        {
            print("POWER!");
            totalDamage = power * 2;
        }
        target.Damage(totalDamage);
        SetTotalDamage(totalDamage);
        damageTextClone.text = $"-{totalDamage}";

        if (repetition < 3) return;
        ScalingOver();
    }

    public void Defend(int divider)
    {
        circle.transform.localPosition = initialPos + new Vector3(Random.Range(-.5f, .5f), Random.Range(-.5f, .5f));
        initialPos = circle.transform.position;

        TMP_Text damageTextClone = Instantiate(popupText, transform.position, Quaternion.identity);
        damageTextClone.transform.localPosition = circle.transform.position + new Vector3(0, 1, 0);
        Destroy(damageTextClone.gameObject, 1);

        repetition++;
        int totalDamage = 0;

        if (divider == 0)
        {
            //Missed
            print("OUCH");
            totalDamage = power * 2;
        }
        else if (divider == 1)
        {
            print("Hit");
            totalDamage = power;
        }
        else if (divider == 2)
        {
            print("DEFEND!");
            totalDamage = (int)(power / 1.5f);
        }
        target.Damage(totalDamage);
        SetTotalDamage(totalDamage);

        damageTextClone.text = $"-{totalDamage}";

        if (repetition < 3) return;
        ScalingOver();
    }

    public void Heal(int divider)
    {
        circle.transform.localPosition = initialPos + new Vector3(Random.Range(-.5f, .5f), Random.Range(-.5f, .5f));
        initialPos = circle.transform.position;
        TMP_Text damageTextClone = Instantiate(popupText, transform.position, Quaternion.identity);
        damageTextClone.transform.localPosition = circle.transform.position + new Vector3(0, 1, 0);
        Destroy(damageTextClone.gameObject, 1);

        repetition++;
        int totalHeal = 0;

        if (divider == 0)
        {
            //Missed
            print("Failed");
            totalHeal = power / 2;
        }
        else if (divider == 1)
        {
            print("Heal!");
            totalHeal = power;
        }
        else if (divider == 2)
        {
            print("RISE!");
            totalHeal = power * 2;
        }
        target.Heal(totalHeal);
        damageTextClone.text = $"+{totalHeal}";

        if (repetition < 1) return;
        ScalingOver();
    }

    public void Spell(int multiplier)
    {
        TMP_Text damageTextClone = Instantiate(popupText, transform.position, Quaternion.identity);
        damageTextClone.transform.localPosition = circle.transform.position + new Vector3(0, 1, 0);
        Destroy(damageTextClone.gameObject, 1);

        int totalDamage = power * multiplier;
        target.Damage(totalDamage);

        damageTextClone.text = $"-{totalDamage}";
        ScalingOver();
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
        repetition = 0;
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