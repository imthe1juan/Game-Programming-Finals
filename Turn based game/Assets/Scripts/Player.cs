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
        battleManager.PlayerTurn(characterName);
    }

    public override void OnMouseDown()
    {
        battleManager.PickTarget(this);
    }

    public override void ExecuteMove()
    {
        base.ExecuteMove();
        Character target = battleManager.GetTarget();
        battleManager.AnnounceAction(characterName + " uses " + preselectedMove.moveName + " to " + battleManager.GetTarget().characterName);
        preselectedMove.Execute(this, target);

        transform.position = target.transform.position - new Vector3(1.5f, 0, 0);

        battleManager.DisableMoveset();
    }
}