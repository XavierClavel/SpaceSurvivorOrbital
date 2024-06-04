using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerSlider : MonoBehaviour, IDifficultyListener
{
    public Slider timerSlider; // Associez ceci avec le Slider dans l'inspecteur
    public float timerDuration = 10f; // Durée du timer en secondes
    public TextMeshProUGUI difficultyText;

    private float timer;
    private bool isTimerRunning = false;

    void Start()
    {
        timerDuration = ConstantsData.timerDuration;

        if (timerSlider != null)
        {
            timerSlider.maxValue = timerDuration;
            resetTimer();
            difficultyText.text = PlayerManager.getDifficulty().ToString();
            EventManagers.difficulty.registerListener(this);
            isTimerRunning = true;
        }
        else
        {
            Debug.LogError("Slider non assigné dans l'inspecteur.");
        }
    }

    private void OnDestroy()
    {
        EventManagers.difficulty.unregisterListener(this);
    }

    void Update()
    {
        if (!isTimerRunning) return;
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            timerSlider.value = timer;
        }
        else
        {
            isTimerRunning = false;
            onTimerEnd();
            resetTimer(); // Réinitialiser le timer après affichage du message
        }
    }

    private void onTimerEnd()
    {
        PlayerManager.increaseDifficulty();
    }

    private void resetTimer()
    {
        timer = timerDuration;
        timerSlider.value = timerDuration;
        isTimerRunning = true;
    }

    public void onDifficultyChange(int value)
    {
        difficultyText.text = value.ToString();
    }
}
