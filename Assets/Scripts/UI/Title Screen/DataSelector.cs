using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.Serialization;

public class DataSelector : MonoBehaviour, UIPanel
{
    private static DataSelector instance;
    [SerializeField] Button startButton;
    public static string selectedCharacter = string.Empty;
    public static string selectedWeapon = string.Empty;
    
    [Header("Character")]
    [SerializeField] private TextMeshProUGUI characterTitleDisplay;
    [SerializeField] private GameObject characterCostDisplay;
    [SerializeField] private TextMeshProUGUI characterCostText;
    [SerializeField] private Image characterImage;
    [SerializeField] private Button characterBuyButton;
    [SerializeField] private UpgradeDisplay characterDisplay;
    
    [Header("Weapon")]
    [SerializeField] private TextMeshProUGUI weaponTitleDisplay;
    [SerializeField] private GameObject weaponCostDisplay;
    [SerializeField] private TextMeshProUGUI weaponCostText;
    [SerializeField] private Image weaponImage;
    [SerializeField] private Button weaponBuyButton;
    [SerializeField] private UpgradeDisplay weaponDisplay;

    private void DisplayCharacter(SelectButton selectButton)
    {
        if (!characterImage.gameObject.activeInHierarchy) characterImage.gameObject.SetActive(true);
        LocalizationManager.LocalizeTextField(selectButton.key, characterTitleDisplay);
        
        characterCostDisplay.SetActive(!selectButton.isUnlocked);
        characterBuyButton.gameObject.SetActive(!selectButton.isUnlocked);
        
        characterCostText.SetText(selectButton.cost.ToString());
        characterImage.sprite = getIcon(selectButton.key);
        
        characterDisplay.SetupAction(selectButton.Buy);
    }
    
    private void DisplayWeapon(SelectButton selectButton)
    {
        if (!weaponImage.gameObject.activeInHierarchy) weaponImage.gameObject.SetActive(true);
        LocalizationManager.LocalizeTextField(selectButton.key, weaponTitleDisplay);
        
        weaponCostDisplay.SetActive(!selectButton.isUnlocked);
        weaponBuyButton.gameObject.SetActive(!selectButton.isUnlocked);
        
        weaponCostText.SetText(selectButton.cost.ToString());
        weaponImage.sprite = getIcon(selectButton.key);
        
        weaponDisplay.SetupAction(selectButton.Buy);
    }

    public static void DisplayGeneric(string key, SelectButton selectButton)
    {
        if (ScriptableObjectManager.dictKeyToCharacterHandler.ContainsKey(key))
        {
            instance.DisplayCharacter(selectButton);
            return;
        }

        if (ScriptableObjectManager.dictKeyToWeaponHandler.ContainsKey(key))
        {
            instance.DisplayWeapon(selectButton);
            return;
        }
    }

    private void Awake()
    {
        instance = this;
        characterImage.gameObject.SetActive(false);
        weaponImage.gameObject.SetActive(false);
    }

    public RectTransform getUITransform()
    {
        return GetComponent<RectTransform>();
    }

    public static Sprite getIcon(string key)
    {
        if (ScriptableObjectManager.dictKeyToCharacterHandler.ContainsKey(key))
        {
            return ScriptableObjectManager.dictKeyToCharacterHandler[key].getIcon();
        }

        if (ScriptableObjectManager.dictKeyToWeaponHandler.ContainsKey(key))
        {
            return ScriptableObjectManager.dictKeyToWeaponHandler[key].getIcon();
        }

        if (ScriptableObjectManager.dictKeyToEquipmentHandler.ContainsKey(key))
        {
            return ScriptableObjectManager.dictKeyToEquipmentHandler[key].getIcon();
        }

        Debug.LogWarning("Selected key does not exist");
        return null;
    }

    public static void Reset()
    {
        selectedCharacter = string.Empty;
        selectedWeapon = string.Empty;
    }

    public void SelectGeneric<T>(T value) where T : ObjectHandler
    {
        if (typeof(T) == typeof(CharacterHandler)) SelectCharacter(value.getKey());
        else if (typeof(T) == typeof(WeaponHandler)) SelectWeapon(value.getKey());
    }
    
    public static void SelectGeneric(string value) 
    {
        if (ScriptableObjectManager.dictKeyToCharacterHandler.ContainsKey(value))
        {
            instance.SelectCharacter(value);
            return;
        }

        if (ScriptableObjectManager.dictKeyToWeaponHandler.ContainsKey(value))
        {
            instance.SelectWeapon(value);
            return;
        }

        Debug.LogWarning("Selected key does not exist");
    }

    public void SelectCharacter(string value)
    {
        selectedCharacter = value;
        SoundManager.PlaySfx(transform, key: "Button_Switch");
        if (selectedWeapon != string.Empty) startButton.interactable = true;
    }

    public void SelectWeapon(string value)
    {
        selectedWeapon = value;
        SoundManager.PlaySfx(transform, key: "Button_Switch");
        if (selectedCharacter != string.Empty) startButton.interactable = true;
    }

    public void Validate()
    {
        PlayerManager.setWeapon(DataManager.dictWeapons[selectedWeapon].Clone(), ScriptableObjectManager.dictKeyToWeaponHandler[selectedWeapon]);
        SoundManager.PlaySfx(transform, key: "Ship_TakeOff");
        PlanetSelector.SelectFirstPlanet();
    }

    public static WeaponHandler getSelectedWeapon()
    {
        return ScriptableObjectManager.dictKeyToWeaponHandler[selectedWeapon];
    }

    public static CharacterHandler getSelectedCharacter()
    {
        return ScriptableObjectManager.dictKeyToCharacterHandler[selectedCharacter];
    }
}