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
    public static int timeRemaining;
    public int setTime;
    [SerializeField] TextMeshProUGUI timeText; // r�f�rence au composant Text de l'UI
    int time;

    public Image timerBar; // Référence à l'image de la jauge de temps
    public Slider timerSlider;

    [SerializeField] Ennemy boss;
    Transform playerTransform;
    GameObject spaceShip;
    WaitForSeconds waitSecond;
    public static Timer instance;
    bool doTimerRun = true;
    public GameObject winText;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        timeRemaining = PlayerManager.currentTimer = setTime;
        playerTransform = PlayerController.instance.transform;
        spaceShip = GameObject.FindGameObjectWithTag("Ship");
        waitSecond = Helpers.GetWait(1f);
        if (doTimerRun) StartCoroutine(nameof(RunTimer));
    }

    public void debug_StopTimer()
    {
        doTimerRun = false;
        StopCoroutine(nameof(RunTimer));
    }

    IEnumerator RunTimer()
    {
        while (timeRemaining > 0)
        {
            float fillAmount = timeRemaining;
            timerSlider.value = fillAmount - 1;
            timeText.text = "Temps restant : " + Mathf.RoundToInt(timeRemaining).ToString(); // affiche le temps restant dans l'UI
            yield return waitSecond;
            timeRemaining--;
            if (timeRemaining == 30) TimeIsRunningOutDisplay();
        }
        //Destroy(spaceShip);
        Instantiate(boss, randomPos() + playerTransform.position, Quaternion.identity).onDeath.AddListener(OnBossDefeat);
    }

    void TimeIsRunningOutDisplay()
    {
        timeText.transform.DOScale(1.5f, 2f);
        timeText.DOColor(Color.red, 2f);
        timeText.fontStyle = FontStyles.Bold;
    }

    Vector3 randomPos()
    {
        float signA = Random.Range(0, 2) * 2 - 1;
        float signB = Random.Range(0, 2) * 2 - 1;
        return signA * Random.Range(6f, 12f) * Vector2.up + signB * Random.Range(4f, 8f) * Vector2.right;
    }

    public void OnBossDefeat()
    {
        PauseMenu.instance.PauseGame(false);
        Instantiate(winText);
        //InputManager.setSelectedObject(firstSelected);
    }

}
