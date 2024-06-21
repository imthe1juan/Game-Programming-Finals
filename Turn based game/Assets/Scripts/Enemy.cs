using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public override void ThisTurn()
    {
        base.ThisTurn();
        BattleManager.Instance.EnemyTurn(characterName + "'s Turn");
        Invoke(nameof(MoveInvoked), .5f);
    }

    private void MoveInvoked()
    {
        int randomMove = Random.Range(0, moves.Count);
        preselectedMove = moves[randomMove];
        BattleManager.Instance.EnemyTurn(characterName + " uses " + preselectedMove.moveName);

        ExecuteMove(preselectedMove);
    }

    public override void ExecuteMove(Move move)
    {
        Character character;
        if (move.isTargetAlly)
        {
            character = BattleManager.Instance.PickRandomEnemy();
            transform.position = character.gameObject.transform.position + new Vector3(1.5f, 0, 0);
        }
        else
        {
            character = BattleManager.Instance.PickRandomAlly();
            transform.position = character.gameObject.transform.position + new Vector3(1.5f, 0, 0);
        }

        move.Execute(this, character);
        BattleManager.Instance.DisableMoveset();
        Invoke(nameof(RevertPosition), .5f);
    }

    public override void OnMouseDown()
    {
        BattleManager.Instance.PickTarget(this);
    }
}