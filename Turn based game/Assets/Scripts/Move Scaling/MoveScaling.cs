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
    private int moveRepeat;
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
        this.move = move;
        this.user = user;
        this.target = target;
        this.power = power;
        moveRepeat = move.moveRepeat;
        vfx = move.vfx;
        initialPos = target.transform.position;
        isEnemy = user.isEnemy;
        circle.isEnemy = isEnemy;
        if (user.isEnemy)
        {
            blockHandler.SetPower(power);
            blockHandler.moveRepeat = move.moveRepeat;
            blockHandler.SetTotalDamage(power * moveRepeat);
            blockHandler.gameObject.SetActive(true);
        }
        else
        {
            spellHandler.moveRepeat = moveRepeat;
            spellHandler.gameObject.SetActive(true);
            spellHandler.SetPower(power);
        }
    }

    public void ScaleMove(Move move, Character user, Character target, int power)
    {
        this.move = move;
        this.user = user;
        this.target = target;
        this.power = power;
        moveRepeat = move.moveRepeat;
        vfx = move.vfx;
        initialPos = target.transform.position;
        isEnemy = user.isEnemy;

        circle.isEnemy = isEnemy;
        circle.gameObject.SetActive(true);
        circle.transform.localPosition = initialPos + new Vector3(Random.Range(-.25f, .25f), Random.Range(-.25f, .25f));
    }

    public void Attack(int multiplier)
    {
        circle.transform.localPosition = initialPos + new Vector3(Random.Range(-.25f, .25f), Random.Range(-.25f, .25f));
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
            damage = (int)(power * 1.35f);
            audioManager.PlayHitSFX();
            audioManager.PlayCriticalSFX();
        }
        TMP_Text popupTextClone = Instantiate(popupText, target.transform.position + new Vector3(0, 1.3f, 0), Quaternion.identity);
        Destroy(popupTextClone.gameObject, .5f);

        if (multiplier > 0)
        {
            popupTextClone.text = $"-{damage}";
        }
        else
        {
            popupTextClone.text = $"Miss!";
        }
        user.AttackSprite();

        SetTotalDamage(damage);

        target.Damage(damage);

        Debug.Log(moveRepeat);
        if (repetition < moveRepeat) return;
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

    public void Defend(int divider)
    {
        circle.transform.localPosition = initialPos + new Vector3(Random.Range(-.25f, .25f), Random.Range(-.25f, .25f));
        initialPos = circle.transform.position;

        repetition++;

        int damage = 0;
        if (divider == 0)
        {
            //Block Failed
            audioManager.PlayHitSFX();
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
            audioManager.PlayCriticalSFX();

            damage = (int)(power / 1.2f);
        }

        target.DefendSprite();

        TMP_Text popupTextClone = Instantiate(popupText, target.transform.position + new Vector3(0, 1.3f, 0), Quaternion.identity);
        popupTextClone.text = $"-{damage}";
        Destroy(popupTextClone.gameObject, .5f);

        SetTotalDamage(damage);
        target.Damage(damage);

        if (repetition < moveRepeat) return;
        target.CheckIfDead();

        circle.gameObject.SetActive(false);

        StartCoroutine(ScalingOverDelay());
    }

    public void Heal(int divider)
    {
        circle.transform.localPosition = initialPos + new Vector3(Random.Range(-.25f, .25f), Random.Range(-.25f, .25f));
        TMP_Text popupTextClone = Instantiate(popupText, target.transform.position + new Vector3(1, 0, 0), Quaternion.identity);
        Destroy(popupTextClone.gameObject, 1f);

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
            totalHeal = Mathf.RoundToInt(power * 1.5f);
        }
        target.Heal(totalHeal);
        popupTextClone.color = Color.green;
        popupTextClone.text = $"+{totalHeal}";

        if (repetition < moveRepeat) return;
        circle.gameObject.SetActive(false);
        StartCoroutine(ScalingOverDelay());
    }

    public void Rest(int multiplier)
    {
        circle.transform.localPosition = initialPos + new Vector3(Random.Range(-.25f, .25f), Random.Range(-.25f, .25f));

        initialPos = circle.transform.position;

        repetition++;
        int totalManaRegen = 0;

        if (multiplier == 0)
        {
            //Missed
            totalManaRegen = Mathf.RoundToInt(power / 1.5f);
        }
        else if (multiplier == 1)
        {
            totalManaRegen = power;
        }
        else if (multiplier == 2)
        {
            totalManaRegen = Mathf.RoundToInt(power * 1.5f);
        }

        TMP_Text popupTextMana = Instantiate(popupText, target.transform.position + new Vector3(1, 0, 0), Quaternion.identity);
        Destroy(popupTextMana.gameObject, 1f);
        user.Heal(Mathf.RoundToInt(totalManaRegen / 1.5f));
        user.RegenMana(totalManaRegen);

        popupTextMana.text = $"<color=#00FF00>+{Mathf.RoundToInt(totalManaRegen / 1.5f)}</color>\n<color=#2323FF>+{totalManaRegen}</color>";

        if (repetition < moveRepeat) return;
        circle.gameObject.SetActive(false);
        StartCoroutine(ScalingOverDelay());
    }

    public void Spell(int totalDamage)
    {
        this.totalDamage = totalDamage;

        StartCoroutine(SpellDelay());
    }

    private IEnumerator SpellDelay()
    {
        yield return new WaitForSeconds(1f);
        TMP_Text popupTextClone = Instantiate(popupText, target.transform.position + new Vector3(0, 1.3f, 0), Quaternion.identity);
        Destroy(popupTextClone.gameObject, 1);

        popupTextClone.text = $"-{totalDamage}";

        user.AttackSprite();
        GameObject vfxClone = Instantiate(vfx, target.transform.position, Quaternion.identity);
        Destroy(vfxClone.gameObject, 1f);

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
            case RestMove:
                {
                    Rest(scaler);

                    return;
                }
        }
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
        totalDamageText.text = $"Total Damage:\n{(int)(totalDamage)}";
    }

    public void ResetTotalDamage()
    {
        totalDamage = 0;
        totalDamageText.gameObject.SetActive(false);
    }
}