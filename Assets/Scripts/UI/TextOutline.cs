using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextOutline : MonoBehaviour
{

    void Awake()
    {
        TextMeshPro textmeshPro = GetComponent<TextMeshPro>();
        textmeshPro.outlineWidth = 2f;
        textmeshPro.outlineColor = Color.black;
    }

}
