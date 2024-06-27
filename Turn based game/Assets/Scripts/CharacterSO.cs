using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "", menuName = "New Character")]
public class CharacterSO : ScriptableObject
{
    public string characterName;
    public Sprite characterSprite;
    public int maxHealth;

    public List<Move> moves;
}