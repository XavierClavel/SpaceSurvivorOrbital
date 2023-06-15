using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StringLocalizer : MonoBehaviour
{
    TextMeshProUGUI textDisplay;
    [SerializeField] LocalizedString localizedString;


    private void Awake()
    {
        textDisplay = GetComponent<TextMeshProUGUI>();
        LocalizationManager.dictDisplayToLocalizedString.Add(textDisplay, localizedString);
    }






}
