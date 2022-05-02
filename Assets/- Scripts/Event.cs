using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using UnityEngine.Localization;
using System.Linq;
using UnityForge.PropertyDrawers;
using DG.Tweening;

public enum actionType {Translate, Rotate, TranslateAndRotate, SetParameter, other}
public enum parameterType {None, Trigger, Bool}

[System.Serializable]
public class Event : ISerializationCallbackReceiver
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


    [ConditionalField(nameof(action), false, actionType.SetParameter)] 
    public Animator animator;

    [ConditionalField(nameof(action), false, actionType.SetParameter)] 
    public parameterType type;


    [ConditionalField(nameof(type), false, parameterType.Trigger)] 
    [AnimatorParameterName(AnimatorControllerParameterType.Trigger, animatorField: "animator")] 
    public string triggerParameter;


    [ConditionalField(nameof(type), false, parameterType.Bool)] 
    [AnimatorParameterName(AnimatorControllerParameterType.Bool, animatorField: "animator")] 
    public string boolParameter;

    [ConditionalField(nameof(type), false, parameterType.Bool)] public bool boolValue;

    public void Execute()
    {
        switch (action) {

            case actionType.SetParameter :
                switch (type) {

                    case parameterType.Trigger :
                        Debug.Log(triggerParameter);
                        animator.SetTrigger(triggerParameter);
                        break;
                    
                    case parameterType.Bool :
                        animator.SetBool(boolParameter, boolValue);
                        break;
                }
                break;
            
            case actionType.Translate :
                objectTransform.DOMove(finalPosition, duration);
                break;

            case actionType.Rotate :
                objectTransform.DORotate(finalRotation, duration);
                break;

            case actionType.TranslateAndRotate : 
                objectTransform.DOMove(finalPosition, duration);
                objectTransform.DORotate(finalRotation, duration);
                break;
            }
    }

    private void OnValidate() {
        if (action != actionType.SetParameter) {
            type = parameterType.None;
        }
    }

    void ISerializationCallbackReceiver.OnBeforeSerialize () => this.OnValidate();
    void ISerializationCallbackReceiver.OnAfterDeserialize () {}


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
