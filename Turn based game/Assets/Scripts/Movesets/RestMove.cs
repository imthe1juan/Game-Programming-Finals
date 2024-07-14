using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Move", menuName = "Move/Rest")]
public class RestMove : Move
{
    private MoveScaling moveScaling;

    public override void Execute(Character user, Character target)
    {
        Debug.Log($"{user.characterName} uses {moveName} on {target.characterName}!");
        moveScaling = FindObjectOfType<MoveScaling>();
        if (user.isEnemy)
        {
            user.Heal(power);
            user.RegenMana(power);
        }
        else
        {
            moveScaling.ScaleMove(this, user, target, power);
        }
    }
}