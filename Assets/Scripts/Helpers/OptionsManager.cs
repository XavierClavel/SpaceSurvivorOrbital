using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider musicSlider;

    [SerializeField] private TextMeshProUGUI sfxVolumeDisplay;
    [SerializeField] private TextMeshProUGUI musicVolumeDisplay;
    [SerializeField] private ItemSelector windowSelector;
    [SerializeField] private ItemSelector languageSelector;
    public static OptionsManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void UpdateMusicVolume()
    {
        SoundManager.UpdateMusicVolume(musicSlider.value * 0.01f);
        musicVolumeDisplay.SetText(((int)(musicSlider.value)).ToString());
    }
    
    public void UpdateSfxVolume(float value)
    {
        SoundManager.UpdateSfxVolume(sfxSlider.value * 0.01f);
        sfxVolumeDisplay.SetText(((int)(sfxSlider.value)).ToString());
    }

    public void SaveOptions()
    {
        var opt = new OptionsProfile();
        opt.sfxVolume = (int)sfxSlider.value;
        opt.musicVolume = (int)musicSlider.value;
        opt.language = LocalizationManager.getLanguage();
        opt.windowMode = windowSelector.getValue();
        
        SaveManager.setOptions(opt);
        gameObject.SetActive(false);
    }

    public void LoadOptions()
    {
        OptionsProfile optionsProfile = SaveManager.getOptions();
        Debug.Log(optionsProfile.musicVolume);
            
        sfxSlider.value = optionsProfile.sfxVolume;
        sfxVolumeDisplay.SetText(optionsProfile.sfxVolume.ToString());
        SoundManager.UpdateSfxVolume(optionsProfile.sfxVolume);

        musicSlider.value = optionsProfile.musicVolume;
        musicVolumeDisplay.SetText(optionsProfile.musicVolume.ToString());
        SoundManager.UpdateMusicVolume(optionsProfile.musicVolume);
        
        languageSelector.setSelected(optionsProfile.language);
        windowSelector.setSelected(optionsProfile.windowMode);
        
    }
}
