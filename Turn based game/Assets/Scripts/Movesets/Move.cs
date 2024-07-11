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

    [TextArea(2, 4)]
    public string moveDescription;

    public abstract void Execute(Character user, Character target);
}