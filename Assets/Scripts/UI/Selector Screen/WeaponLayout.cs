using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class WeaponLayout : MonoBehaviour
{
    [SerializeField] DataSelector dataSelector;
    [SerializeField] SelectButton button;
    [SerializeField] ObjectReferencer objectReferencer;
    SelectButton selectedButton;
    // Start is called before the first frame update
    void Start()
    {
        int maxIndex = System.Enum.GetNames(typeof(weapon)).Length;
        for (int i = 1; i < maxIndex; i++)
        {
            SelectButton newButton = Instantiate(button);
            newButton.transform.SetParent(transform);
            newButton.transform.localScale = Vector3.one;
            newButton.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;

            weapon selectedWeapon = (weapon)i;
            UnityAction action = delegate
            {
                dataSelector.SelectWeapon(selectedWeapon);
                ChangeSelectedButton(newButton);
            };
            newButton.button.onClick.AddListener(action);
            newButton.button.image.sprite = objectReferencer.getWeaponSprite(selectedWeapon);
        }
    }

    void ChangeSelectedButton(SelectButton newButton)
    {
        if (selectedButton != null) selectedButton.background.color = Color.white;
        selectedButton = newButton;
        selectedButton.background.color = Color.yellow;
    }

}
