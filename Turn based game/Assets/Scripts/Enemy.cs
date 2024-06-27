using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Enemy : Character
{
    public override void ThisTurn()
    {
        base.ThisTurn();
        battleManager.EnemyTurn(characterName + "'s Turn");
        Invoke(nameof(MoveInvoked), .5f);
    }

    private void MoveInvoked()
    {
        int randomMove = Random.Range(0, moves.Count);
        preselectedMove = moves[randomMove];
        battleManager.EnemyTurn(characterName + " uses " + preselectedMove.moveName + " to " + battleManager.GetTarget().characterName);

        ExecuteMove(preselectedMove);
    }

    public override void ExecuteMove(Move move)
    {
        Character target;
        if (move.isTargetAlly) //Ally of the Enemy
        {
            target = battleManager.PickRandomEnemy(); // It picks an enemy of the player (ally of the enemy)
            transform.position = target.gameObject.transform.position + new Vector3(1.5f, 0, 0);
        }
        else
        {
            target = battleManager.PickRandomAlly(); // Attacks you/your ally
            transform.position = target.gameObject.transform.position + new Vector3(1.5f, 0, 0);
        }
        StartCoroutine(ExecuteAfterDelay(move, target));
        battleManager.DisableMoveset();
    }

    private IEnumerator ExecuteAfterDelay(Move move, Character target)
    {
        yield return new WaitForSeconds(1);
        move.Execute(this, target);
    }

    public override void OnMouseDown()
    {
        battleManager.PickTarget(this);
    }
}