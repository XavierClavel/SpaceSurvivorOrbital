using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.Serialization;

public class EquipmentSelector : MonoBehaviour, UIPanel
{
    private static EquipmentSelector instance;
    [SerializeField] Button startButton;
    public static string selectedEquipment = string.Empty;

    [Header("Equipment")]
    [SerializeField] private TextMeshProUGUI equipmentTitleDisplay;
    [SerializeField] private GameObject equipmentCostDisplay;
    [SerializeField] private TextMeshProUGUI equipmentCostText;
    [SerializeField] private Image equipmentImage;
    [SerializeField] private Button equipmentBuyButton;
    [SerializeField] private UpgradeDisplay equipmentDisplay;

    private void DisplayEquipment(SelectButton selectButton)
    {
        if (!equipmentImage.gameObject.activeInHierarchy) equipmentImage.gameObject.SetActive(true);
        LocalizationManager.LocalizeTextField(selectButton.key, equipmentTitleDisplay);

        equipmentCostDisplay.SetActive(!selectButton.isUnlocked);
        equipmentBuyButton.gameObject.SetActive(!selectButton.isUnlocked);

        equipmentCostText.SetText(selectButton.cost.ToString());
        equipmentImage.sprite = getIcon(selectButton.key);

        equipmentDisplay.SetupAction(selectButton.Buy);
    }

    public static void DisplayGeneric(string key, SelectButton selectButton)
    {
        if (ScriptableObjectManager.dictKeyToEquipmentHandler.ContainsKey(key))
        {
            instance.DisplayEquipment(selectButton);
            return;
        }
    }

    private void Awake()
    {
        instance = this;
        equipmentImage.gameObject.SetActive(false);
    }

    public RectTransform getUITransform()
    {
        return GetComponent<RectTransform>();
    }

    public static Sprite getIcon(string key)
    {
        if (ScriptableObjectManager.dictKeyToEquipmentHandler.ContainsKey(key))
        {
            return ScriptableObjectManager.dictKeyToEquipmentHandler[key].getIcon();
        }

        Debug.LogWarning("Selected key does not exist");
        return null;
    }

    public static void Reset()
    {
        selectedEquipment = string.Empty;
    }

    public void SelectGeneric<T>(T value) where T : ObjectHandler
    {
        if (typeof(T) == typeof(EquipmentHandler)) SelectEquipment(value.getKey());
    }

    public static void SelectGeneric(string value)
    {
        if (ScriptableObjectManager.dictKeyToEquipmentHandler.ContainsKey(value))
        {
            instance.SelectEquipment(value);
            return;
        }

        Debug.LogWarning("Selected key does not exist");
    }

    public void SelectEquipment(string value)
    {
        selectedEquipment = value;
        SoundManager.PlaySfx(transform, key: "Button_Switch");
        if (selectedEquipment != string.Empty) startButton.interactable = true;
    }

    public static EquipmentHandler getSelectedEquipment()
    {
        return ScriptableObjectManager.dictKeyToEquipmentHandler[selectedEquipment];
    }

}