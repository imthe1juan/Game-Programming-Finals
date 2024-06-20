using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character
{
    public override void Awake()
    {
        base.Awake();
    }

    public override void ThisTurn()
    {
        base.ThisTurn();
        SetMoveset();
        BattleManager.Instance.PlayerTurn(characterName, "Attack");
    }

    public override void OnMouseDown()
    {
        BattleManager.Instance.PickTarget(this);
    }

    public override void EnableCharacter()
    {
        sr.color = new Color32(155, 89, 182, 255);
        c2D.enabled = true;
    }

    public override void DisableCharacter()
    {
        sr.color = new Color32(155, 89, 182, 100);
        c2D.enabled = false;
    }
}