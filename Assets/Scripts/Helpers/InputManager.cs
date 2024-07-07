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
    private static List<IInputListener> inputListeners = new List<IInputListener>();
    
    static GameObject selectedObject;
    static InputManager instance;
    static PlayerInput playerInput;
    private static inputType input;

    public static void registerInputListener(IInputListener inputListener)
    {
        inputListeners.Add(inputListener);
    }

    public static void unregisterInputListener(IInputListener inputListener)
    {
        inputListeners.TryRemove(inputListener);
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        playerInput = GetComponent<PlayerInput>();
    }

    public static void setSelectedObject(GameObject newObject)
    {
        selectedObject = newObject;
        OnSelectChange();
    }


    public static void OnInputChange()
    {
        if (instance == null) return;
        if (input == inputType.android) return;
        
        input = playerInput.currentControlScheme == Vault.other.inputGamepad ? inputType.gamepad : inputType.keyboard ;
        OnSelectChange();
        PlayerController.SwitchInput(input == inputType.gamepad);
        inputListeners.ForEach(it => it.onInputSwitch(input));
    }

    public static inputType getInputType() => input;



    public static void OnSelectChange()
    {
        Debug.Log(input);
        if (input == inputType.gamepad) EventSystem.current.SetSelectedGameObject(selectedObject);
        else EventSystem.current.SetSelectedGameObject(null);
    }
}
