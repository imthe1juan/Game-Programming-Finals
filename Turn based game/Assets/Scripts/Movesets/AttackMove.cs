using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Move", menuName = "Move/Attack")]
public class AttackMove : Move
{
    private MoveScaling moveScaling;

    public override void Execute(Character user, Character target)
    {
        Debug.Log($"{user.characterName} uses {moveName} on {target.characterName}!");
        moveScaling = FindObjectOfType<MoveScaling>();

        moveScaling.ScaleMove(this, user, target, power);
    }
}