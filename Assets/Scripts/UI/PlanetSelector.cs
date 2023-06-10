using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlanetSelector : MonoBehaviour
{
    public bool generateRandomPlanets = true;
    public static PlanetSelector instance;
    public Sprite planetBlue;
    public Sprite planetBrown;
    public Sprite planetRed;

    [Header("Parameters")]
    [SerializeField] float resourceMultiplier = 0.33f;
    [SerializeField] float altarMultiplier = 0.5f;
    [SerializeField] float randomMultiplier = 0.5f;

    private void Awake()
    {
        instance = this;
    }

    public void SelectPlanet(Planet planet)
    {
        PlanetManager.setData(planet.planetData);
        SceneManager.LoadScene("Planet");
    }

    public static planetDangerosity getDangerosity(PlanetData planetData)
    {
        int resourceAmount = (int)planetData.violetScarcity + (int)planetData.orangeScarcity + (int)planetData.greenScarcity;
        float resourceFactor = instance.resourceMultiplier * (float)resourceAmount;
        float altarFactor = instance.altarMultiplier * (planetData.hasAltar ? 1 : 0);
        float randomFactor = instance.randomMultiplier * Random.Range(-1f, 1f);
        float dangerosityValue = resourceFactor + altarFactor + randomFactor;
        Debug.Log(dangerosityValue);
        if (dangerosityValue <= 1) return planetDangerosity.peaceful;
        else if (dangerosityValue > 2) return planetDangerosity.hard;
        else return planetDangerosity.medium;
    }

}
