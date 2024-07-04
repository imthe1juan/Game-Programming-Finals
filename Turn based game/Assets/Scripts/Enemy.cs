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
        Invoke(nameof(MoveInvoked), 1f);
    }

    private void MoveInvoked()
    {
        int randomMove = Random.Range(0, moves.Count);
        preselectedMove = moves[randomMove];

        Character target;
        if (preselectedMove.isTargetAlly) //Ally of the Enemy
        {
            target = battleManager.PickRandomEnemy(); // It picks an enemy of the player (ally of the enemy)
            transform.position = target.gameObject.transform.position + new Vector3(1.5f, 0, 0);
        }
        else
        {
            target = battleManager.PickRandomAlly(); // Attacks you/your ally
            transform.position = target.gameObject.transform.position + new Vector3(1.5f, 0, 0);
        }

        this.target = target;
        battleManager.EnemyTurn(characterName + " uses " + preselectedMove.moveName + " to " + target.characterName);

        battleManager.FocusMove(this, target);
        ExecuteMove(preselectedMove);
        CameraManager.Instance.TargetTakingAction(target.transform, isEnemy);
    }

    public override void ExecuteMove(Move move)
    {
        StartCoroutine(ExecuteAfterDelay(move, this.target));
    }

    public override void Heal(int heal)
    {
        base.Heal(heal);
    }

    private IEnumerator ExecuteAfterDelay(Move move, Character target)
    {
        yield return new WaitForSeconds(1);
        move.Execute(this, target);
        if (thisTurn && move.moveName == "Heal")
        {
            Invoke(nameof(NextTurn), .5f);
        }
    }

    public override void OnMouseDown()
    {
        battleManager.PickTarget(this);
    }
}