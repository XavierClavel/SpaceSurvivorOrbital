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
    private static float resourceMultiplier = 0.33f;
    private static float altarMultiplier = 0.5f;
    private static float sizeMultiplier = 0.1f;
    private static float randomMultiplier = 0.5f;
    private static float globalDifficultyMultiplier = 0.8f;
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
        SceneTransitionManager.TransitionToScene(planet.planetData);
    }

    public static int getDifficulty(PlanetData planetData)
    {
        int resourceAmount = (int)planetData.greenScarcity + (int)planetData.orangeScarcity + (int)planetData.greenScarcity;
        float resourceFactor = resourceMultiplier * (float)resourceAmount;
        float sizeFactor = sizeMultiplier * ((float)planetData.size - 1f);
        float randomFactor = randomMultiplier * Random.Range(-1f, 1f);
        float globalFactor = globalDifficultyMultiplier * globalDifficulty;
        float difficultyValue = resourceFactor + randomFactor + globalFactor - sizeFactor;
        return (int)difficultyValue + 1;
    }

}
