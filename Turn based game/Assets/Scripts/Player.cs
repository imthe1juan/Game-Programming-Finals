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

    public override void ExecuteMove()
    {
        base.ExecuteMove();
        Character target = BattleManager.Instance.GetTarget();
        transform.position = target.transform.position - new Vector3(1.5f, 0, 0);

        preselectedMove.Execute(this, target);
        BattleManager.Instance.DisableMoveset();
    }
}