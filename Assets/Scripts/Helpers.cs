using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public static class Extensions
{
    public static int mod(ref this int x, int m)
    {
        x = (x % m + m) % m;
        return x;
    }

    public static T getRandom<T>(this IList<T> list)
    {
        int randomIndex = UnityEngine.Random.Range(0, list.Count);
        return list[randomIndex];
    }

    ///<summary>
    ///Returns a copy of the list.
    ///</summary>
    public static T overflow<T>(this T[,] array, int x, int y)
    {
        int sizeX = array.GetLength(1);
        int sizeY = array.GetLength(0);
        int newX = x.mod(sizeX);
        int newY = y.mod(sizeY);
        return array[newX, newY];
    }

    ///<summary>
    ///Returns a copy of the list.
    ///</summary>
    public static T overflow<T>(this T[,] array, Vector2Int xy)
    {
        int sizeX = array.GetLength(1);
        int sizeY = array.GetLength(0);
        int x = xy.x;
        int y = xy.y;
        int newX = x.mod(sizeX);
        int newY = y.mod(sizeY);
        Debug.Log(new Vector2Int(newX, newY));
        return array[newX, newY];
    }

    ///<summary>
    ///Returns a copy of the list.
    ///</summary>
    public static List<T> Copy<T>(this List<T> list)
    {
        return list.ToArray().ToList();

    }

    ///<summary>
    ///Returns the mathematical union of two lists of Item.
    ///</summary>
    public static List<T> Union<T>(this List<T> list1, List<T> list2)
    {
        List<T> result = list1.Copy();
        foreach (T item in list2)
        {
            if (!result.Contains(item))
            {
                result.Add(item);
            }
        }
        return result;
    }

    ///<summary>
    ///Returns the mathematical intesection of two lists of Item.
    ///</summary>
    public static List<T> Intersection<T>(this List<T> list1, List<T> list2)
    {
        List<T> result = new List<T>();
        foreach (T item in list1)
        {
            if (list2.Contains(item))
            {
                result.Add(item);
            }
        }
        return result;
    }

    ///<summary>
    ///Removes multiple items from a given list.
    ///</summary>
    public static void RemoveList<T>(this List<T> list1, List<T> list2)
    {
        foreach (T item in list2)
        {
            if (list1.Contains(item))
            {
                list1.Remove(item);
            }
        }
    }


    ///<summary>
    ///Parses the text and returns the first substring between two instances of the defined tag.
    ///</summary>
    public static string GetStrBetweenTag(this string value, string tag)
    {
        if (value.Contains(tag))
        {
            int index = value.IndexOf(tag) + tag.Length;
            return value.Substring(index, value.IndexOf(tag, index) - index);
        }
        else return "";
    }

    ///<summary>
    ///Parses the text and returns all the substring contained between two instances of the defined tag.
    ///</summary>
    public static List<string> GetAllStrBetweenTag(this string value, string tag)
    {
        List<string> returnValue = new List<string>();
        int tagLength = tag.Length;
        while (value.Contains(tag))
        {
            int startIndex = value.IndexOf(tag) + tagLength;
            int endIndex = value.IndexOf(tag, startIndex);
            returnValue.Add(value.Substring(startIndex, endIndex - startIndex));
            value = value.Substring(endIndex + tagLength);
        }
        return returnValue;
    }

    public static string GetStrBetweenTags(this string value, string startTag, string endTag)
    {
        if (value.Contains(startTag) && value.Contains(endTag))
        {
            int index = value.IndexOf(startTag) + startTag.Length;
            return value.Substring(index, value.IndexOf(endTag) - index);
        }
        else return null;
    }

    public static IEnumerable<int> AllIndexesOf(this string str, string searchstring)
    {
        int minIndex = str.IndexOf(searchstring);
        while (minIndex != -1 && minIndex + searchstring.Length < str.Length)
        {
            Debug.Log(minIndex);
            Debug.Log(str[minIndex - 1]);
            if (str[minIndex - 1] == '<') yield return minIndex;
            minIndex = str.IndexOf(searchstring, minIndex + searchstring.Length);// + searchstring.Length;
        }
    }
}


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

    public static void SpawnPS(Transform t, ParticleSystem prefabPS)
    {
        ParticleSystem ps = Instantiate(prefabPS, t);
        ps.Play();
        instance.WaitAndKill(ps.main.duration + 1f, ps.gameObject);
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
