using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using UnityEngine.UI;
using TMPro;


public class Helpers : MonoBehaviour
{
    static readonly Dictionary<float, WaitForSeconds> waitDictionary = new Dictionary<float, WaitForSeconds>();
    static readonly Dictionary<float, WaitForSecondsRealtime> waitDictionaryRealtime = new Dictionary<float, WaitForSecondsRealtime>();
    public static readonly WaitForEndOfFrame GetWaitFrame = new WaitForEndOfFrame();
    public static readonly WaitForFixedUpdate GetWaitFixed = new WaitForFixedUpdate();
    public static Helpers instance;
    private static TextMeshProUGUI debugDisplay;
    private static Dictionary<int, TextMeshProUGUI> dictDebugDisplays = new Dictionary<int, TextMeshProUGUI>();
    [SerializeField] GameObject debugDisplayPrefab;

    public static Quaternion v2ToQuaternion(Vector2 v2)
    {
        float angle = Vector2.SignedAngle(Vector2.up, v2);
        return Quaternion.Euler(0f, 0f, angle);
    }

    public static int ClampInt(int value, int min, int max)
    {
        if (value < min) value = min;
        if (value > max) value = max;
        return value;
    }

    public static int CeilInt(int value, int max)
    {
        if (value > max) value = max;
        return value;
    }

    public static int FloorInt(int value, int min)
    {
        if (value < min) value = min;
        return value;
    }

    public static void CreateDebugDisplay(int index = -1)
    {
        TextMeshProUGUI currentDebugDisplay = Instantiate(debugDisplay);
        if (index < 0) debugDisplay = currentDebugDisplay;
        else dictDebugDisplays[index] = currentDebugDisplay;
    }

    public static void DebugDisplay(String str, int index = -1)
    {
        TextMeshProUGUI currentDebugDisplay;
        if (index < 0) currentDebugDisplay = debugDisplay;
        else currentDebugDisplay = dictDebugDisplays[index];
        currentDebugDisplay.text = str;
    }


    void Awake()
    {
        instance = this;
    }

    public static WaitForSeconds GetWait(float time)
    {
        if (waitDictionary.TryGetValue(time, out WaitForSeconds wait)) return wait;
        waitDictionary[time] = new WaitForSeconds(time);
        return waitDictionary[time];
    }

    public static WaitForSecondsRealtime GetWaitRealtime(float time)
    {
        if (waitDictionaryRealtime.TryGetValue(time, out WaitForSecondsRealtime wait)) return wait;
        waitDictionaryRealtime[time] = new WaitForSecondsRealtime(time);
        return waitDictionaryRealtime[time];
    }

    public void WaitAndKill(float time, GameObject objectToDestroy)
    {
        StartCoroutine(WaitThenKill(time, objectToDestroy));
    }

    IEnumerator WaitThenKill(float time, GameObject objectToDestroy)
    {
        yield return GetWait(time);
        Destroy(objectToDestroy);
    }

    public void LerpQuaternion(Transform objectTransform, Quaternion initialPos, Quaternion finalPos, float duration)
    {
        StartCoroutine(LerpQuaternionCoroutine(objectTransform, initialPos, finalPos, duration));
    }

    IEnumerator LerpQuaternionCoroutine(Transform objectTransform, Quaternion initialPos, Quaternion finalPos, float duration)
    {
        float invDuration = 1f / duration;
        float ratio = 0f;
        float startTime = Time.time;
        while (ratio < 1f)
        {
            ratio = (Time.time - startTime) * invDuration;
            objectTransform.rotation = Quaternion.Slerp(initialPos, finalPos, ratio);
            yield return GetWaitFrame;
        }
    }

}
