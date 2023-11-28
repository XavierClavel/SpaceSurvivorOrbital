using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TitleScreen : MonoBehaviour, UIPanel
{
    private static TitleScreen instance;
    public static bool isSelectionFree { get; private set; }
    [SerializeField] private TextMeshProUGUI soulsDisplay;

    [SerializeField] TextMeshProUGUI characterTitleDisplay;
    [SerializeField] TextMeshProUGUI characterCostDisplay;
    [SerializeField] TextMeshProUGUI weaponTitleDisplay;
    [SerializeField] TextMeshProUGUI weaponCostDisplay;
    static UnityAction buyAction;
    static GameObject selectedButton;

    [Header("Debug")] 
    [SerializeField] private bool freeSelection;

    private void Awake()
    {
        isSelectionFree = freeSelection;
        instance = this;
        UpdateSoulsDisplay();

        characterCostDisplay.SetText("");
        characterTitleDisplay.SetText("");
        weaponCostDisplay.SetText("");
        weaponTitleDisplay.SetText("");
        buyAction = delegate { };
    }

    // Start is called before the first frame update
    public void Setup()
    {
        ResetManager.Reset();
    }

    public RectTransform getUITransform()
    {
        return GetComponent<RectTransform>();
    }

    public void ResetSave()
    {
        SaveManager.Reset();
        UpdateSoulsDisplay();
    }

    public void AddSouls()
    {
        PlayerManager.setSouls(PlayerManager.getSouls() + 100);
        UpdateSoulsDisplay();
        
    }

    public static void UpdateSoulsDisplay()
    {
        instance.soulsDisplay.SetText(SaveManager.retrieveSouls().ToString());
    }

    public static void DisplayUpgrade(string key)
    {
        LocalizationManager.LocalizeTextField(key + Vault.key.ButtonTitle, instance.characterTitleDisplay);
        LocalizationManager.LocalizeTextField(key + Vault.key.ButtonDescription, instance.characterCostDisplay);
        LocalizationManager.LocalizeTextField(key + Vault.key.ButtonTitle, instance.weaponTitleDisplay);
        LocalizationManager.LocalizeTextField(key + Vault.key.ButtonDescription, instance.weaponCostDisplay);
    }
    public void OnClick()
    {
        buyAction.Invoke();
        EventSystem.current.SetSelectedGameObject(selectedButton);
    }

    public static void SetupBuyButton(UnityAction action, GameObject selected)
    {
        buyAction = action;
        selectedButton = selected;
    }

}

public static class ResetManager
{
    public static void Reset()
    {
        Debug.Log("game state reset");
        NodeManager.Reset();
        DataSelector.Reset();
        PlayerManager.ResetTimer();
        PlayerManager.Reset();
        PlanetSelectionManager.GenerateData();
        Planet.Reset();
    }

}