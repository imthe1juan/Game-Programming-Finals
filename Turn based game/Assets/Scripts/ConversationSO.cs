using UnityEngine;

[CreateAssetMenu(fileName = "Conversation_", menuName = "New Conversation")]
public class ConversationSO : ScriptableObject
{
    public string[] characterName;

    public Sprite[] characterSprite;
    public Dialogue[] dialogue;
}

[System.Serializable]
public struct Dialogue
{
    [TextArea(3, 5)]
    public string dialogueString;

    public int speaker;
}