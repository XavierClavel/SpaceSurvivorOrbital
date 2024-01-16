using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public enum planetSize { small, medium, large }
public enum planetResourceScarcity { rare, medium, common }
public enum planetType { ice, mushroom, desert, storm, jungle }

[System.Serializable]
public class PlanetData
{
    public planetSize size;
    public int difficulty;
    public planetResourceScarcity denScarcity;
    public planetResourceScarcity violetScarcity;
    public planetResourceScarcity ressourceScarcity;
    public planetType type;

    public gameScene getScene()
    {
        return type switch
        {
            planetType.ice => gameScene.planetIce,
            planetType.jungle => gameScene.planetJungle,
            planetType.desert => gameScene.planetDesert,
            planetType.mushroom => gameScene.planetMushroom,
            planetType.storm => gameScene.planetStorm,
            _ => gameScene.titleScreen
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
        this.size = Helpers.getRandomEnum<planetSize>();
        this.ressourceScarcity = Helpers.getRandomEnum<planetResourceScarcity>();
        this.violetScarcity = Helpers.getRandomEnum<planetResourceScarcity>();
        this.type = Helpers.getRandomEnum<planetType>();

        this.difficulty = PlanetSelector.getDifficulty();
        

        return this;
    }
}
