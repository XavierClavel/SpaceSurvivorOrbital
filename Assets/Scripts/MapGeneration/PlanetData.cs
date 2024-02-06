using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using System.Linq;


public enum planetSize { small, medium, large }
public enum planetResourceScarcity { rare, medium, common }
public enum planetType { ice, mushroom, desert, storm, jungle, shop }

[System.Serializable]
public class PlanetData
{
    public planetSize size;
    public int difficulty;
    public planetResourceScarcity ressourceScarcity;
    public planetType type;
    public bool isBoss = false;

    public static PlanetData Boss()
    {
        return new PlanetData()
        {
            size = planetSize.medium,
            difficulty = 0,
            ressourceScarcity =  planetResourceScarcity.rare,
            type = planetType.storm,
            isBoss = true,
        };
    }

    public gameScene getScene()
    {
        return type switch
        {
            planetType.ice => gameScene.planetIce,
            planetType.jungle => gameScene.planetJungle,
            planetType.desert => gameScene.planetDesert,
            planetType.mushroom => gameScene.planetMushroom,
            planetType.storm => gameScene.planetStorm,
            planetType.shop => gameScene.shop,
            _ => throw new ArgumentOutOfRangeException($"Unexpected enum value")
        };
    }

    public static PlanetData getRandom()
    {
        PlanetData planetData = new PlanetData();
        planetData.setData();
        return planetData;
    }

    private PlanetData setData()
    {
        this.type = Helpers.getRandomEnum<planetType>(planetType.storm);
        if (this.type == planetType.shop)
        {
            this.size = planetSize.medium;
            return this;
        }
        this.size = Helpers.getRandomEnum<planetSize>();
        this.difficulty = PlanetSelector.getDifficulty();

        this.ressourceScarcity = this.type switch
        {
            planetType.desert => planetResourceScarcity.medium,
            planetType.ice => planetResourceScarcity.medium,
            planetType.jungle => planetResourceScarcity.common,
            planetType.mushroom => planetResourceScarcity.common,
            planetType.storm => planetResourceScarcity.rare,
            _ => throw new ArgumentOutOfRangeException("Unexpected enum value")
        };
        

        return this;
    }
}
