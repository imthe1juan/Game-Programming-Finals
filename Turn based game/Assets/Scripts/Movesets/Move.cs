using System;
using UnityEngine;

public abstract class Move : ScriptableObject
{
    public GameObject vfx;
    public string moveName;
    public Sprite moveSprite;
    public int power;
    public int manaCost;
    public int moveRepeat;
    public bool isTargetAlly;
    public bool isTargetSelf;

    [TextArea(2, 4)]
    public string moveDescription;

    public abstract void Execute(Character user, Character target);
}