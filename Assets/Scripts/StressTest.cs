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
        controls.Player.Jump.started += ctx => StartCoroutine("SpawnEnnemies");
        controls.Player.Jump.canceled += ctx => StopCoroutine("SpawnEnnemies");
        if (StressTestEnabled) controls.Enable();
        //Helpers.CreateDebugDisplay();
    }

    IEnumerator SpawnEnnemies()
    {
        WaitForSeconds wait = Helpers.GetWait(0.2f);
        while (true)
        {
            Planet.instance.SpawnEnnemy();
            //Helpers.DebugDisplay((nbEnnemies.ToString()));
            Debug.Log(nbEnnemies.ToString());
            yield return wait;
        }
    }
}
