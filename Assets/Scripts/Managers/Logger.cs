using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Logger : MonoBehaviour
{
    static TextMeshProUGUI logger;
    static string tempLogString = "";

    private void Awake()
    {
        logger = GetComponent<TextMeshProUGUI>();
        if (tempLogString != "")
        {
            logger.text = tempLogString;
            tempLogString = "";
        }
    }

    public static void Log(string s)
    {
        if (logger == null)
        {
            tempLogString += (s + "\n");
            return;
        }
        logger.text += (s + "\n");
    }
}
