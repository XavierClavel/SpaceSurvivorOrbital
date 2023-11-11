using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public enum selectorType { character, weapon }

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
        }
    }

    void SetupLayout<T>(List<T> list) where T : ObjectHandler
    {
        for (int i = 0; i < list.Count; i++)
        {
            SelectButton newButton = Instantiate(button);
            newButton.transform.SetParent(transform);
            newButton.transform.localScale = Vector3.one;
            newButton.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;

            int value = i;
            T entity = list[value];
            UnityAction action = delegate
            {
                dataSelector.SelectGeneric(entity);
                ChangeSelectedButton(newButton);
            };
            newButton.button.onClick.AddListener(action);
            newButton.button.image.sprite = entity.getIcon();
        }
    }

    void ChangeSelectedButton(SelectButton newButton)
    {
        if (selectedButton != null) selectedButton.background.color = Color.white;
        selectedButton = newButton;
        selectedButton.background.color = Color.yellow;
    }
}
