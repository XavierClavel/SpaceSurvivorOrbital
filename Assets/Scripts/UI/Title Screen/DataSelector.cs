using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.Serialization;
using System.Linq;

public class DataSelector : MonoBehaviour, UIPanel
{
    public static DataSelector instance;
    [SerializeField] Button startButton;
    public static string selectedCharacter = string.Empty;
    public static string selectedWeapon = string.Empty;
    
    [Header("Character")]
    [SerializeField] private TextMeshProUGUI characterTitleDisplay;
    [SerializeField] private GameObject characterCostDisplay;
    [SerializeField] private TextMeshProUGUI characterCostText;
    [SerializeField] private TextMeshProUGUI characterDescriptionText;
    [SerializeField] private Image characterImage;
    [SerializeField] private Button characterBuyButton;
    [SerializeField] private UpgradeDisplay characterDisplay;
    
    [Header("Weapon")]
    [SerializeField] private TextMeshProUGUI weaponTitleDisplay;
    [SerializeField] private GameObject weaponCostDisplay;
    [SerializeField] private TextMeshProUGUI weaponCostText;
    [SerializeField] private TextMeshProUGUI weaponDescriptionText;
    [SerializeField] private Image weaponImage;
    [SerializeField] private Button weaponBuyButton;
    [SerializeField] private UpgradeDisplay weaponDisplay;

    public static HashSet<string> selectedEquipments = new HashSet<string>();

    [Header("Equipment")]
    [SerializeField] private TextMeshProUGUI equipmentTitleDisplay;
    [SerializeField] private GameObject equipmentCostDisplay;
    [SerializeField] private TextMeshProUGUI equipmentCostText;
    [SerializeField] private TextMeshProUGUI equipmentDescriptionText;
    [SerializeField] private Image equipmentImage;
    [SerializeField] private Button equipmentBuyButton;
    [SerializeField] private UpgradeDisplay equipmentDisplay;
    [SerializeField] private ChargedSelectButton equipmentChargeDisplay;

    public Dictionary<string, SelectButton> dictKeyToButton = new Dictionary<string, SelectButton>();
    [SerializeField] List<SelectorLayout> selectorLayouts;

    private int currentEquipmentCharge = 0;
    private int maxEquipmentCharge = 4;
    private int maxMaxEquipmentCharge = 10;

    [SerializeField] private GameObject buyChargeButton;
    [SerializeField] private TextMeshProUGUI chargeCostDisplay;

    [SerializeField] private List<ChargeIndicator> chargeIndicators;

    public Color colorUnfilled;
    public Color colorFilled;
    public Color colorLocked;

    private int currentChargeCost;
    private bool validated = false;


    public void BuyCharge()
    {
        if (!DataSelector.Transaction(currentChargeCost)) return;
        maxEquipmentCharge++;
        SaveManager.setCharge(maxEquipmentCharge);
        UpdateCurrentCharge();
    }

    private void UpdateBuyChargeButton()
    {
        buyChargeButton.SetActive(maxEquipmentCharge < maxMaxEquipmentCharge);
        currentChargeCost = ConstantsData.chargeBaseCost + (maxEquipmentCharge - 3) * ConstantsData.chargeCostIncrement;
        chargeCostDisplay.SetText(currentChargeCost.ToString());
    }

    private void DisplayCharacter(SelectButton selectButton)
    {
        if (!characterImage.gameObject.activeInHierarchy) characterImage.gameObject.SetActive(true);
        LocalizationManager.LocalizeTextField(selectButton.key, characterTitleDisplay);
        LocalizationManager.LocalizeTextField(selectButton.key + Vault.key.ButtonDescription, characterDescriptionText);
        
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
        LocalizationManager.LocalizeTextField(selectButton.key + Vault.key.ButtonDescription, weaponDescriptionText);
        
        weaponCostDisplay.SetActive(!selectButton.isUnlocked);
        weaponBuyButton.gameObject.SetActive(!selectButton.isUnlocked);
        
        weaponCostText.SetText(selectButton.cost.ToString());
        weaponImage.sprite = getIcon(selectButton.key);
        
        weaponDisplay.SetupAction(selectButton.Buy);
    }

