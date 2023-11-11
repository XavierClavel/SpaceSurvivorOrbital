using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectButton : MonoBehaviour
{
    [HideInInspector] public Button button;
    [HideInInspector] public RectTransform rectTransform;
    public Image background;

    private void Awake()
    {
        button = GetComponent<Button>();
        rectTransform = GetComponent<RectTransform>();
    }
}
