using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public abstract class TreeButton : MonoBehaviour, IPointerEnterHandler, ISelectHandler
{

    protected List<string> activateButton = new List<string>();
    protected List<string> desactivateButton = new List<string>();
    [HideInInspector] public string key;
    [SerializeField] protected TextMeshProUGUI titleText;
    [SerializeField] protected TextMeshProUGUI descriptionText;
    protected List<Effect> effects = new List<Effect>();


    public skillButtonStatus status { get; private set; }
    [HideInInspector] public Button button;
    protected Image image;
    protected delegate void buttonAction();
    protected UpgradeData upgradeData;
    ButtonSprite buttonSprite;


    public virtual void Initialize(string key)
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();

        this.key = key;

        titleText.SetText(key);
        LocalizationManager.LocalizeTextField(key + Vault.key.ButtonTitle, titleText);
        LocalizationManager.LocalizeTextField(key + Vault.key.ButtonDescription, descriptionText);

        upgradeData = DataManager.dictUpgrades[key];

        effects = upgradeData.effects.Copy();

        activateButton = upgradeData.upgradesEnabled;
        desactivateButton = upgradeData.upgradesDisabled;
        desactivateButton.TryAdd(key);

        if (PanelSelector.dictKeyToButtonSprites.ContainsKey(upgradeData.spriteKey))
        {
            buttonSprite = PanelSelector.dictKeyToButtonSprites[upgradeData.spriteKey];
        }
        else Debug.LogWarning($"sprite key {upgradeData.spriteKey} is not associated with button sprites");
    }


    public void OnClick()
    {
        Execute(ApplyEffects);
    }

    protected void ApplyEffects()
    {
        foreach (Effect effect in effects)
        {
            effect.Apply();
        }
    }

    protected void Execute(buttonAction action)
    {
        if (status != skillButtonStatus.unlocked) return;

        if (!SpendResources()) return;
        ResourcesDisplay.UpdateDisplay();

        NodeManager.UpdateList(activateButton, skillButtonStatus.unlocked);
        NodeManager.UpdateList(desactivateButton, skillButtonStatus.locked);
        NodeManager.UpdateButton(this, skillButtonStatus.bought);

        action();
    }

    protected abstract bool SpendResources();

    public void UpdateStatus(skillButtonStatus status)
    {
        this.status = status;
        button.interactable = status == skillButtonStatus.unlocked;

        if (buttonSprite == null) return;
        switch (status)
        {
            case skillButtonStatus.bought:
                image.sprite = buttonSprite.purchased;
                break;

            case skillButtonStatus.unlocked:
                image.sprite = buttonSprite.available;
                break;

            case skillButtonStatus.locked:
                image.sprite = buttonSprite.locked;
                break;
        }
    }

    // When highlighted with mouse.
    public void OnPointerEnter(PointerEventData eventData)
    {
        UpgradeDisplay.DisplayUpgrade(key);
    }

    // When selected.
    public void OnSelect(BaseEventData eventData)
    {
        UpgradeDisplay.DisplayUpgrade(key);
        // Do something.
        // Debug.Log("<color=red>Event:</color> Completed selection.");
    }
}
