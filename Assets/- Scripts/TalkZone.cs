using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkZone : MonoBehaviour
{

    [HideInInspector] public Transform targetTransform;

    void Start()
    {
        targetTransform = GetComponentInChildren<Transform>();
    }
}
