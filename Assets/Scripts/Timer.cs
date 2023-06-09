using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Timer : MonoBehaviour
{
    public static int timeRemaining = 120;
    [SerializeField] TextMeshProUGUI timeText; // r�f�rence au composant Text de l'UI
    [SerializeField] float timeToAdd = 5; // temps � ajouter lorsqu'une touche est enfonc�e
    int time;

    [SerializeField] GameObject boss;
    Transform playerTransform;
    GameObject spaceShip;
    WaitForSeconds waitSecond;
    public static Timer instance;
    bool doTimerRun = true;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        timeRemaining = PlayerManager.currentTimer += 120;
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
            timeText.text = "Temps restant : " + Mathf.RoundToInt(timeRemaining).ToString(); // affiche le temps restant dans l'UI
            yield return waitSecond;
            timeRemaining--;
        }
        Instantiate(boss, randomPos() + playerTransform.position, Quaternion.identity);
        Destroy(spaceShip);
    }

    Vector3 randomPos()
    {
        float signA = Random.Range(0, 2) * 2 - 1;
        float signB = Random.Range(0, 2) * 2 - 1;
        return signA * Random.Range(6f, 12f) * Vector2.up + signB * Random.Range(4f, 8f) * Vector2.right;
    }

}
