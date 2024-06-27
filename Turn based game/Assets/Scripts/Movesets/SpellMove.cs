using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellMove : Move
{
    private MoveScaling moveScaling;

    public override void Execute(Character user, Character target)
    {
        Debug.Log($"{user.characterName} uses {moveName} on {target.characterName}!");
        moveScaling = FindObjectOfType<MoveScaling>();

        moveScaling.ScaleMove(this, user, target, power);
    }
}
