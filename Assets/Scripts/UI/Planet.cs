using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public PlanetData planetData;
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
            planetData.dangerosity = Helpers.getRandomEnum(planetDangerosity.peaceful);
            planetData.violetScarcity = Helpers.getRandomEnum(planetResourceScarcity.none);
            planetData.orangeScarcity = Helpers.getRandomEnum(planetResourceScarcity.none);
            planetData.greenScarcity = Helpers.getRandomEnum(planetResourceScarcity.none);
        }


        sizeBar.maxAmount = 3;
        sizeBar.currentAmount = (int)planetData.size + 1;
        sizeBar.Initialize();

        dangerosityBar.maxAmount = 5;
        dangerosityBar.currentAmount = (int)planetData.dangerosity + 1;
        dangerosityBar.Initialize();

        violetBar.maxAmount = 5;
        violetBar.currentAmount = (int)planetData.violetScarcity;
        violetBar.Initialize();

        orangeBar.maxAmount = 5;
        orangeBar.currentAmount = (int)planetData.orangeScarcity;
        orangeBar.Initialize();

        greenBar.maxAmount = 5;
        greenBar.currentAmount = (int)planetData.greenScarcity;
        greenBar.Initialize();
    }
}
