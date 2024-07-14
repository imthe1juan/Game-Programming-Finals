using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "", menuName = "New Character")]
public class CharacterSO : ScriptableObject
{
    public string characterName;
    public Sprite characterPortraitSprite;
    public Sprite characterDefaultSprite;
    public Sprite characterAttackSprite;
    public Sprite characterDefendSprite;

    public int maxHealth;
    public int maxMana;

    public List<Move> moves;
}