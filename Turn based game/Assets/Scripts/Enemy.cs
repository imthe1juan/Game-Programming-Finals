using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public override void ThisTurn()
    {
        base.ThisTurn();
        BattleManager.Instance.EnemyTurn(characterName + "'s Turn");
        Invoke(nameof(MoveInvoked), 1);
    }

    private void MoveInvoked()
    {
        int randomMove = Random.Range(0, moves.Count);
        ExecuteMove(moves[randomMove]);
    }

    public override void ExecuteMove(Move move)
    {
        move.Execute(this, BattleManager.Instance.PickRandomAlly());
        BattleManager.Instance.DisableMoveset();
    }

    public override void OnMouseDown()
    {
        BattleManager.Instance.PickTarget(this);
    }

    public override void EnableCharacter()
    {
        sr.color = new Color32(231, 76, 60, 255);
        c2D.enabled = true;
    }

    public override void DisableCharacter()
    {
        sr.color = new Color32(231, 76, 60, 100);
        c2D.enabled = false;
    }
}