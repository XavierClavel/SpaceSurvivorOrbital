using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Collectible : MonoBehaviour
{
    public TextMeshProUGUI greenText;
    public int greenAmount;

    public TextMeshProUGUI yellowText;
    public int yellowAmount;

    private void Update()
    {
        greenAmount = PlayerManager.amountGreen;
        greenText.text = greenAmount.ToString();

        yellowAmount = PlayerManager.amountOrange;
        yellowText.text = yellowAmount.ToString();
    }
}
