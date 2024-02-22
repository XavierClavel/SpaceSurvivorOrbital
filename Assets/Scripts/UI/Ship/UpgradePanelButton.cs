using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class UpgradePanelButton : MonoBehaviour, ISelectHandler
{
    private static UpgradePanelButton currentSelected = null;
    public Button button;

    [SerializeField] private Image image;
    [SerializeField] private GameObject parent;
    [SerializeField] private GameObject newDisplay;
    [SerializeField] private StringLocalizer titleSkill;
    [SerializeField] private Shapes.Rectangle border;
    [SerializeField] private Shapes.Rectangle fill;
    [SerializeField] private Color32 fillColorSelected;
    [SerializeField] private Color32 borderColorUnselected;

    private void OnDestroy()
    {
        currentSelected = null;
    }

    private void Start()
    {
        button.onClick.AddListener(delegate { newDisplay.SetActive(false); });
    }

    public void Disable()
    {
        parent.SetActive(false);
    }
    public UpgradePanelButton setSprite(Sprite sprite)
    {
        image.sprite = sprite;
        return this;
    }

    public UpgradePanelButton setAction(UnityAction action)
    {
        button.onClick.AddListener(action);
        return this;
    }

    public UpgradePanelButton setTextKey(string text)
    {
        titleSkill.setKey(text);
        return this;
    }

    public void flagNew()
    {
        newDisplay.SetActive(true);
    }

    public void OnSelect(BaseEventData eventData)
    {
        /*
        var gradientFill = border.Fill;
        gradientFill.colorStart = fillColorSelected;
        gradientFill.colorEnd = fillColorSelected;
        border.Fill = gradientFill;
        currentSelected = this;
        */
    }

}
