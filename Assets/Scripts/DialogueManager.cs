using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI speakerText;
    public TextMeshProUGUI dialogueText;
    public Transform choicesContainer; // parent for choice buttons
    public Button choiceButtonPrefab;  // prefab with a TMP label inside
    public Button continueButton;

    [Header("Typing")]
    public float typeSpeed = 0.03f;
    public bool allowSkip = true;

    [Header("Speaker Colors")]
    public List<string> speakerNames; // parallel lists for inspector convenience
    public List<Color> speakerColors;

    private Dialogue currentDialogue;
    private int currentIndex = 0;
    private Coroutine typingCoroutine;
    private bool isTyping = false;

    void Awake()
    {
        // hide UI initially if desired
    }

    public void StartDialogue(Dialogue dialogue)
    {
        currentDialogue = dialogue;
        currentIndex = 0;
        ShowLine(currentIndex);
    }

    void ClearChoices()
    {
        foreach (Transform t in choicesContainer)
            Destroy(t.gameObject);
    }

    void ShowLine(int index)
    {
        if (index < 0 || currentDialogue == null || index >= currentDialogue.lines.Length)
        {
            EndDialogue();
            return;
        }

        DialogueLine line = currentDialogue.lines[index];

        // Speaker styling
        speakerText.text = line.speaker;
        speakerText.color = GetColorForSpeaker(line.speaker);

        // Typing
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeText(line.text));

        // Choices / Continue
        ClearChoices();
        if (line.choices != null && line.choices.Length > 0)
        {
            continueButton.gameObject.SetActive(false);
            foreach (var choice in line.choices)
            {
                Button b = Instantiate(choiceButtonPrefab, choicesContainer);
                var tmp = b.GetComponentInChildren<TextMeshProUGUI>();
                if (tmp != null) tmp.text = choice.choiceText;
                int next = choice.nextLineIndex;
                b.onClick.RemoveAllListeners();
                b.onClick.AddListener(() => ShowLine(next));
            }
        }
        else
        {
            continueButton.gameObject.SetActive(true);
            continueButton.onClick.RemoveAllListeners();
            continueButton.onClick.AddListener(() => ShowLine(currentIndex + 1));
        }

        currentIndex = index;
    }

    IEnumerator TypeText(string fullText)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char c in fullText)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }
        isTyping = false;
    }

    void Update()
    {
        // skip typing if user clicks / presses space
        if (allowSkip && isTyping && Input.GetMouseButtonDown(0))
        {
            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);
            dialogueText.text = currentDialogue.lines[currentIndex].text;
            isTyping = false;
        }
    }

    Color GetColorForSpeaker(string speaker)
    {
        for (int i = 0; i < speakerNames.Count; i++)
        {
            if (speakerNames[i].Equals(speaker, System.StringComparison.OrdinalIgnoreCase) && i < speakerColors.Count)
                return speakerColors[i];
        }
        // default
        return Color.white;
    }

    void EndDialogue()
    {
        // Optional: hide panel, notify, etc.
        Debug.Log("Dialogue finished.");
        continueButton.gameObject.SetActive(false);
        ClearChoices();
    }
}