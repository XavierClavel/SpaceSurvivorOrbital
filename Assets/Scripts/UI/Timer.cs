using UnityEngine;
using UnityEngine.UI;

public class TimerSlider : MonoBehaviour
{
    public Slider timerSlider; // Associez ceci avec le Slider dans l'inspecteur
    public float timerDuration = 10f; // Durée du timer en secondes

    private float timer;
    private bool isTimerRunning = false;

    void Start()
    {
        if (timerSlider != null)
        {
            timerSlider.maxValue = timerDuration;
            ResetTimer();
            isTimerRunning = true;
        }
        else
        {
            Debug.LogError("Slider non assigné dans l'inspecteur.");
        }
    }

    void Update()
    {
        if (isTimerRunning)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                timerSlider.value = timer;
            }
            else
            {
                isTimerRunning = false;
                Invoke(nameof(ResetTimer), 0); // Réinitialiser le timer après affichage du message
            }
        }
    }

    private void ResetTimer()
    {
        timer = timerDuration;
        timerSlider.value = timerDuration;
        isTimerRunning = true;
    }
}
