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
    protected List<Effect> effects = new List<Effect>();


    public skillButtonStatus status { get; private set; }
    [HideInInspector] public Button button;
    public RectTransform rectTransform;
    protected Image image;
    protected delegate void buttonAction();
    protected UpgradeData upgradeData;
    ButtonSprite buttonSprite;


    public virtual void Initialize(string key)
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();

        this.key = key;

        upgradeData = DataManager.dictUpgrades[key];

        effects = upgradeData.effects.Copy();

        activateButton = upgradeData.upgradesEnabled;
        desactivateButton = upgradeData.upgradesDisabled;
        desactivateButton.TryAdd(key);

        if (ScriptableObjectManager.dictKeyToButtonSprites.ContainsKey(upgradeData.spriteKey))
        {
            buttonSprite = ScriptableObjectManager.dictKeyToButtonSprites[upgradeData.spriteKey];
        }
        else Debug.LogWarning($"sprite key {upgradeData.spriteKey} is not associated with button sprites");
    }


    public void OnClick()
    {
        SoundManager.PlaySfx(transform, key: "Button_Switch");
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
        if (!DebugManager.areUpgradesFree && !SpendResources()) return;
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
        //button.interactable = status == skillButtonStatus.unlocked;

        if (buttonSprite == null) return;
        switch (status)
        {
            case skillButtonStatus.bought:
                image.sprite = buttonSprite.purchased;
                SoundManager.PlaySfx(transform, key: "Button_Buy");
                HideCost();
                break;

            case skillButtonStatus.unlocked:
                image.sprite = buttonSprite.available;
                break;

            case skillButtonStatus.locked:
                image.sprite = buttonSprite.locked;
                break;
        }
    }

    protected abstract void HideCost();

    // When highlighted with mouse.
    public void OnPointerEnter(PointerEventData eventData)
    {
        //UpgradeDisplay.DisplayUpgrade(key);
    }

    // When selected.
    public void OnSelect(BaseEventData eventData)
    {
        UpgradeDisplay.DisplayUpgrade(key);
        UpgradeDisplay.SetupBuyButton(delegate { Execute(ApplyEffects); }, gameObject);
        // Do something.
        // Debug.Log("<color=red>Event:</color> Completed selection.");
    }
}
