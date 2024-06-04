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
    public Sprite planetSwamp;
    public Sprite planetDesert;
    public Sprite planetStorm;
    public Sprite shop;
    public Sprite shopArtefact;

    private void Awake()
    {
        instance = this;
        PlayerManager.increaseDifficulty();
        InputManager.setSelectedObject(firstSelected);
    }


    public static void SelectPlanet(Planet planet)
    {
        PlanetManager.setData(planet.planetData);
        SceneTransitionManager.TransitionToScene(planet.planetData);
    }
    
    public static void SelectFirstPlanet()
    {
        PlanetData planetData = PlanetSelectionManager.getStartPlanetData();
        PlanetManager.setData(planetData);
        SceneTransitionManager.TransitionToScene(planetData);
    }

    public static int getDifficulty()
    {
        if (DebugManager.instance == null)
        {
            return PlayerManager.getDifficulty();
        }
        return DebugManager.doOverrideDifficulty() ? DebugManager.getDebugDifficulty() : PlayerManager.getDifficulty();
    }

}
