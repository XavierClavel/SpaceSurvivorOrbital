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

public class Counter
{
    private bool value = false;
    private float duration;
    private float remainingTime = -1f;
    private MonoBehaviour context;
    private UnityAction onStart = null;
    private UnityAction onComplete = null;
    private IEnumerator counter()
    {
        onStart?.Invoke();
        value = true;
        remainingTime = duration;
        while (remainingTime > 0f)
        {
            yield return Helpers.getWaitFixed();
            remainingTime -= Time.fixedDeltaTime;
        }

        value = false;
        onComplete?.Invoke();
    }

    public void ResetCounter()
    {
        if (remainingTime <= 0f) context.StartCoroutine(counter());
        else remainingTime = duration;
    }

    public bool getValue()
    {
        return value;
    }

    public Counter(MonoBehaviour context, float duration)
    {
        this.context = context;
        this.duration = duration;
    }

    public Counter addOnStartEvent(UnityAction action)
    {
        onStart = action;
        return this;
    }

    public Counter addOnCompleteEvent(UnityAction action)
    {
        onComplete = action;
        return this;
    }
    
}

public class Stacker<T>
{
    private Dictionary<T, int> dict = new Dictionary<T, int>();

    public void stack(T value)
    {
        if (dict.ContainsKey(value)) dict[value]++;
        else dict[value] = 1;
    }

    public void unstack(T value)
    {
        if (!dict.ContainsKey(value)) return;
        dict[value]--;
        if (dict[value] == 0) dict.Remove(value);
    }

    public HashSet<T> get()
    {
        return dict.Keys.ToHashSet();
    }
}

public enum shape
{
    circle,
    square
}

public static class SingletonManager
{
    private static Dictionary<Type, MonoBehaviour> dictClassToInstance = new Dictionary<Type, MonoBehaviour>();

    public static bool OnInstanciation<T>(T instance) where T : MonoBehaviour
    {
        MonoBehaviour value;
        bool isFirstInstance = !dictClassToInstance.TryGetValue(typeof(T), out value) || value == instance;
        if (isFirstInstance)
        {
            dictClassToInstance[typeof(T)] = instance;
            GameObject.DontDestroyOnLoad(instance.gameObject);
        }
        else
        {
            GameObject.Destroy(instance.gameObject);
        }
        return isFirstInstance;
    }

    public static T get<T>() where T : MonoBehaviour
    {
        if (!dictClassToInstance.ContainsKey(typeof(T)))
        {
            throw new System.ArgumentException($"No singleton of type {typeof(T)} has currently been initialized.");
        }
        return (T)dictClassToInstance[typeof(T)];
    }
}

public static class Extensions
{

    public static Vector3 getRotation(this Vector2 direction)
    {
        return Vector2.SignedAngle(Vector2.right, direction) * Vector3.forward;
    }

    public static Vector3 getRotation(this Vector3 direction)
    {
        return Vector2.SignedAngle(Vector2.right, direction) * Vector3.forward;
    }

    public static Quaternion getRotationQuat(this Vector3 direction)
    {
        return Quaternion.Euler(direction.getRotation());
    }


    public static Vector3 getRotationTo(this Transform t, Transform target)
    {
        return (target.position - t.position).getRotation();
    }
    public static Vector3 getRotationTo(this Transform t, Vector3 position)
    {
        return (position - t.position).getRotation();
    }

