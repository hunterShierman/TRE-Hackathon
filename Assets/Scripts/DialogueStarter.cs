using UnityEngine;

public class DialogueStarter : MonoBehaviour
{
    public DialogueManager manager;

    void Start()
    {
        // build sample dialogue in code
        Dialogue temp = ScriptableObject.CreateInstance<Dialogue>();
        temp.lines = new DialogueLine[4];

        temp.lines[0] = new DialogueLine
        {
            speaker = "emerly35",
            text = "hey thats my favorite song too!",
            choices = null
        };
        temp.lines[1] = new DialogueLine
        {
            speaker = "emerly35",
            text = "you always have the best taste in music",
            choices = null
        };
        temp.lines[2] = new DialogueLine
        {
            speaker = "neo",
            text = "hey thanks!",
            choices = new Choice[]
            {
                new Choice { choiceText = "you know it!", nextLineIndex = 3 },
                new Choice { choiceText = "its hacker type!", nextLineIndex = 3 },
                new Choice { choiceText = "but we are teenagers :p", nextLineIndex = 3 },
            }
        };
        temp.lines[3] = new DialogueLine
        {
            speaker = "emerly35",
            text = "wow, going full angsty teenager colors? ;)",
            choices = null
        };

        manager.StartDialogue(temp);
    }
}