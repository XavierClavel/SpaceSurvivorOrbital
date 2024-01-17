using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Planet : MonoBehaviour, IPointerEnterHandler, ISelectHandler
{
    //Consts
    private Vector2 randomizePositionFactor = new Vector2(100f,25f);
    
    //Static
    public static int currentTier = 0;
    private static GameObject currentlyDisplayedPanel = null;
    
    //API
    public PlanetData planetData;
    
    //Exposed
    public Image planet;
    //[SerializeField] DiscreteBarHandler sizeBar;
    //[SerializeField] DiscreteBarHandler dangerosityBar;
    [SerializeField] private Button button;
    [SerializeField] private DiscreteBarHandler violetBar;
    [SerializeField] private DiscreteBarHandler yellowBar;
    [SerializeField] private DiscreteBarHandler greenBar;
    [SerializeField] private GameObject displayPanel;

    private static Dictionary<String, Vector3> dictPlanetToPos = new Dictionary<string, Vector3>();
    
    //Private
    private int planetTier;
    public int tier;
    public int row;

#region staticAPI

    public static void Reset()
    {
        currentTier = 0;
        currentlyDisplayedPanel = null;
        dictPlanetToPos = new Dictionary<string, Vector3>();
    }

#endregion
    

    public void setup(Node node, Vector3 position)
    {
        this.tier = node.tier;
        this.row = node.row;
        if (tier != currentTier ||  !PlanetSelectionManager.getPossiblePathNodes().Contains(node))
        {
            button.interactable = false;
            planet.color = node == PlanetSelectionManager.GetSelectedNode() ? 
                new Color(38, 29, 4) : Color.grey;
        }

        if (PlanetSelectionManager.dictKeyToPlanetData.TryGetValue(node.key, out var value))
        {
            planetData = value;
        }
        else
        {
            Debug.LogError($"Key {node.key} is missing from {nameof(PlanetSelectionManager.dictKeyToPlanetData)}");
        }


        button.onClick.AddListener(delegate
        {
            currentTier++;
            PlanetSelectionManager.SelectNode(node);
            PlanetSelector.SelectPlanet(this);
            SoundManager.PlaySfx(transform, key: "Ship_Landing");
        });

        planet.sprite = getSprite();
        if (tier != 0 && row != 3)
        {
            if (dictPlanetToPos.TryGetValue(node.key, out var pos))
            {
                planet.transform.position += pos;
            }
            else
            {
                Vector3 pos2 = Helpers.getRandomPositionInRadius(randomizePositionFactor);
                planet.transform.position += pos2;
                dictPlanetToPos[node.key] = pos2;
            }
            
        }
        
    }


    Sprite getSprite()
    {
        return planetData.type switch
        {
            planetType.mushroom => PlanetSelector.instance.planetMushroom,
            //planetType.storm => PlanetSelector.instance.planetStorm,
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
            //planetType.storm => PlanetSelector.instance.colorStorm,
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
