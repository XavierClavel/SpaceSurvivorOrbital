using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public enum inputType
{
    keyboard,
    gamepad,
    android,
}

public class InputManager : MonoBehaviour
{
    private static GameObject selectedObject;
    private static PlayerInput playerInput;
    private static inputType input;
    
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        Debug.Log("setting up");
    }

    public static void setSelectedObject(GameObject newObject)
    {
        selectedObject = newObject;
        OnSelectChange();
    }


    public static void OnInputChange()
    {
        Debug.Log("input change");
        if (input == inputType.android) return;

        input = getInputType();
        OnSelectChange();
        EventManagers.inputs.dispatchEvent(it => it.onInputSwitch(input));
    }

    public static inputType getInputType() => playerInput.currentControlScheme == Vault.other.inputGamepad ? inputType.gamepad : inputType.keyboard;



    public static void OnSelectChange()
    {
        Debug.Log(selectedObject.name);
        EventSystem.current.SetSelectedGameObject(selectedObject);
        /*
        if (input == inputType.gamepad) EventSystem.current.SetSelectedGameObject(selectedObject);
        else EventSystem.current.SetSelectedGameObject(null);
        */
    }

    private void OnDestroy()
    {
        selectedObject = null;
    }
}
