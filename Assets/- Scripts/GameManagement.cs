using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagement : MonoBehaviour
{

    [HideInInspector] public static int nbPlanetsWEnnemies;
    [SerializeField] GameObject crosshair;
    [SerializeField] Material crosshairGlow;
    public static GameManagement instance;
    int currentLevel;
    [SerializeField] Image crosshairImage;

    private void Awake() {
        instance = this;
    }
    
    private void OnTriggerEnter(Collider other) {
        if (nbPlanetsWEnnemies == 0) {
            Debug.Log("Level ended");
            currentLevel = int.Parse(SceneManager.GetActiveScene().name.Split(' ')[1]);
            int nextLevel = currentLevel ++;
            SceneManager.LoadScene("Bossfight");
            try {
                PlayerPrefs.SetInt("levelReached", nextLevel);
                SceneManager.LoadScene("Level " + nextLevel);
            }
            catch {
                Debug.Log("No next level in build settings");
            }
        }
        else {
            Debug.Log("nope");
            Debug.Log(nbPlanetsWEnnemies);
        }
    }

    public void HideCrosshair()
    {
        crosshair.SetActive(false);
    }

    public void ShowCrosshair()
    {
        crosshair.SetActive(true);
    }

    public void GlowCrosshair()
    {
        crosshairImage.material = crosshairGlow;
    }

    public void NormalCrosshair()
    {
        crosshairImage.material = null;
    }

}
