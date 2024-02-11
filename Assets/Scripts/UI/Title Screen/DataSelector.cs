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
    [SerializeField] private StringLocalizer characterTitleDisplay;
    [SerializeField] private GameObject characterCostDisplay;
    [SerializeField] private TextMeshProUGUI characterCostText;
    [SerializeField] private StringLocalizer characterDescriptionText;
    [SerializeField] private Image characterImage;
    [SerializeField] private Button characterBuyButton;
    
    [Header("Weapon")]
    [SerializeField] private StringLocalizer weaponTitleDisplay;
    [SerializeField] private GameObject weaponCostDisplay;
    [SerializeField] private TextMeshProUGUI weaponCostText;
    [SerializeField] private StringLocalizer weaponDescriptionText;
    [SerializeField] private Image weaponImage;
    [SerializeField] private Button weaponBuyButton;

    public static HashSet<string> selectedEquipments = new HashSet<string>();

    [Header("Equipment")]
    [SerializeField] private StringLocalizer equipmentTitleDisplay;
    [SerializeField] private GameObject equipmentCostDisplay;
    [SerializeField] private TextMeshProUGUI equipmentCostText;
    [SerializeField] private StringLocalizer equipmentDescriptionText;
    [SerializeField] private Image equipmentImage;
    [SerializeField] private Button equipmentBuyButton;
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
    private bool playSound = false;


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
        characterTitleDisplay.setKey(selectButton.key + Vault.key.ButtonTitle);
        characterDescriptionText.setKey(selectButton.key + Vault.key.ButtonDescription);
        
        characterCostDisplay.SetActive(!selectButton.isUnlocked);
        characterBuyButton.gameObject.SetActive(!selectButton.isUnlocked);
        characterBuyButton.onClick.RemoveAllListeners();
        characterBuyButton.onClick.AddListener(selectButton.Buy);
        
        characterCostText.SetText(selectButton.cost.ToString());
        characterImage.sprite = getIcon(selectButton.key);
    }
    
    private void DisplayWeapon(SelectButton selectButton)
    {
        if (!weaponImage.gameObject.activeInHierarchy) weaponImage.gameObject.SetActive(true);
        weaponTitleDisplay.setKey(selectButton.key + Vault.key.ButtonTitle);
        weaponDescriptionText.setKey(selectButton.key + Vault.key.ButtonDescription);
        
        weaponCostDisplay.SetActive(!selectButton.isUnlocked);
        weaponBuyButton.gameObject.SetActive(!selectButton.isUnlocked);
        weaponBuyButton.onClick.RemoveAllListeners();
        weaponBuyButton.onClick.AddListener(selectButton.Buy);
        
        weaponCostText.SetText(selectButton.cost.ToString());
        weaponImage.sprite = getIcon(selectButton.key);
    }

    private void DisplayEquipment(SelectButton selectButton)
    {
        if (!equipmentImage.gameObject.activeInHierarchy) equipmentImage.gameObject.SetActive(true);
        equipmentTitleDisplay.setKey(selectButton.key + Vault.key.ButtonTitle);
        equipmentDescriptionText.setKey(selectButton.key + Vault.key.ButtonDescription);
        equipmentChargeDisplay.gameObject.SetActive(true);
        equipmentChargeDisplay.setCharge(ScriptableObjectManager.dictKeyToEquipmentHandler[selectButton.key].getCharge());
        
        equipmentCostDisplay.SetActive(!selectButton.isUnlocked);
        equipmentBuyButton.gameObject.SetActive(!selectButton.isUnlocked);
        equipmentBuyButton.onClick.RemoveAllListeners();
        equipmentBuyButton.onClick.AddListener(selectButton.Buy);

        equipmentCostText.SetText(selectButton.cost.ToString());
        equipmentImage.sprite = getIcon(selectButton.key);
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
        playSound = true;
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
        SelectGeneric(value.getKey());
    }

    public static void SelectGeneric(string value) 
    {
        if (instance.playSound) SoundManager.PlaySfx("Button_Switch");
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
        if (selectedWeapon != string.Empty) startButton.interactable = true;
    }

    public void SelectWeapon(string value)
    {
        if (selectedWeapon != null) dictKeyToButton[selectedWeapon].onDeselect();
        dictKeyToButton[value].onSelect();
        selectedWeapon = value;
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
    }

    public void ValidateSelection()
    {
        PlayerManager.setCharacter(ScriptableObjectManager.dictKeyToCharacterHandler[selectedCharacter]);
        PlayerManager.setWeapon(DataManager.dictWeapons[selectedWeapon].Clone(), ScriptableObjectManager.dictKeyToWeaponHandler[selectedWeapon]);
        foreach (var equipment in selectedEquipments)
        {
            PlayerManager.AcquireEquipment(equipment);
        }
    }

    public void Validate()
    {
        if (validated) return;
        validated = true;
        ValidateSelection();
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
        if (SaveManager.getSouls() < cost) return false;
        SaveManager.spendSouls(cost);
        TitleScreen.UpdateSoulsDisplay();
        return true;
    }
}