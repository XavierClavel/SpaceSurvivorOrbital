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
    [SerializeField] float globalDifficultyMultiplier = 0.8f;
    static int globalDifficulty = -1;

    private void Awake()
    {
        instance = this;
        globalDifficulty++;
    }

    public void SelectPlanet(Planet planet)
    {
        PlanetManager.setData(planet.planetData);
        SceneManager.LoadScene("Planet");
    }

    public static int getDifficulty(PlanetData planetData)
    {
        int resourceAmount = (int)planetData.violetScarcity + (int)planetData.orangeScarcity + (int)planetData.greenScarcity;
        float resourceFactor = instance.resourceMultiplier * (float)resourceAmount;
        float altarFactor = instance.altarMultiplier * (planetData.hasAltar ? 1 : 0);
        float randomFactor = instance.randomMultiplier * Random.Range(-1f, 1f);
        float globalFactor = instance.globalDifficultyMultiplier * globalDifficulty;
        float difficultyValue = resourceFactor + altarFactor + randomFactor + globalFactor;
        return (int)difficultyValue;
    }

}
