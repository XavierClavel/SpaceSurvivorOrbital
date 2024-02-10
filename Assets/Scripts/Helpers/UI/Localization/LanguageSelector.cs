using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Language
{
    public Sprite flag;
    public string name;
    public string key;
}

public class LanguageSelector : MonoBehaviour
{
    [SerializeField] private List<Language> languages;
    [SerializeField] private Image flagIcon;
    [SerializeField] private TextMeshProUGUI textDisplay;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button previousButton;
    private int currentIndex = 0;
    
    void Start()
    {
        nextButton.onClick.AddListener(Next);
        previousButton.onClick.AddListener(Previous);

        currentIndex = languages.FindIndex(it => it.key == LocalizationManager.getLanguage());
        if (currentIndex == -1)
        {
            Debug.LogError($"Language {LocalizationManager.getLanguage()} was not defined in LanguageSelector");
            return;
        }
        
        DisplayLanguage();
        UpdateButtons();
    }

    private void UpdateButtons()
    {
        if (currentIndex == languages.maxIndex()) nextButton.gameObject.SetActive(false);
        if (currentIndex == 0) previousButton.gameObject.SetActive(false);
    }

    private void DisplayLanguage(Language language)
    {
        flagIcon.sprite = language.flag;
        textDisplay.SetText(language.name);
    }

    private void DisplayLanguage(int index) => DisplayLanguage(languages[index]);
    private void DisplayLanguage() => DisplayLanguage(currentIndex);

    private void SelectLanguage()
    {
        LocalizationManager.setLanguage(languages[currentIndex].key);
        DisplayLanguage();
    }

    public void Next()
    {
        if (currentIndex >= languages.maxIndex()) return;
        if (currentIndex == 0) previousButton.gameObject.SetActive(true);
        currentIndex++;
        SelectLanguage();
        if (currentIndex == languages.maxIndex()) nextButton.gameObject.SetActive(false);
    }

    public void Previous()
    { 
        if (currentIndex == 0) return;
        if (currentIndex == languages.maxIndex()) nextButton.gameObject.SetActive(true);
        currentIndex--;
        SelectLanguage();
        if (currentIndex == 0) previousButton.gameObject.SetActive(false);
    }
    
}
