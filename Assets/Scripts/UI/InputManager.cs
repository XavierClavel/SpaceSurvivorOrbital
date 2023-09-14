using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    static GameObject selectedObject;
    static InputManager instance;
    static bool isPlayingWithGamepad = false;
    static PlayerInput playerInput;


    private void Awake()
    {
        SingletonManager.OnInstanciation(this);
        instance = SingletonManager.get<InputManager>();

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
        isPlayingWithGamepad = playerInput.currentControlScheme == Vault.other.inputGamepad;
        OnSelectChange();
        PlayerController.SwitchInput(isPlayingWithGamepad);
    }



    public static void OnSelectChange()
    {
        EventSystem.current.SetSelectedGameObject(selectedObject);
        /*
        if (isPlayingWithGamepad) EventSystem.current.SetSelectedGameObject(selectedObject);
        else EventSystem.current.SetSelectedGameObject(null);
        */
    }
}
