using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTree : MonoBehaviour
{
    public int greenRessource = PlayerManager.amountGreen;
    public int yellowRessource = PlayerManager.amountOrange;

    [Header("Life Tree")]
    public Button[] lifeList; 
    public string[] lifeTextList; 
    public Text[] greenLifeCostText;
    public Text[] yellowLifeCostText;

    public int[] greenLifeCost;
    public int[] yellowLifeCost;

    void Start()
    {
        lifeSkill();
    }

    void lifeSkill()
    {
        for (int i = 0; i < greenLifeCost.Length; i++)
        {
            greenLifeCostText[i].GetComponentInChildren<Text>().text = greenLifeCost[i].ToString();
        }

        for (int i = 0; i < yellowLifeCost.Length; i++)
        {
            yellowLifeCostText[i].GetComponentInChildren<Text>().text = yellowLifeCost[i].ToString();
        }

        for (int i = 0; i < lifeList.Length; i++)
        {
            int index = i;

            lifeList[i].GetComponentInChildren<Text>().text = lifeTextList[i];

            lifeList[i].onClick.AddListener(() =>
            {
                Debug.Log("Le bouton " + lifeTextList[index] + " a été cliqué !");

                if (index == 0 && greenRessource >= greenLifeCost[0] && yellowRessource >= yellowLifeCost[0])
                {
                    greenRessource -= greenLifeCost[0];
                    yellowRessource -= yellowLifeCost[0];
                    lifeList[1].interactable = true;
                    lifeList[index].interactable = false;
                }

                if (index == 1 && greenRessource >= greenLifeCost[1] && yellowRessource >= yellowLifeCost[1])
                {
                    greenRessource -= greenLifeCost[0];
                    yellowRessource -= yellowLifeCost[0];
                    lifeList[2].interactable = true;
                    lifeList[index].interactable = false;
                }

                if (index == 2 && greenRessource >= greenLifeCost[2] && yellowRessource >= yellowLifeCost[2])
                {
                    greenRessource -= greenLifeCost[0];
                    yellowRessource -= yellowLifeCost[0];
                    lifeList[3].interactable = true;
                    lifeList[index].interactable = false;
                }

                if (index == 3 && greenRessource >= greenLifeCost[3] && yellowRessource >= yellowLifeCost[3])
                {
                    greenRessource -= greenLifeCost[0];
                    yellowRessource -= yellowLifeCost[0];
                    lifeList[0].interactable = true;
                    lifeList[index].interactable = false;
                }
            });
        }

        // Désactiver les boutons cibles au démarrage du script
        lifeList[1].interactable = false;
        lifeList[2].interactable = false;
        lifeList[3].interactable = false;
    }
    }
