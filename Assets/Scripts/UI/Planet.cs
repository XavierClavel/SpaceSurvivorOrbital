using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Planet : MonoBehaviour
{
    public PlanetData planetData;
    [SerializeField] Image planet;
    [SerializeField] DiscreteBarHandler sizeBar;
    [SerializeField] DiscreteBarHandler dangerosityBar;
    [SerializeField] DiscreteBarHandler violetBar;
    [SerializeField] DiscreteBarHandler orangeBar;
    [SerializeField] DiscreteBarHandler greenBar;



    private void Start()
    {
        if (PlanetSelector.instance.generateRandomPlanets)
        {
            planetData.size = Helpers.getRandomEnum(planetSize.large);
            planetData.violetScarcity = Helpers.getRandomEnum(planetResourceScarcity.none);
            planetData.orangeScarcity = Helpers.getRandomEnum(planetResourceScarcity.none);
            planetData.greenScarcity = Helpers.getRandomEnum(planetResourceScarcity.none);
            planetData.type = Helpers.getRandomEnum(planetType.mushroom);
            planetData.hasAltar = true;

            planetData.difficulty = PlanetSelector.getDifficulty(planetData);

        }

        planet.sprite = getSprite();
        GetComponent<Image>().color = getColor();

        sizeBar.maxAmount = 3;
        sizeBar.currentAmount = (int)planetData.size + 1;
        sizeBar.Initialize();

        dangerosityBar.maxAmount = 10;
        dangerosityBar.currentAmount = planetData.difficulty + 1;
        dangerosityBar.Initialize();

        violetBar.maxAmount = 3;
        violetBar.currentAmount = (int)planetData.violetScarcity;
        violetBar.Initialize();

        orangeBar.maxAmount = 3;
        orangeBar.currentAmount = (int)planetData.orangeScarcity;
        orangeBar.Initialize();

        greenBar.maxAmount = 3;
        greenBar.currentAmount = (int)planetData.greenScarcity;
        greenBar.Initialize();

    }


    Sprite getSprite()
    {
        switch (planetData.type)
        {
            case planetType.mushroom:
                return PlanetSelector.instance.planetMushroom;

            case planetType.ice:
                return PlanetSelector.instance.planetMushroom;

            case planetType.jungle:
                return PlanetSelector.instance.planetJungle;

            case planetType.storm:
                return PlanetSelector.instance.planetStorm;

            case planetType.desert:
                return PlanetSelector.instance.planetDesert;

            default:
                return PlanetSelector.instance.planetMushroom;
        }
    }

    Color getColor()
    {
        switch (planetData.type)
        {
            case planetType.ice:
                return PlanetSelector.instance.colorIce;

            case planetType.mushroom:
                return PlanetSelector.instance.colorMushroom;

            case planetType.desert:
                return PlanetSelector.instance.colorDesert;

            case planetType.jungle:
                return PlanetSelector.instance.colorJungle;

            case planetType.storm:
                return PlanetSelector.instance.colorStorm;

            default:
                return PlanetSelector.instance.colorMushroom;
        }
    }


}
