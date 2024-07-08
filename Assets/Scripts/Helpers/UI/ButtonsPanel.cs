using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsPanel : MonoBehaviour
{
    [SerializeField] private string key;
    [SerializeField] private CanvasGroup canvasGroup;
    public void setKey(string value) => key = value; 
    public string getKey() => key;
    public void setActive() => canvasGroup.interactable = true;
    public void setInactive() => canvasGroup.interactable = false;
}
