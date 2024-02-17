using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ArtefactPanel : MonoBehaviour
{
    [SerializeField] private Image image;

    [SerializeField] private StringLocalizer titleDisplay;

    [SerializeField] private StringLocalizer descriptionDisplay;
    [SerializeField] private RectTransform rectTransform;
    private static ArtefactPanel instance;

    private void Awake()
    {
        instance = this;
    }

    public static void Display(ArtefactHandler artefact)
    {
        instance.rectTransform.DOScale(Vector3.one, 0.5f)
            .SetEase(Ease.InOutQuad)
            .SetUpdate(true);
        PauseMenu.instance.PauseGame(false);
        instance.image.sprite = artefact.getIcon();
        string key = artefact.getKey();
        instance.titleDisplay.setKey(key + Vault.key.ButtonTitle);
        instance.descriptionDisplay.setKey(key + Vault.key.ButtonDescription);
    }

    public void Hide()
    {
        PauseMenu.instance.ResumeGame();
        instance.rectTransform.DOScale(Vector3.zero, 0.5f)
            .SetEase(Ease.InOutQuad)
            .SetUpdate(true);
    }
}
