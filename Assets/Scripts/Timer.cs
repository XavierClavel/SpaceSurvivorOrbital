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

    public Slider timerSlider;
    
    public float totalTime = 60.0f; // Temps total en secondes
    private float elapsedTime = 0.0f; // Temps écoulé en secondes
   
    private bool bossHere = false;

    [SerializeField] Ennemy boss;
    
    Transform playerTransform;
    
    public static Timer instance;
    
    public GameObject winText;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        playerTransform = PlayerController.instance.transform;
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        timerSlider.value = 1 - (elapsedTime / totalTime);

        if (elapsedTime >= totalTime  && bossHere == false)
        {
            Instantiate(boss, randomPos() + playerTransform.position, Quaternion.identity).onDeath.AddListener(OnBossDefeat);
            bossHere = true;
        }
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
