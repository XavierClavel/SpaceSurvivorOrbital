using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StressTest : MonoBehaviour
{
    InputMaster controls;
    public static int nbEnnemies = 0;
    [SerializeField] bool StressTestEnabled = false;

    // Start is called before the first frame update
    void Start()
    {
        controls = new InputMaster();
        controls.Player.Jump.started += ctx => StartCoroutine(nameof(SpawnEnnemies));
        controls.Player.Jump.canceled += ctx => StopCoroutine(nameof(SpawnEnnemies));
        if (StressTestEnabled) controls.Enable();
        //Helpers.CreateDebugDisplay();
    }

    IEnumerator SpawnEnnemies()
    {
        WaitForSeconds wait = Helpers.getWait(0.2f);
        while (true)
        {
            //SpawnManager.instance.SpawnEnnemy();
            //Helpers.DebugDisplay((nbEnnemies.ToString()));
            Debug.Log(nbEnnemies.ToString());
            yield return wait;
        }
    }
}