    public static Bounds getBounds(this Camera camera)
    {
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float cameraHeight = camera.orthographicSize * 2;
        Bounds bounds = new Bounds(
            camera.transform.position,
            new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
        return bounds;
    }

    public static Vector3 getRandom(this Bounds bounds)
    {
        float x = bounds.center.x + Helpers.getRandomSign() * Helpers.getRandomFloat(bounds.extents.x * 0.8f);
        float y = bounds.center.y + Helpers.getRandomSign() * Helpers.getRandomFloat(bounds.extents.y * 0.8f);
        return new Vector3(x, y, 0f);
    }

    public static void TryRemove<T1, T2>(this Dictionary<T1, T2> dict, T1 key)
    {
        if (dict.ContainsKey(key)) dict.Remove(key);
    }

    public static T Pop<T>(this List<T> list, int index = 0)
    {
        T value = list[index];
        list.RemoveAt(index);
        return value;
    }
    
    public static Vector2 perpendicular(this Vector2 v)
    {
        return new Vector2(v.y, -v.x);
    }

    public static T[] append<T>(this T[] array, T element)
    {
        return array.Append(element).ToArray();
    }

    public static T Switch<T>(this T switcher, T value1, T value2)
    {
        if (switcher.Equals(value1)) switcher = value2;
        else if (switcher.Equals(value2)) switcher = value1;
        else throw new ArgumentOutOfRangeException("switcher equals neither value1 or value2");
        return switcher;
    }

    public static int Mean(this Vector2Int v2)
    {
        return (int)Mathf.Round((float)(v2.x + v2.y) * 0.5f);
    }

    public static int IndexOf(this List<string> list, string value, System.StringComparison comparison = System.StringComparison.OrdinalIgnoreCase)
    {
        return list.FindIndex(x => x.Equals(value, comparison));
    }

    public static string RemoveFirst(this string str)
    {
        str.Remove(0, 1);
        return str.Substring(1);
    }

    public static string RemoveLast(this string str)
    {
        str.Remove(str.Length - 1, 1);
        return str.Substring(0, str.Length - 1);
    }

    public static char First(this string str)
    {
        return str[0];
    }
    public static char Last(this string str)
    {
        return str[^1];
    }

    public static void updateX(this Transform t, float value)
    {
        t.position = new Vector3(value, t.position.y, t.position.z);
    }

    public static void updateY(this Transform t, float value)
    {
        t.position = new Vector3(t.position.x, value, t.position.z);
    }

    public static void updateZ(this Transform t, float value)
    {
        t.position = new Vector3(t.position.x, t.position.y, value);
    }

    public static TEnum getRandom<TEnum>() where TEnum : System.Enum
    {
        if (!typeof(TEnum).IsEnum)
        {
            throw new ArgumentException("T must be an enumerated type");
        }

        Array values = Enum.GetValues(typeof(TEnum));
        return (TEnum)values.GetValue(UnityEngine.Random.Range(0, values.Length));
    }


    public static TEnum getRandomA<TEnum>(TEnum enumType) where TEnum : System.Enum
    {
        if (!typeof(TEnum).IsEnum)
        {
            throw new ArgumentException("T must be an enumerated type");
        }

        Array values = Enum.GetValues(typeof(TEnum));
        return (TEnum)values.GetValue(UnityEngine.Random.Range(0, values.Length));
    }

    ///<summary>
    ///Removes the item from the list if it is present, else does nothing.
    ///</summary>
    public static void TryRemove<T>(this List<T> list, T value)
    {
        if (list.Contains(value)) list.Remove(value);
    }

    ///<summary>
    ///Adds the item to the list if it is absent, else does nothing.
    ///</summary>
    public static void TryAdd<T>(this List<T> list, T value)
    {
        if (!list.Contains(value)) list.Add(value);
    }

    public static List<T> TryAdd<T>(this List<T> list, List<T> values)
    {
        if (values == null) return list;
        foreach (T item in values)
        {
            list.TryAdd(item);
        }
        return list;
    }

    ///<summary>
    ///Adds an object X times to a list
    ///</summary>
    public static void AddMultiple<T>(this List<T> list, T value, int amount)
    {
        if (amount == 0) return;
        if (amount < 0) throw new ArgumentOutOfRangeException();
        for (int i = 0; i < amount; i++)
        {
            list.Add(value);
        }
    }

    public static int IntDistance(this Vector2Int pos1, Vector2Int pos2)
    {
        //TODO : distance calculation : losange, square, circle
        int distanceX = Helpers.IntAbs(pos1.x - pos2.x);
        int distanceY = Helpers.IntAbs(pos1.y - pos2.y);
        return distanceX + distanceY;
    }

    public static List<Vector2Int> GetPosAtDistance(this Vector2Int center, int distance)
    {
        List<Vector2Int> list = new List<Vector2Int>();
        Vector2Int currentPos = center + distance * Vector2Int.up;
        while (currentPos.y > center.y)
        {
            currentPos += Vector2Int.down + Vector2Int.right;
            list.Add(currentPos);
        }
        while (currentPos.x > center.x)
        {
            currentPos += Vector2Int.down + Vector2Int.left;
            list.Add(currentPos);
        }
        while (currentPos.y < center.y)
        {
            currentPos += Vector2Int.up + Vector2Int.left;
            list.Add(currentPos);
        }
        while (currentPos.x < center.x)
        {
            currentPos += Vector2Int.up + Vector2Int.right;
            list.Add(currentPos);
        }

        return list;
    }

    public static List<Vector2Int> GetPosInRange(this Vector2Int center, int maxDistance)
    {
        List<Vector2Int> list = new List<Vector2Int>();
        for (int distance = 1; distance <= maxDistance; distance++)
        {
            list.AddList(GetPosAtDistance(center, distance));
        }
        return list;
    }

    public static int mod(this int x, int m)
    {
        return (x % m + m) % m;
    }

    public static float mod(this float x, int m)
    {
        return (x % m + m) % m;
    }

    public static Vector3 mod(this Vector3 v, Vector3 m)
    {
        return new Vector3(
            (v.x % m.x + m.x) % m.x,
            (v.y % m.y + m.y) % m.y,
            (v.z % m.z + m.z) % m.z
        );
    }

    public static Vector3 mod(this Vector3 v, int m)
    {
        return new Vector3(
            (v.x % m + m) % m,
            (v.y % m + m) % m,
            (v.z % m + m) % m
        );
    }

    public static float getRandom(this Vector2 v)
    {
        return UnityEngine.Random.Range(v.x, v.y);
    }

    ///<summary>Returns random int contained between x inclusive and y inclusive components of input vector2int.</summary>
    public static int getRandom(this Vector2Int v)
    {
        return UnityEngine.Random.Range(v.x, v.y + 1);
    }

    ///<summary>Returns random item of list.</summary>
    public static T getRandom<T>(this IList<T> list)
    {
        switch (list.Count)
        {
            case 0:
                throw new ArgumentException("List is empty");
            case 1:
                return list[0];
            default:
            {
                int randomIndex = UnityEngine.Random.Range(0, list.Count);
                return list[randomIndex];
            }
        }
    }
    
    ///<summary>Returns random item of list and removes it from the list.</summary>
    public static T popRandom<T>(this IList<T> list)
    {
        if (list.Count == 0) throw new ArgumentException("List is empty");
        int randomIndex = UnityEngine.Random.Range(0, list.Count);
        T value = list[randomIndex];
        list.RemoveAt(randomIndex);
        return value;
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
        if (list2 == null) return list1.Copy();
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

    public static List<T> Difference<T>(this List<T> list1, List<T> list2)
    {
        List<T> intersection = list1.Intersection(list2);
        List<T> union = list1.Union(list2);
        List<T> result = new List<T>();
        foreach (T item in union)
        {
            if (!intersection.Contains(item))
            {
                result.TryAdd(item);
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
    ///Zdds multiple items to a given list.
    ///</summary>
    public static void AddList<T>(this List<T> list1, List<T> list2)
    {
        foreach (T item in list2)
        {
            list1.Add(item);
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
    private static WaitForFixedUpdate waitFixedUpdate = new WaitForFixedUpdate(); 
    public static readonly WaitForEndOfFrame GetWaitFrame = new WaitForEndOfFrame();
    public static Helpers instance;
    private static TextMeshProUGUI debugDisplay;
    private static Dictionary<int, TextMeshProUGUI> dictDebugDisplays = new Dictionary<int, TextMeshProUGUI>();
    [SerializeField] GameObject debugDisplayPrefab;
    static bool? platformAndroidValue = null;

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
            Destroy(child.gameObject);
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

    public static TEnum getRandomEnum<TEnum>() where TEnum : System.Enum
    {
        if (!typeof(TEnum).IsEnum)
        {
            throw new ArgumentException("T must be an enumerated type");
        }

        Array values = Enum.GetValues(typeof(TEnum));
        return (TEnum)values.GetValue(UnityEngine.Random.Range(0, values.Length));
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
        ParticleSystem ps = Instantiate(prefabPS, t.position, Quaternion.identity);
        ps.Play();
        GameObject.Destroy(ps.gameObject, ps.main.duration + 1f);
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