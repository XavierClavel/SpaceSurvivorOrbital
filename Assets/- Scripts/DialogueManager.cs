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
    public static DialogueManager instance;
    WaitForSeconds waitForSeconds;
    InputMaster controls;
    public LocalizedStringTable table;
    public List<DialogueEvent> dialogueEvents;
    [HideInInspector] public List<Event> toDo;
    Animator animator;
    private void OnValidate() {
        foreach (DialogueEvent dialogueEvent in dialogueEvents) {
            foreach (Event individualEvent in dialogueEvent.events) {
                if (individualEvent.action != actionType.SetParameter) {
                    individualEvent.type = parameterType.None;
                }
            }
        }
    }

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

        animator = GetComponent<Animator>();
    }

    public void StartDialogue()
    {
        textDisplay.gameObject.SetActive(true);
        sentences = new Queue<string>();
        sentences.Clear();

        var stringTable = table.GetTable();
        

        foreach (SharedTableData.SharedTableEntry entry in stringTable.SharedData.Entries) {
            sentences.Enqueue(stringTable.GetEntry(entry.Id).LocalizedValue);
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
            switch (eventAction.action) {

                case actionType.SetParameter :
                    switch (eventAction.type) {

                        case parameterType.Trigger :
                        Debug.Log(eventAction.triggerParameter);
                            eventAction.dialogueManager.animator.SetTrigger(eventAction.triggerParameter);
                            break;
                        
                        case parameterType.Bool :
                            eventAction.dialogueManager.animator.SetBool(eventAction.boolParameter, eventAction.boolValue);
                            break;
                    }
                    break;
                
                case actionType.ChangeDialogue :
                    eventAction.dialogueManager.table = eventAction.table;
                    break;
                
                case actionType.Translate :
                    eventAction.objectTransform.DOMove(eventAction.finalPosition, eventAction.duration);
                    break;

                case actionType.Rotate :
                    eventAction.objectTransform.DORotate(eventAction.finalRotation, eventAction.duration);
                    break;

                case actionType.TranslateAndRotate : 
                    eventAction.objectTransform.DOMove(eventAction.finalPosition, eventAction.duration);
                    eventAction.objectTransform.DORotate(eventAction.finalRotation, eventAction.duration);
                    break;
            }
        
        //gameObject.SetActive(false);
        PlayerController.instance.ConversationEnded();
        }
        
    }
}
