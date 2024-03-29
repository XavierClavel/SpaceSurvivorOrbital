using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResourcesDisplay : MonoBehaviour
{
    public TextMeshProUGUI greenText;
    public TextMeshProUGUI yellowText;
    [SerializeField] TextMeshProUGUI upgradePointsAmountText;
    static ResourcesDisplay instance;

    private void Start()
    {
        instance = this;
        UpdateResourcesDisplay();
    }

    public static void UpdateResourcesDisplay()
    {
        instance.upgradePointsAmountText.text = PlayerManager.amountBlue.ToString();
        instance.greenText.text = PlayerManager.amountGreen.ToString();
        instance.yellowText.text = PlayerManager.amountOrange.ToString();
    }

}
