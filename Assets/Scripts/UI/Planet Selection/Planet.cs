using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Planet : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    //Consts
    private Vector2 randomizePositionFactor = new Vector2(70f,10f);
    
    //Static
    public static int currentTier = 0;
    
    //API
    public PlanetData planetData;
    
    //Exposed
    public Image planet;
    //[SerializeField] DiscreteBarHandler sizeBar;
    //[SerializeField] DiscreteBarHandler dangerosityBar;
    [SerializeField] private Button button;
    [SerializeField] private GameObject displayPanel;

    private static Dictionary<String, Vector3> dictPlanetToPos = new Dictionary<string, Vector3>();
    
    //Private
    private int planetTier;
    public int tier;
    public int row;
    public Node node;

    private bool mouseOverride = false;

#region staticAPI

    

    public static void Reset()
    {
        currentTier = 0;
        dictPlanetToPos = new Dictionary<string, Vector3>();
    }

#endregion
    

    public void setup(Node node, Vector3 position)
    {
        this.tier = node.tier;
        this.row = node.row;
        this.node = node;
        Debug.Log($"tier : {tier}, row: {row}, currentTier: {currentTier}, inPathNodes: {PlanetSelectionManager.getPossiblePathNodes().Contains(node)}");
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

        /*
        button.onClick.AddListener(delegate
        {
            PlanetSelectionManager.SelectPlanet(this, node);
        });
        */

        planet.sprite = getSprite();
        
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


    Sprite getSprite()
    {
        return planetData.type switch
        {
            planetType.mushroom => PlanetSelector.instance.planetMushroom,
            planetType.storm => PlanetSelector.instance.planetStorm,
            planetType.ice => PlanetSelector.instance.planetIce,
            planetType.jungle => PlanetSelector.instance.planetJungle,
            planetType.swamp => PlanetSelector.instance.planetSwamp,
            planetType.desert => PlanetSelector.instance.planetDesert,
            planetType.shop => PlanetSelector.instance.shop,
            planetType.shopArtefact => PlanetSelector.instance.shopArtefact,
            _ => PlanetSelector.instance.planetMushroom,
        };
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOverride = true;
        PlanetSelectionManager.onSelect(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PlanetSelectionManager.onDeselect(this);
    }
    

    // When selected.
    public void OnSelect(BaseEventData eventData)
    {
        PlanetSelectionManager.onSelect(this);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (mouseOverride)
        {
            mouseOverride = false;
            return;
        }
        PlanetSelectionManager.onDeselect(this);
    }

    public void Select()
    {
        PlanetSelectionManager.SelectPlanet(this, node);
    }
    
    
}