    private void DisplayEquipment(SelectButton selectButton)
    {
        if (!equipmentImage.gameObject.activeInHierarchy) equipmentImage.gameObject.SetActive(true);
        LocalizationManager.LocalizeTextField(selectButton.key, equipmentTitleDisplay);
        LocalizationManager.LocalizeTextField(selectButton.key + Vault.key.ButtonDescription, equipmentDescriptionText);
        equipmentChargeDisplay.gameObject.SetActive(true);
        equipmentChargeDisplay.setCharge(ScriptableObjectManager.dictKeyToEquipmentHandler[selectButton.key].getCharge());
        
        equipmentCostDisplay.SetActive(!selectButton.isUnlocked);
        equipmentBuyButton.gameObject.SetActive(!selectButton.isUnlocked);

        equipmentCostText.SetText(selectButton.cost.ToString());
        equipmentImage.sprite = getIcon(selectButton.key);

        equipmentDisplay.SetupAction(selectButton.Buy);
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

        if (ScriptableObjectManager.dictKeyToEquipmentHandler.ContainsKey(key))
        {
            instance.DisplayEquipment(selectButton);
            return;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        characterImage.gameObject.SetActive(false);
        weaponImage.gameObject.SetActive(false);
        equipmentImage.gameObject.SetActive(false);

        selectorLayouts.ForEach(it => it.Setup());

        dictKeyToButton[DataManager.dictWeapons.Keys.ToList()[0]].Select();
        dictKeyToButton[ScriptableObjectManager.dictKeyToCharacterHandler.Keys.ToList()[0]].Select();

        maxEquipmentCharge = SaveManager.getCharge();

        UpdateCurrentCharge();
    }

    public void UpdateCurrentCharge()
    {
        for (int i = 0; i < chargeIndicators.Count; i++)
        {
            if (i < currentEquipmentCharge) chargeIndicators[chargeIndicators.Count - 1 - i].Fill();
            else if (i < maxEquipmentCharge) chargeIndicators[chargeIndicators.Count - 1 - i].Unfill();
            else chargeIndicators[chargeIndicators.Count - 1 - i].Lock();
        }
        UpdateBuyChargeButton();
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
        selectedCharacter = null;
        selectedWeapon = null;
        selectedEquipments = new HashSet<string>();
    }

    public void SelectGeneric<T>(T value) where T : ObjectHandler
    {
        if (typeof(T) == typeof(CharacterHandler)) SelectCharacter(value.getKey());
        else if (typeof(T) == typeof(EquipmentHandler)) SelectEquipment(value.getKey());
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

        if (ScriptableObjectManager.dictKeyToEquipmentHandler.ContainsKey(value))
        {
            instance.SelectEquipment(value);
            return;
        }

        Debug.LogWarning("Selected key does not exist");
    }

    public void SelectCharacter(string value)
    {
        if (selectedCharacter != null) dictKeyToButton[selectedCharacter].onDeselect();
        dictKeyToButton[value].onSelect();
        selectedCharacter = value;
        SoundManager.PlaySfx(transform, key: "Button_Switch");
        if (selectedWeapon != string.Empty) startButton.interactable = true;
    }

    public void SelectWeapon(string value)
    {
        if (selectedWeapon != null) dictKeyToButton[selectedWeapon].onDeselect();
        dictKeyToButton[value].onSelect();
        selectedWeapon = value;
        SoundManager.PlaySfx(transform, key: "Button_Switch");
        if (selectedCharacter != string.Empty) startButton.interactable = true;
    }
    public void SelectEquipment(string value)
    {
        
        if (selectedEquipments.Contains(value)) //unselect
        {
            selectedEquipments.Remove(value);
            dictKeyToButton[value].onDeselect();
            currentEquipmentCharge -= ScriptableObjectManager.dictKeyToEquipmentHandler[value].getCharge();
        }
        else //select
        {
            if (ScriptableObjectManager.dictKeyToEquipmentHandler[value].getCharge() + currentEquipmentCharge > maxEquipmentCharge)
            {
                return;
            }
            selectedEquipments.Add(value);
            dictKeyToButton[value].onSelect();
            currentEquipmentCharge += ScriptableObjectManager.dictKeyToEquipmentHandler[value].getCharge();
        }
        UpdateCurrentCharge();
        SoundManager.PlaySfx(transform, key: "Button_Switch");
    }

    public void Validate()
    {
        if (validated) return;
        validated = true;
        PlayerManager.setCharacter(ScriptableObjectManager.dictKeyToCharacterHandler[selectedCharacter]);
        PlayerManager.setWeapon(DataManager.dictWeapons[selectedWeapon].Clone(), ScriptableObjectManager.dictKeyToWeaponHandler[selectedWeapon]);
        foreach (var equipment in selectedEquipments)
        {
            PlayerManager.AcquireEquipment(equipment);
        }
        SoundManager.PlaySfx(transform, key: "Ship_TakeOff");
        SceneTransitionManager.TransitionToScene(gameScene.ship);
    }

    public static WeaponHandler getSelectedWeapon()
    {
        return ScriptableObjectManager.dictKeyToWeaponHandler[selectedWeapon];
    }

    public static CharacterHandler getSelectedCharacter()
    {
        return ScriptableObjectManager.dictKeyToCharacterHandler[selectedCharacter];
    }

    public static bool Transaction(int cost)
    {
        PlayerManager.setSouls();
        if (PlayerManager.getSouls() < cost) return false;
        PlayerManager.spendSouls(cost);
        TitleScreen.UpdateSoulsDisplay();
        return true;
    }
}