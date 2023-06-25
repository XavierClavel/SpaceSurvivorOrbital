using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StringLocalizer : MonoBehaviour
{
    [SerializeField] string key;

    private void Awake()
    {
        LocalizationManager.stringLocalizers.Add(this);
    }


    public void Initialize()
    {
        if (!DataManager.dictLocalization.ContainsKey(key))
        {
            throw new System.ArgumentException($"{gameObject.name} is trying to call the \"{key}\" key which does not exist.");
        }
        LocalizedString localizedString = DataManager.dictLocalization[key];
        LocalizationManager.dictDisplayToLocalizedString.Add(GetComponent<TextMeshProUGUI>(), localizedString);

        Destroy(this);
    }






}
