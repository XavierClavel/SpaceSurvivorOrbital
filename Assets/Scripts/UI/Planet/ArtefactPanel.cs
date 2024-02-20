using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ArtefactPanel : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] ParticleSystem particleBack;

    [SerializeField] private StringLocalizer titleDisplay;

    [SerializeField] private StringLocalizer descriptionDisplay;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private GameObject firstSelected;
    private static ArtefactPanel instance;

    private void Awake()
    {
        instance = this;
    }

    public static void Display(ArtefactHandler artefact)
    {
        instance.particleBack.Play();
        instance.rectTransform.DOScale(Vector3.one, 0.5f)
            .SetEase(Ease.OutQuad)
            .SetUpdate(true);
        PauseMenu.instance.PauseGame(false);
        instance.image.sprite = artefact.getIcon();
        string key = artefact.getKey();
        instance.titleDisplay.setKey(key + Vault.key.ButtonTitle);
        instance.descriptionDisplay.setKey(key + Vault.key.ButtonDescription);
        InputManager.setSelectedObject(instance.firstSelected);
    }

    public void Hide()
    {
        PauseMenu.instance.ResumeGame();
        instance.rectTransform.DOScale(Vector3.zero, 0.5f)
            .SetEase(Ease.InQuad)
            .SetUpdate(true);
    }
}
