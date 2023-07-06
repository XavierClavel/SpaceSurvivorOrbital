using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CharacterLayout : MonoBehaviour
{
    [SerializeField] DataSelector dataSelector;
    [SerializeField] SelectButton button;
    [SerializeField] ObjectReferencer objectReferencer;
    SelectButton selectedButton;
    delegate void actionDelegate(int i);
    // Start is called before the first frame update
    void Start()
    {
        int maxIndex = System.Enum.GetNames(typeof(character)).Length;
        for (int i = 1; i < maxIndex; i++)
        {
            SelectButton newButton = Instantiate(button);
            newButton.transform.SetParent(transform);
            newButton.transform.localScale = Vector3.one;
            newButton.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;

            character selectedCharacter = (character)i;
            UnityAction action = delegate
            {
                dataSelector.SelectCharacter(selectedCharacter);
                ChangeSelectedButton(newButton);
            };
            newButton.button.onClick.AddListener(action);
            newButton.button.image.sprite = objectReferencer.getCharacterSprite(selectedCharacter);
        }
    }

    void ChangeSelectedButton(SelectButton newButton)
    {
        if (selectedButton != null) selectedButton.background.color = Color.white;
        selectedButton = newButton;
        selectedButton.background.color = Color.yellow;
    }

}
