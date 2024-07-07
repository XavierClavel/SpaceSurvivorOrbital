using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.Events;
using Object = UnityEngine.Object;

public enum shape
{
    circle,
    square
}

public static class Helpers
{
    static readonly Dictionary<float, WaitForSeconds> waitDictionary = new Dictionary<float, WaitForSeconds>();
    static readonly Dictionary<float, WaitForSecondsRealtime> waitDictionaryRealtime = new Dictionary<float, WaitForSecondsRealtime>();
    private static WaitForFixedUpdate waitFixedUpdate = new WaitForFixedUpdate(); 
    public static readonly WaitForEndOfFrame GetWaitFrame = new WaitForEndOfFrame();
    private static TextMeshProUGUI debugDisplay;
    private static Dictionary<int, TextMeshProUGUI> dictDebugDisplays = new Dictionary<int, TextMeshProUGUI>();
    static bool? platformAndroidValue = null;
    
    public static void FireProjectiles(UnityAction<float> fireAction , int amountBullets, float spread, float startRotation = 0f)
    {
        int sideBullets = amountBullets / 2;

        if (amountBullets % 2 == 1)
        {
            for (int i = -sideBullets; i <= sideBullets; i++)
            {
                fireAction.Invoke(startRotation + i * spread);
            }
            return;
        }

        for (int i = -sideBullets; i <= sideBullets; i++)
        {
            if (i == 0) continue;
            float j = i - Mathf.Sign(i) * 0.5f;

            fireAction.Invoke(startRotation + j * spread);
        }
    }

    public static WaitForFixedUpdate getWaitFixed()
    {
        return waitFixedUpdate;
    } 

    public static List<Collider2D> OverlapCircularArcAll(Transform center, Vector2 direction, float radius, float span, int layerMask)
    {
        List<Collider2D> validColliders = new List<Collider2D>();
        float halfSpan = span * 0.5f;

        Collider2D[] collidersInRadius = Physics2D.OverlapCircleAll(center.position, radius, layerMask);
        foreach (Collider2D collider in collidersInRadius)
        {
            if (Vector2.Angle(collider.bounds.center - center.position, direction) <= halfSpan)
            { 
                validColliders.Add(collider);
            }
        }

        RaycastHit2D[] collidersEdgeLeft = Physics2D.RaycastAll(center.position, direction, radius, layerMask);
        foreach (RaycastHit2D raycastHit in collidersEdgeLeft)
        {
            validColliders.TryAdd(raycastHit.collider);
        }

        RaycastHit2D[] collidersEdgeRight = Physics2D.RaycastAll(center.position, direction, radius, layerMask);
        foreach (RaycastHit2D raycastHit in collidersEdgeRight)
        {
            validColliders.TryAdd(raycastHit.collider);
        }

        return validColliders;
    }


    public static void SetParent(Transform instance, Transform parent, int z = 0, float scale = 1f)
    {
        instance.SetParent(parent);
        instance.localScale = Vector3.one * scale;
        instance.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero + z * Vector3.forward;
    }

    public static void SetParent(GameObject instance, Transform parent)
    {
        SetParent(instance.transform, parent);
    }

    public static void SetParent(GameObject instance, GameObject parent)
    {
        SetParent(instance.transform, parent.transform);
    }

    public static void SetParent(Transform instance, GameObject parent)
    {
        SetParent(instance, parent.transform);
    }

    public static void KillAllChildren(Transform t)
    {
        foreach (Transform child in t)
        {
            Object.Destroy(child.gameObject);
        }
    }

    public static void printList<T>(List<T> list)
    {
        foreach (T item in list) Debug.Log(item);
    }


    public static float getRandomFloat(float maxValue)
    {
        return UnityEngine.Random.Range(0f, maxValue);
    }

    ///<summary>Returns -1f or 1f.</summary>
    public static float getRandomSign()
    {
        return UnityEngine.Random.Range(0, 2) * 2 - 1;
    }

    public static bool isPlatformAndroid()
    {
        if (platformAndroidValue != null) return (bool)platformAndroidValue;
#if UNITY_EDITOR
        platformAndroidValue = false;
        return (bool)platformAndroidValue;
#else
        platformAndroidValue = Application.platform == RuntimePlatform.Android;
        return (bool)platformAndroidValue;
#endif
    }

    public static Quaternion LookRotation2D(Vector2 direction)
    {
        float angle = Vector2.SignedAngle(Vector2.right, direction);
        return Quaternion.Euler(0f, 0f, angle);
    }
    
