using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Planet : MonoBehaviour, IPointerEnterHandler, ISelectHandler
{
    //Consts
    private const float randomizePositionFactor = 25f;
    
    //Static
    private static int currentTier = 0;
    private static GameObject currentlyDisplayedPanel = null;
    
    //API
    public PlanetData planetData;
    
    //Exposed
    public Image planet;
    //[SerializeField] DiscreteBarHandler sizeBar;
    //[SerializeField] DiscreteBarHandler dangerosityBar;
    [SerializeField] private Button button;
    [SerializeField] private DiscreteBarHandler violetBar;
    [SerializeField] private DiscreteBarHandler orangeBar;
    [SerializeField] private DiscreteBarHandler greenBar;
    [SerializeField] private GameObject displayPanel;
    
    //Private
    private int planetTier;
    public int tier;
    public int row;

#region staticAPI

    public static void Reset()
    {
        currentTier = 0;
        currentlyDisplayedPanel = null;
    }

#endregion
    

    public void setup(Node node)
    {
        this.tier = node.tier;
        this.row = node.row;
        if (tier != currentTier ||  !PlanetSelectionManager.getPossiblePathNodes().Contains(node))
        {
            button.interactable = false;
            if (node == PlanetSelectionManager.GetSelectedNode())
            {
                planet.color = new Color(38, 29, 4);
            }
            else planet.color = Color.grey;
        }

        if (PlanetSelectionManager.dictKeyToPlanetData.TryGetValue(node.key, out var value))
        {
            planetData = value;
        }
        else
        {
            GenerateData();
            PlanetSelectionManager.dictKeyToPlanetData[node.key] = planetData;
        }


        button.onClick.AddListener(delegate
        {
            currentTier++;
            PlanetSelector.SelectPlanet(this);
            PlanetSelectionManager.SelectNode(node);
        });

        planet.sprite = getSprite();
        planet.transform.position += Helpers.getRandomPositionInRadius(randomizePositionFactor);
        //GetComponent<Image>().color = getColor();

        /*
         sizeBar.maxAmount = 3;
        sizeBar.currentAmount = (int)planetData.size + 1;
        sizeBar.Initialize();

        dangerosityBar.maxAmount = 10;
        dangerosityBar.currentAmount = planetData.difficulty + 1;
        dangerosityBar.Initialize();
        */
        
        violetBar.maxAmount = 5;
        violetBar.currentAmount = (int)planetData.violetScarcity + 1;
        violetBar.Initialize();

        orangeBar.maxAmount = 3;
        orangeBar.currentAmount = (int)planetData.orangeScarcity;
        orangeBar.Initialize();

        greenBar.maxAmount = 3;
        greenBar.currentAmount = (int)planetData.greenScarcity;
        greenBar.Initialize();
    }

    private void GenerateData()
    {
            planetData.size = Helpers.getRandomEnum<planetSize>();
            planetData.violetScarcity = Helpers.getRandomEnum<planetResourceScarcity>();
            planetData.orangeScarcity = Helpers.getRandomEnum<planetResourceScarcity>();
            planetData.greenScarcity = Helpers.getRandomEnum<planetResourceScarcity>();
            planetData.type = Helpers.getRandomEnum<planetType>();
            planetData.hasAltar = true;

            planetData.difficulty = PlanetSelector.getDifficulty(planetData);
    }


    Sprite getSprite()
    {
        return planetData.type switch
        {
            planetType.mushroom => PlanetSelector.instance.planetMushroom,
            planetType.storm => PlanetSelector.instance.planetStorm,
            planetType.ice => PlanetSelector.instance.planetIce,
            planetType.jungle => PlanetSelector.instance.planetJungle,
            planetType.desert => PlanetSelector.instance.planetDesert,
            _ => PlanetSelector.instance.planetMushroom
        };
    }

    Color getColor()
    {
        return planetData.type switch
        {
            planetType.ice => PlanetSelector.instance.colorIce,
            planetType.mushroom => PlanetSelector.instance.colorMushroom,
            planetType.desert => PlanetSelector.instance.colorDesert,
            planetType.jungle => PlanetSelector.instance.colorJungle,
            planetType.storm => PlanetSelector.instance.colorStorm,
            _ => PlanetSelector.instance.colorMushroom
        };
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        //UpgradeDisplay.DisplayUpgrade(key);
    }

    // When selected.
    public void OnSelect(BaseEventData eventData)
    {
        if (currentlyDisplayedPanel != null)
        {
            currentlyDisplayedPanel.SetActive(false);
        }

        currentlyDisplayedPanel = displayPanel;
        currentlyDisplayedPanel.SetActive(true);

        //UpgradeDisplay.DisplayUpgrade(key);
        //UpgradeDisplay.SetupBuyButton(delegate { Execute(ApplyEffects); }, gameObject);
        // Do something.
        // Debug.Log("<color=red>Event:</color> Completed selection.");
    }


}
