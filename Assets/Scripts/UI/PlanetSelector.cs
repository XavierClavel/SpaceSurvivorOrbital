using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlanetSelector : MonoBehaviour
{
    public void SelectPlanet(Planet planet)
    {
        PlanetManager.setData(planet.planetData);
        SceneManager.LoadScene("Planet");
    }

}