    public static Quaternion LookAt2D(Vector2 position, Vector2 target)
    {
        Vector2 direction = target - position;
        float angle = Vector2.SignedAngle(Vector2.up, direction);
        return Quaternion.Euler(0f, 0f, angle);
    }

    public static TEnum ParseEnum<TEnum>(string s)
    {
        return (TEnum)Enum.Parse(typeof(TEnum), s, true);
    }

    public static List<string> ParseList(string s)
    {
        string[] values = s.Split(',');
        List<string> newValues = new List<string>();
        foreach (string value in values)
        {
            string v = value.Trim();
            if (v != "") newValues.Add(value.Trim());
        }
        return newValues;
    }

    public static Vector2Int ParseVector2Int(string s)
    {
        if (s == "") return Vector2Int.zero;
        if (!s.Contains('-')) return int.Parse(s) * Vector2Int.one;
        string[] values = s.Split('-');
        int v1 = int.Parse(values[0].Trim());
        int v2 = int.Parse(values[1].Trim());
        return new Vector2Int(v1, v2);
    }

    public static void SetMappedValue<T>(List<string> s, Dictionary<int, int> mapper, int i, out T variable)
    {
        if (i >= s.Count) throw new ArgumentOutOfRangeException($"{i} superior to max index {s.Count - 1}");
        setValue(out variable, s[mapper[i]].Trim());
    }

    public static T parseString<T>(string s)
    {
        s = s.Trim();
        try
        {
            switch (System.Type.GetTypeCode(typeof(T)))
            {
                case System.TypeCode.Int32:
                    if (typeof(T).IsEnum) return (T)(object)ParseEnum<T>(s);
                    return (T)(object)int.Parse(s);

                case System.TypeCode.Single:
                    return (T)(object)float.Parse(s, new CultureInfo(Vault.other.cultureInfoFR).NumberFormat);

                case System.TypeCode.String:
                    return (T)(object)s;
                
                case TypeCode.Boolean:
                    return (T)(object)Boolean.Parse(s);

                case System.TypeCode.Object:
                    if (typeof(T) == typeof(Vector2Int)) return (T)(object)ParseVector2Int(s);
                    if (typeof(T) == typeof(List<string>)) return (T)(object)ParseList(s);
                    throw new ArgumentException($"Type {typeof(T)}) not supported.");

                default:
                    throw new ArgumentException($"Type {typeof(T)}) not supported.");
            }
        }
        catch
        {
            throw new ArgumentException($"Failed to parse \"{s}\" for variable of type {typeof(T)})");
        }
    }

    public static void setValue<T>(out T variable, string s)
    {
        variable = parseString<T>(s);
    }


    public static float Sinh(float value)
    {
        return 0.5f * (Mathf.Exp(value) - Mathf.Exp(-value));
    }

    public static Color color_whiteTransparent = new Color(1f, 1f, 1f, 0f);

    public static Color ColorFromInt(int r, int g, int b, int a)
    {
        return new Color((float)r / 255f, (float)g / 255f, (float)b / 255f, (float)a / 255f);
    }

    public static bool ProbabilisticBool(float chanceOfSuccess)
    {
        return UnityEngine.Random.Range(0f, 1f) <= chanceOfSuccess;
    }

    public static int IntAbs(int value)
    {
        return value < 0 ? -value : value;
    }

    public static Vector3 getRandomPositionInRadius(float radius, shape shape = shape.circle)
    {
        switch (shape)
        {
            case shape.circle:
                return radius * UnityEngine.Random.insideUnitCircle;

            case shape.square:
                float valueX = Helpers.getRandomSign() * UnityEngine.Random.Range(0f, radius);
                float valueY = Helpers.getRandomSign() * UnityEngine.Random.Range(0f, radius);
                return new Vector3(valueX, valueY, 0f);

            default:
                throw new ArgumentException("value not recognized");
        }

    }

    public static Vector3 getRandomPositionInRadius(Vector2 size, shape shape = shape.square)
    {
        switch (shape)
        {
            case shape.circle:
                throw new NotImplementedException();

            case shape.square:
                float valueX = Helpers.getRandomSign() * UnityEngine.Random.Range(0f, size.x);
                float valueY = Helpers.getRandomSign() * UnityEngine.Random.Range(0f, size.y);
                return new Vector3(valueX, valueY, 0f);

            default:
                throw new ArgumentException("value not recognized");
        }

    }

