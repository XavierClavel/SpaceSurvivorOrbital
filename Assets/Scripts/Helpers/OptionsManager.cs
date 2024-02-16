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
    public void UpdateMusicVolume()
    {
        SoundManager.UpdateMusicVolume(musicSlider.value);
        musicVolumeDisplay.SetText(((int)(musicSlider.value * 100)).ToString());
    }
    
    public void UpdateSfxVolume(float value)
    {
        SoundManager.UpdateSfxVolume(sfxSlider.value);
        sfxVolumeDisplay.SetText(((int)(sfxSlider.value * 100)).ToString());
    }
}
