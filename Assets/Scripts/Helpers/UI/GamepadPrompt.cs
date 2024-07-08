using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamepadPrompt : MonoBehaviour, IInputListener
{
    [SerializeField] private GameObject prompt;
    
    // Start is called before the first frame update
    void Start()
    {
        EventManagers.inputs.registerListener(this);
        onInputSwitch(InputManager.getInputType());
    }

    private void OnDestroy()
    {
        EventManagers.inputs.unregisterListener(this);
    }

    private void OnEnable()
    {
        onInputSwitch(InputManager.getInputType());
    }

    public void onInputSwitch(inputType source)
    {
        prompt.SetActive(source == inputType.gamepad);
    }
}
