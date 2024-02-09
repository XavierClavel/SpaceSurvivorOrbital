using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UpgradePanelButton : MonoBehaviour
{
    [SerializeField] private Button button;

    [SerializeField] private Image image;
    [SerializeField] private GameObject parent;

    public void Disable()
    {
        parent.SetActive(false);
    }

    public void setSprite(Sprite sprite)
    {
        image.sprite = sprite;
    }

    public void setAction(UnityAction action)
    {
        button.onClick.AddListener(action);
    }
}
