using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Move", menuName = "Move/Attack")]
public class AttackMove : Move
{
    public override void Execute(Character user, Character target)
    {
        Debug.Log($"{user.characterName} uses {moveName} on {target.characterName}!");
        target.Damage(power);
    }
}