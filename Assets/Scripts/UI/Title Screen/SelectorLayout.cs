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

    public void Setup()
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
                SetupEquipmentLayout(ScriptableObjectManager.getEquipments());
                break;
        }
    }

    void SetupLayout<T>(List<T> list) where T : HidableObjectHandler
    {
        foreach (HidableObjectHandler handler in list)
        {
            if (!handler.isDiscovered()) continue;
            SelectButton newButton = Instantiate(button, transform, true);
            newButton.transform.localScale = Vector3.one;
            newButton.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
            newButton.Setup(handler.getKey(), this);
        }
    }
    
    void SetupEquipmentLayout(List<EquipmentHandler> list)
    {
        foreach (EquipmentHandler handler in list)
        {
            if (!handler.isDiscovered()) continue;
            ChargedSelectButton newButton = (ChargedSelectButton)Instantiate(button, transform, true);
            newButton.transform.localScale = Vector3.one;
            newButton.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
            newButton.Setup(handler.getKey(), this);
            
            newButton.setCharge(handler.getCharge());
        }
    }

    public void UpdateSelectedButton(SelectButton newButton)
    {
        if (selectedButton != null) selectedButton = newButton;
        return;
        selectedButton.background.color = Color.white;
        
        selectedButton.background.color = Color.yellow;
    }
}
