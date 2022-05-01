using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using UnityEngine.Localization;
using System.Linq;
using UnityForge.PropertyDrawers;

public enum actionType {Translate, Rotate, TranslateAndRotate, ChangeDialogue, SetParameter, other}
public enum parameterType {None, Trigger, Bool}

[System.Serializable]
public class Event
{
    public actionType action;
    [ConditionalField(nameof(action), false, actionType.Rotate, actionType.Translate, actionType.TranslateAndRotate)] 
    public Transform objectTransform;


    [ConditionalField(nameof(action), false, actionType.Translate, actionType.TranslateAndRotate)] 
    public Vector3 finalPosition;


    [ConditionalField(nameof(action), false, actionType.Rotate, actionType.TranslateAndRotate)] 
    public Vector3 finalRotation;


    [ConditionalField(nameof(action), false, actionType.Rotate, actionType.Translate, actionType.TranslateAndRotate)] 
    public float duration;


    [ConditionalField(nameof(action), false, actionType.ChangeDialogue)] 
    public LocalizedStringTable table;


    [ConditionalField(nameof(action), false, actionType.ChangeDialogue)] 
    public bool self;


    [ConditionalField(nameof(self), true)] 
    public DialogueManager dialogueManager;


    [SerializeField] Animator animator;
    [ConditionalField(nameof(action), false, actionType.SetParameter)] 
    public parameterType type;


    [ConditionalField(nameof(type), false, parameterType.Trigger)] 
    [AnimatorParameterName(AnimatorControllerParameterType.Trigger, animatorField: "animator")] 
    public string triggerParameter;


    [ConditionalField(nameof(type), false, parameterType.Bool)] 
    [AnimatorParameterName(AnimatorControllerParameterType.Bool, animatorField: "animator")] 
    public string boolParameter;

    [ConditionalField(nameof(type), false, parameterType.Bool)] public bool boolValue;


}

[System.Serializable]
public class DialogueEvent
{
    #if UNITY_EDITOR
    public string Name;
    #endif
    [AnimatorStateName] public string stateName;
    public List<Event> events;
}
