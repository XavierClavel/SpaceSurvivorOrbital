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

    private void Awake()
    {
        instance = this;
    }

    public void SelectPlanet(Planet planet)
    {
        PlanetManager.setData(planet.planetData);
        SceneManager.LoadScene("Planet");
    }

}
