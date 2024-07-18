using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character
{
    public override void Awake()
    {
        base.Awake();
        SetCharacter();
    }

    public override void ThisTurn()
    {
        base.ThisTurn();
        SetMoveset();
        battleManager.PlayerTurn(characterName, characterSO.characterPortraitSprite);
    }

    public override void OnMouseDown()
    {
        battleManager.PickTarget(this);
    }

    public override void ExecuteMove()
    {
        UpdateMana(-preselectedMove.manaCost);
        target = battleManager.GetTarget();
        battleManager.FocusMove(this, target);

        CameraManager.Instance.TargetTakingAction(target, preselectedMove, isEnemy);
        Vector3 offset = Vector3.zero;
        if (preselectedMove.moveName != "'Rest'") //If move is not rest
        {
            offset = new Vector3(2f, 0, 0);
            if (preselectedMove.moveName == "'Nature's Embrace'" && target.characterName == "Talindra")
            {
                offset = new Vector3(0f, 0, 0);
            }
        }
        transform.position = target.transform.position - offset;

        battleManager.DisableMoveset();

        battleManager.AnnounceAction(characterName + " uses " + preselectedMove.moveName + " to " + battleManager.GetTarget().characterName);
        Invoke(nameof(ExecuteMoveDelay), 1f);
    }

    private void ExecuteMoveDelay()
    {
        preselectedMove.Execute(this, target);
    }
}