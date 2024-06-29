using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Move", menuName = "Move/Heal")]
public class HealMove : Move
{
    private MoveScaling moveScaling;

    public override void Execute(Character user, Character target)
    {
        Debug.Log($"{user.characterName} uses {moveName} on {target.characterName}!");
        moveScaling = FindObjectOfType<MoveScaling>();

        if (user.isEnemy)
        {
            if (user == target)
            {
                user.Heal(power);
            }
            else
            {
                target.Heal(power);
            }
        }
        else
        {
            moveScaling.ScaleMove(this, user, target, power);
        }
    }
}