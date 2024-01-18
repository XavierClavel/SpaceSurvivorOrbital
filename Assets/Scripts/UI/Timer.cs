using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using DG.Tweening;
using Shapes;

public class Timer : MonoBehaviour
{
    [SerializeField] Ennemy boss;
    
    public static int timeRemainingToAdd;
    private float timeRemaining;

    private float elapsedTime = 0.0f; // Temps écoulé en secondes


    [SerializeField] private Image timerDisplay;
    [SerializeField] private Transform skullPivotTransform;
    [SerializeField] private Transform skullTransform;
    
    
    public GameObject winText;
    private float factor;

    private float startAngle = 0f;
    private float endAngle = -65f;


    

    void Start()
    {
        factor = 1f / timeRemaining;
        if (!DebugManager.doNoTimer()) StartCoroutine(nameof(TimerRunner));
        float angle = startAngle + (endAngle - startAngle) * factor;
    }
    
    private IEnumerator TimerRunner()
    {
        while (true)
        {
            timeRemaining = ConstantsData.timerDuration;
            while (timeRemaining > 0)
            {
                timeRemaining -= Time.fixedDeltaTime;
                //timerSlider.value = timeRemaining * factor;
                //disc.AngRadiansEnd = endAngle + (startAngle - endAngle) * timeRemaining * factor;
                timerDisplay.fillAmount = 1 - timeRemaining * factor;
                //skullPivotTransform.eulerAngles = endAngle * (1 - timeRemaining * factor) * Vector3.forward;
                //skullTransform.eulerAngles = Vector3.zero;
                yield return Helpers.GetWait(Time.fixedDeltaTime);
            }
            PlanetSelector.IncreaseDifficulty(true);
        }
        //Vector3 bossPosition = PlayerController.instance.transform.position + Helpers.getRandomPositionInRadius(new Vector2(4f, 8f));
        //Instantiate(boss, bossPosition, Quaternion.identity).onDeath.AddListener(OnBossDefeat);
    }


    private void OnBossDefeat()
    {
        PauseMenu.instance.PauseGame(false);
        Instantiate(winText);
        //InputManager.setSelectedObject(firstSelected);
    }

}
