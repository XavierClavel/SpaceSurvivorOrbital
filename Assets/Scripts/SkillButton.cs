using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public enum effectType
{
    baseDamage, maxHealth
}

public enum operationType
{
    add, multiply, assignation
}

[Serializable]
public class Effect
{
    public operationType operation;
    public effectType effect;
    public int value;
    

    public void Apply()
    {
        PlayerManager.maxHealth = 100;

        switch (effect)
        {
            //case effectType.baseDamage:
              // ApplyOperation(ref PlayerManager.baseDamage , value);
               //break;

            case effectType.maxHealth:
                ApplyOperation(ref PlayerManager.maxHealth , value);
               break;
        }

        Debug.Log(PlayerManager.maxHealth);
    }

    public void ApplyOperation (ref int parameter, int value)
    {
        switch (operation)
        {
            case operationType.add:
                parameter += value;
                break;

            case operationType.multiply:
                parameter *= value;
                break;

            case operationType.assignation:
                parameter = value;
                break;
        }
    }
    public void ApplyOperation(ref float parameter, float value)
    {
        switch (operation)
        {
            case operationType.add:
                parameter += value;
                break;

            case operationType.multiply:
                parameter *= value;
                break;

            case operationType.assignation:
                parameter = value;
                break;
        }
    }
}

public class SkillButton : MonoBehaviour
{
    public static int greenRessource = PlayerManager.amountGreen;
    public static int yellowRessource = PlayerManager.amountOrange;

    public int greenLifeCost;
    public int yellowLifeCost;

    public List<Button> activateButton = new List<Button>();
    public List<Button> desactivateButton = new List<Button>();
    public List<Effect> effects = new List<Effect>();

    public TextMeshProUGUI greenCostText;
    public TextMeshProUGUI yellowCostText;

    public bool isFirst;

    private void Start()
    {
        greenCostText.text = greenLifeCost.ToString();
        yellowCostText.text = yellowLifeCost.ToString();

        if (!isFirst)
        {
            GetComponent<Button>().interactable = false;
        }
    }

    public void OnClick()
    {
        if (greenRessource < greenLifeCost || yellowRessource < yellowLifeCost)
        {
            return;  
        }

        greenRessource -= greenLifeCost;
        yellowRessource -= yellowLifeCost;

        foreach (Button button in activateButton)
        {
            button.interactable = true;
        }

        foreach (Button button in desactivateButton)
        {
            button.interactable = false;
        }

        foreach (Effect effect in effects)
        {
            effect.Apply();
        }
    }
}
