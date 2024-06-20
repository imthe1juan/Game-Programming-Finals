using System;
using UnityEngine;

public abstract class Move : ScriptableObject
{
    public string moveName;
    public int power;
    public float cooldown;
    public bool isTargetAlly;

    public abstract void Execute(Character user, Character target);
}