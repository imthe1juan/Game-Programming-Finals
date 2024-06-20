using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Move", menuName = "Move/Heal")]
public class HealMove : Move
{
    public override void Execute(Character user, Character target)
    {
        Debug.Log($"{user.characterName} uses {moveName} on {target.characterName}!");
        target.Heal(power);
    }
}