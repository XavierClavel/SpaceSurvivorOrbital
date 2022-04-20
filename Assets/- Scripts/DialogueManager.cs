using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class Dialogue
{
    [TextArea(4,10)]
    public string[] sentences;

}

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    private Queue<string> sentences;
    bool isTyping = false;
    [SerializeField] Dialogue dialogue;
    public static DialogueManager instance;
    WaitForSeconds waitForSeconds;
    InputMaster controls;

    void OnEnable() {
        controls.Enable();
    }

    void OnDisable() {
        controls.Disable();
    }

    void Awake()
    {
        instance = this;
        controls = new InputMaster();

        sentences = new Queue<string>();
        waitForSeconds = Helpers.GetWait(0.015f);

        controls.Talk.Talk.performed += context => DisplayNextSentence(); 
    }

    public void StartDialogue()
    {
        textDisplay.gameObject.SetActive(true);
        sentences = new Queue<string>();
        sentences.Clear();
        foreach (string sentence in dialogue.sentences){
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (!isTyping) {
            if (sentences.Count == 0) {
                EndDialogue();
                return;
            }

            string sentence = sentences.Dequeue();
		    StartCoroutine(TypeSentence(sentence));
        }
    }

    IEnumerator TypeSentence (string sentence)
	{
        isTyping = true;
		textDisplay.text = "";


		foreach (char letter in sentence.ToCharArray())     //induce a delay between each letter and play a sound
		{
			textDisplay.text += letter;
            yield return waitForSeconds;
		}
        isTyping = false;
	}

    void EndDialogue()
    {
        textDisplay.gameObject.SetActive(false);
        gameObject.SetActive(false);
        PlayerController.instance.ConversationEnded();
    }
}
