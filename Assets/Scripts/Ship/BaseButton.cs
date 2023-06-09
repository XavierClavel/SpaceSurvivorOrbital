using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class TreeButton : MonoBehaviour
{

    protected List<string> activateButton = new List<string>();
    protected List<string> desactivateButton = new List<string>();
    public string upgradeName;
    [SerializeField] protected TextMeshProUGUI titleText;
    [SerializeField] protected TextMeshProUGUI descriptionText;
    protected List<Effect> effects = new List<Effect>();


    public skillButtonStatus status { get; private set; }
    [HideInInspector] public Button button;
    protected Image image;
    protected delegate void buttonAction();
    public bool isFirst;

    protected virtual void Awake()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();

        upgradeName = upgradeName.Trim();
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

        SkillTree.UpdateList(activateButton, skillButtonStatus.unlocked);
        SkillTree.UpdateList(desactivateButton, skillButtonStatus.locked);
        SkillTree.UpdateButton(this, skillButtonStatus.bought);

        action();
    }

    protected abstract bool SpendResources();

    public void UpdateStatus(skillButtonStatus status)
    {
        this.status = status;
        switch (status)
        {
            case skillButtonStatus.bought:
                image.color = Color.green;
                break;

            case skillButtonStatus.unlocked:
                image.color = Color.white;
                break;

            case skillButtonStatus.locked:
                image.color = Color.gray;
                break;
        }
    }
}
