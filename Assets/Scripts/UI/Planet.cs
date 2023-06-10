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
            planetData.type = Helpers.getRandomEnum(planetType.blue);
            planetData.hasAltar = Helpers.ProbabilisticBool(0.3f);

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
            case planetType.blue:
                return PlanetSelector.instance.planetBlue;

            case planetType.brown:
                return PlanetSelector.instance.planetBrown;

            case planetType.red:
                return PlanetSelector.instance.planetRed;

            default:
                return PlanetSelector.instance.planetBlue;
        }
    }

    Color getColor()
    {
        switch (planetData.type)
        {
            case planetType.blue:
                return new Color32(86, 107, 210, 255);

            case planetType.red:
                return new Color32(196, 62, 62, 255);

            case planetType.brown:
                return new Color32(210, 148, 86, 255);

            default:
                return new Color32(92, 106, 147, 255); ;
        }
    }


}