    public static Vector3 getRandomPositionInRing(float minRadius, float maxRadius, shape shape)
    {
        switch (shape)
        {
            case shape.circle:
                throw new NotImplementedException();

            case shape.square:
                float valueX = Helpers.getRandomSign() * UnityEngine.Random.Range(minRadius, maxRadius);
                float valueY = Helpers.getRandomSign() * UnityEngine.Random.Range(minRadius, maxRadius);
                return new Vector3(valueX, valueY, 0f);

            default:
                throw new ArgumentException("value not recognized");
        }

    }

    public static Vector3 getRandomPositionInRing(Vector2 radius, shape shape)
    {
        switch (shape)
        {
            case shape.circle:
                throw new NotImplementedException();

            case shape.square:
                float valueX;
                float valueY;
                if (getRandomBool())
                {
                    valueX = Helpers.getRandomSign() * radius.x;
                    valueY = Helpers.getRandomSign() * getRandomFloat(radius.y);
                }
                else
                {
                    valueX = Helpers.getRandomSign() * getRandomFloat(radius.x);
                    valueY = Helpers.getRandomSign() * radius.y;
                }
                
                return new Vector3(valueX, valueY, 0f);

            default:
                throw new ArgumentException("value not recognized");
        }

    }

    public static bool getRandomBool()
    {
        return UnityEngine.Random.Range(0, 2) == 0;
    }

    public static TEnum getRandomEnum<TEnum>(params TEnum[] excludedValues) where TEnum : System.Enum
    {
        if (!typeof(TEnum).IsEnum)
        {
            throw new ArgumentException("T must be an enumerated type");
        }

        var values = Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToList();
        values.TryRemove(excludedValues);

        return values.getRandom();
    }
    
    


    public static Vector2Int RoundToVector2IntStep(Vector3 value, Vector2Int step)
    {
        return new Vector2Int(RoundToIntStep(value.x, step.x), RoundToIntStep(value.y, step.y));
    }

    public static Vector2Int RoundToVector2IndexStep(Vector3 value, Vector2Int step)
    {
        return new Vector2Int(RoundToIntStep(value.x, step.x) / step.x, RoundToIntStep(value.y, step.y) / step.y);
    }

    public static Vector2Int CentralSymmetry(Vector2Int value, Vector2Int center)
    {
        Vector2Int offset = center - value;
        return center + offset;
    }

    public static int IntSign(int value)
    {
        return value >= 0 ? 1 : -1;
    }

    public static int RoundToIntStep(float value, int step)
    {
        return Mathf.RoundToInt(value / (float)step) * step;
    }

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

    public static float FloorFloat(float value, float min)
    {
        if (value < min) value = min;
        return value;
    }

    public static void CreateDebugDisplay(int index = -1)
    {
        TextMeshProUGUI currentDebugDisplay = Object.Instantiate(debugDisplay);
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

    public static WaitForSeconds getWait(float time)
    {
        if (waitDictionary.TryGetValue(time, out WaitForSeconds wait)) return wait;
        waitDictionary[time] = new WaitForSeconds(time);
        return waitDictionary[time];
    }
    

    public static WaitForSecondsRealtime getWaitRealtime(float time)
    {
        if (waitDictionaryRealtime.TryGetValue(time, out WaitForSecondsRealtime wait)) return wait;
        waitDictionaryRealtime[time] = new WaitForSecondsRealtime(time);
        return waitDictionaryRealtime[time];
    }

    public static void SpawnPS(Transform t, ParticleSystem prefabPS)
    {
        ParticleSystem ps = Object.Instantiate(prefabPS, t.position, Quaternion.identity);
        ps.Play();
        GameObject.Destroy(ps.gameObject, ps.main.duration + 1f);
    }

/*
    public void LerpQuaternion(Transform objectTransform, Quaternion initialPos, Quaternion finalPos, float duration)
    {
        Object.StartCoroutine(LerpQuaternionCoroutine(objectTransform, initialPos, finalPos, duration));
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
    */

}

public class GenericDictionary<T1>
{
    private Dictionary<T1, object> _dict = new Dictionary<T1, object>();

    public void Add<T2>(T1 key, T2 value) where T2 : class
    {
        _dict.Add(key, value);
    }

    public T2 GetValue<T2>(T1 key) where T2 : class
    {
        return _dict[key] as T2;
    }
}

public class RefContainer<T>
{
    public T Ref { get; set; }
    public string getType() { return nameof(T); }
}