using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using DG.Tweening;

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
    WaitForSeconds waitForSeconds;
    [HideInInspector] public InputMaster controls;
    public LocalizedStringTable table;
    public List<DialogueEvent> dialogueEvents;
    public List<Event> toDo;
    Animator animator;
    [HideInInspector] public Transform targetTransform;

    void OnEnable() {
        controls.Enable();
    }

    void OnDisable() {
        controls.Disable();
    }

    void Awake()
    {
        controls = new InputMaster();

        sentences = new Queue<string>();
        waitForSeconds = Helpers.GetWait(0.015f);

        controls.Talk.Talk.performed += context => DisplayNextSentence(); 

        animator = GetComponent<Animator>();

        controls.Disable();

        targetTransform = GetComponentInChildren<Transform>();
    }

    public void StartDialogue()
    {
        textDisplay.gameObject.SetActive(true);
        sentences = new Queue<string>();
        sentences.Clear();

        var stringTable = table.GetTable();
        

        foreach (SharedTableData.SharedTableEntry entry in stringTable.SharedData.Entries) {
            sentences.Enqueue(stringTable.GetEntry(entry.Id).LocalizedValue);
            Debug.Log(sentences.Count);
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
        foreach (Event eventAction in toDo) {
            eventAction.Execute();
        }
        animator.SetTrigger("DialogueEnd");
        controls.Disable();
        PlayerController.instance.ConversationEnded();
        
    }
}
