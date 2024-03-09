using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using System.Linq;


public enum planetSize { small, medium, large }
public enum planetResourceScarcity { rare, medium, common }

public enum planetType
{
    ice, 
    mushroom, 
    desert, 
    storm, 
    jungle, 
    swamp, 
    shop, 
    shopArtefact,
}

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
            planetType.swamp => gameScene.planetSwamp,
            planetType.desert => gameScene.planetDesert,
            planetType.mushroom => gameScene.planetMushroom,
            planetType.storm => gameScene.planetStorm,
            planetType.shop => gameScene.shop,
            planetType.shopArtefact => gameScene.shopArtefact,
            _ => throw new ArgumentOutOfRangeException($"Unexpected enum value")
        };
    }

    public static PlanetData getRandom(int tier)
    {
        PlanetData planetData = new PlanetData();
        planetData.setData(tier);
        return planetData;
    }

    private PlanetData setData(int tier)
    {
        this.type = Helpers.getRandomEnum<planetType>(
            planetType.storm, 
            planetType.shop, 
            planetType.shopArtefact);
        
        if (tier > 1 && Helpers.ProbabilisticBool(0.10f))
        {
            this.type = planetType.shop;
            this.size = planetSize.medium;
            return this;
        }
        if (tier > 1 && Helpers.ProbabilisticBool(0.07f))
        {
            this.type = planetType.shopArtefact;
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
            planetType.swamp => planetResourceScarcity.common,
            planetType.mushroom => planetResourceScarcity.common,
            planetType.storm => planetResourceScarcity.rare,
            _ => throw new ArgumentOutOfRangeException("Unexpected enum value")
        };
        

        return this;
    }
}
