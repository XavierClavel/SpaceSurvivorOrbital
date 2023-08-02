using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public enum selectorType { character, weapon, tool }

public class SelectorLayout : MonoBehaviour
{
    [SerializeField] selectorType type;
    [SerializeField] DataSelector dataSelector;
    [SerializeField] SelectButton button;
    [SerializeField] ObjectReferencer objectReferencer;
    [SerializeField] SpriteReferencer spriteReferencer;
    SelectButton selectedButton;

    void Start()
    {
        switch (type)
        {
            case selectorType.character:
                SetupLayout<character>();
                break;

            case selectorType.weapon:
                SetupLayout<weapon>();
                break;

            case selectorType.tool:
                SetupLayout<tool>();
                break;
        }
    }

    void SetupLayout<TEnum>() where TEnum : struct, IConvertible, IComparable, IFormattable
    {
        int maxIndex = System.Enum.GetNames(typeof(TEnum)).Length;
        for (int i = 1; i < maxIndex; i++)
        {
            SelectButton newButton = Instantiate(button);
            newButton.transform.SetParent(transform);
            newButton.transform.localScale = Vector3.one;
            newButton.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;

            int value = i;
            UnityAction action = delegate
            {
                dataSelector.SelectGeneric<TEnum>(value);
                ChangeSelectedButton(newButton);
            };
            newButton.button.onClick.AddListener(action);
            newButton.button.image.sprite = spriteReferencer.getSpriteGeneric<TEnum>(value);
        }
    }

    void ChangeSelectedButton(SelectButton newButton)
    {
        if (selectedButton != null) selectedButton.background.color = Color.white;
        selectedButton = newButton;
        selectedButton.background.color = Color.yellow;
    }
}
