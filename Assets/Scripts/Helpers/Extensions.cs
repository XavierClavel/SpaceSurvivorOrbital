using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    public static string toBase64String(this BitArray b)
    {
        return Convert.ToBase64String(b.toByteArray());
    }

    public static Byte[] toByteArray(this BitArray b)
    {
        Byte[] e = new Byte[(b.Length / 8 + (b.Length % 8 == 0 ? 0 : 1))];
        b.CopyTo(e, 0);
        return e;
    }
    
    public static void KillAllChildren(this Transform t)
    {
        foreach (Transform child in t)
        {
            Component.Destroy(child.gameObject);
        }
    }
    
    private static System.Random rng = new System.Random();

    public static bool isEmpty<T>(this IList<T> list)
    {
        return list.Count == 0;
    }
    
    public static List<T> getRandomList<T>(this IEnumerable<T> list, int size)
    {
        return list.OrderBy(x => rng.Next()).Take(size).ToList();
    }

    public static void Shuffle<T>(this IList<T> list)  
    {  
        int n = list.Count;  
        while (n > 1) {  
            n--;  
            int k = rng.Next(n + 1);  
            (list[k], list[n]) = (list[n], list[k]);
        }  
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