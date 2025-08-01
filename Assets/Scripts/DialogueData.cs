using System;
using UnityEngine;

[Serializable]
public class Choice
{
    public string choiceText;
    public int nextLineIndex; // index in Dialogue.lines; -1 for end
}

[Serializable]
public class DialogueLine
{
    public string speaker; // astranout
    [TextArea(2, 6)]
    public string text;
    public Choice[] choices; // leave empty or null for linear continuation
}

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/Conversation")]
public class Dialogue : ScriptableObject
{
    public DialogueLine[] lines;
}