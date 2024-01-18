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
    //public Sprite planetStorm;

    [Header("Background Colors")]

    public Color colorMushroom;
    public Color colorIce;
    public Color colorJungle;
    public Color colorDesert;
    //public Color colorStorm;

    [Header("Parameters")]
    public static int globalDifficulty = 0;
    public ParticleSystem difficultyPS;

    public static void IncreaseDifficulty(bool display = false)
    {
        if (globalDifficulty >= DataManager.dictDifficulty.Keys.Count) return;
        globalDifficulty++;
        if (display)
        {
            Instantiate(instance.difficultyPS, PlayerController.instance.transform);
            SoundManager.PlaySfx(PlayerController.instance.transform, key: "Difficulty_Up");
        }
        Debug.Log($"Difficulty increased, now {globalDifficulty}");
    }

    public static void ResetDifficulty()
    {
        globalDifficulty = 0;
    }

    private void Awake()
    {
        instance = this;
        IncreaseDifficulty();
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
        return globalDifficulty;
    }

}
