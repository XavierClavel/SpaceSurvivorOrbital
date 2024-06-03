using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerSlider : MonoBehaviour
{
    public Slider timerSlider; // Associez ceci avec le Slider dans l'inspecteur
    public float timerDuration = 10f; // Durée du timer en secondes
    public TextMeshProUGUI difficultyText;
    public int difficulty;

    private float timer;
    private bool isTimerRunning = false;

    void Start()
    {
        timerDuration = ConstantsData.timerDuration;
        difficulty = PlanetSelector.getDifficulty();

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
        difficultyText.text = difficulty.ToString();

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
