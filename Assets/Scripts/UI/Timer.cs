using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Timer : MonoBehaviour
{
    [SerializeField] Ennemy boss;
    
    public static int timeRemainingToAdd;
    private float timeRemaining;

    public Slider timerSlider;
    
    private float elapsedTime = 0.0f; // Temps écoulé en secondes
    
    
    public GameObject winText;
    private float factor;


    

    void Start()
    {
        timeRemaining = ConstantsData.timerDuration;
        factor = 1f / timeRemaining;
        if (!DebugManager.instance.noTimer) StartCoroutine(nameof(TimerRunner));
    }
    
    private IEnumerator TimerRunner()
    {
        while (timeRemaining > 0)
        {
            timeRemaining -= Time.fixedDeltaTime;
            timerSlider.value = timeRemaining * factor;
            yield return Helpers.GetWait(Time.fixedDeltaTime);
        }

        Vector3 bossPosition = PlayerController.instance.transform.position + Helpers.getRandomPositionInRadius(new Vector2(4f, 8f));
        Instantiate(boss, bossPosition, Quaternion.identity)
            .onDeath.AddListener(OnBossDefeat);
    }


    private void OnBossDefeat()
    {
        PauseMenu.instance.PauseGame(false);
        Instantiate(winText);
        //InputManager.setSelectedObject(firstSelected);
    }

}
