using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class PlanetSelector : MonoBehaviour
{
    public bool generateRandomPlanets = true;
    public static PlanetSelector instance;
    [SerializeField] GameObject firstSelected;
    [Header("Sprites")]
    public Sprite planetMushroom;
    public Sprite planetIce;
    public Sprite planetJungle;
    public Sprite planetDesert;
    public Sprite planetStorm;

    [Header("Background Colors")]

    public Color colorMushroom;
    public Color colorIce;
    public Color colorJungle;
    public Color colorDesert;
    public Color colorStorm;

    [Header("Parameters")]
    [SerializeField] float resourceMultiplier = 0.33f;
    [SerializeField] float altarMultiplier = 0.5f;
    [SerializeField] float sizeMultiplier = 0.1f;
    [SerializeField] float randomMultiplier = 0.5f;
    [SerializeField] float globalDifficultyMultiplier = 0.8f;
    static int globalDifficulty = -1;

    private void Awake()
    {
        instance = this;
        globalDifficulty++;
        InputManager.setSelectedObject(firstSelected);
    }

    public static void SelectPlanet(Planet planet)
    {
        PlanetManager.setData(planet.planetData);
        SceneManager.LoadScene(Vault.scene.Planet);
    }

    public static int getDifficulty(PlanetData planetData)
    {
        int resourceAmount = (int)planetData.violetScarcity + (int)planetData.orangeScarcity + (int)planetData.greenScarcity;
        float resourceFactor = instance.resourceMultiplier * (float)resourceAmount;
        float altarFactor = instance.altarMultiplier * (planetData.hasAltar ? 1 : 0);
        float sizeFactor = instance.sizeMultiplier * ((float)planetData.size - 1f);
        float randomFactor = instance.randomMultiplier * Random.Range(-1f, 1f);
        float globalFactor = instance.globalDifficultyMultiplier * globalDifficulty;
        float difficultyValue = resourceFactor + altarFactor + randomFactor + globalFactor - sizeFactor;
        return (int)difficultyValue;
    }

}
