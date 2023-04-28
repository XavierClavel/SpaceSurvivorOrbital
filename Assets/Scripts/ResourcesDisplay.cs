using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResourcesDisplay : MonoBehaviour
{
    public TextMeshProUGUI greenText;

    public TextMeshProUGUI yellowText;
    static ResourcesDisplay instance;

    private void Start()
    {
        instance = this;
        UpdateDisplay();
    }

    public static void UpdateDisplay()
    {
        instance.greenText.text = PlayerManager.amountGreen.ToString();
        instance.yellowText.text = PlayerManager.amountOrange.ToString();
    }

}
