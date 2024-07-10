using System;
using UnityEngine;

public abstract class Move : ScriptableObject
{
    public GameObject vfx;
    public string moveName;
    public int power;
    public int manaCost;
    public int moveRepeat;
    public bool isTargetAlly;

    public abstract void Execute(Character user, Character target);
}