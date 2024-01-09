using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public enum selectorType { character, weapon, equipment }

public class SelectorLayout : MonoBehaviour
{
    [SerializeField] selectorType type;
    [SerializeField] DataSelector dataSelector;
    [SerializeField] SelectButton button;
    SelectButton selectedButton;

    void Start()
    {
        switch (type)
        {
            case selectorType.character:
                SetupLayout(ScriptableObjectManager.getCharacters());
                break;

            case selectorType.weapon:
                SetupLayout(ScriptableObjectManager.getWeapons());
                break;

            case selectorType.equipment:
                SetupLayout(ScriptableObjectManager.getEquipment());
                break;
        }
    }

    void SetupLayout<T>(List<T> list) where T : ObjectHandler
    {
        foreach (ObjectHandler handler in list)
        {
            SelectButton newButton = Instantiate(button);
            newButton.transform.SetParent(transform);
            newButton.transform.localScale = Vector3.one;
            newButton.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
            newButton.Setup(handler.getKey(), this);
        }


    }

    public void UpdateSelectedButton(SelectButton newButton)
    {
        if (selectedButton != null) selectedButton.background.color = Color.white;
        selectedButton = newButton;
        selectedButton.background.color = Color.yellow;
    }
}
