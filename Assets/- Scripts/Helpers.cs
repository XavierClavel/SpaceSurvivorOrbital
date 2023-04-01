using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEditor;
using UnityEngine.UI;

// Defines an attribute that makes the array use enum values as labels.
// Use like this:
//      [NamedArray(typeof(eDirection))] public GameObject[] m_Directions;
 
public class NamedArrayAttribute : PropertyAttribute {
    public Type TargetEnum;
    public NamedArrayAttribute(Type TargetEnum) {
        this.TargetEnum = TargetEnum;
    }
}

 
#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(NamedArrayAttribute))]
public class NamedArrayDrawer : PropertyDrawer {
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        // Properly configure height for expanded contents.
        return EditorGUI.GetPropertyHeight(property, label, property.isExpanded);
    }
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        // Replace label with enum name if possible.
        try {
            var config = attribute as NamedArrayAttribute;
            var enum_names = System.Enum.GetNames(config.TargetEnum);
            int pos = int.Parse(property.propertyPath.Split('[', ']')[1]);
            var enum_label = enum_names.GetValue(pos) as string;

            // Make names nicer to read (but won't exactly match enum definition).
            enum_label = ObjectNames.NicifyVariableName(enum_label);
            label = new GUIContent(enum_label);
        } catch {
            // keep default label
        }
        EditorGUI.PropertyField(position, property, label, property.isExpanded);
    }
}
#endif



public class GamepadMenuHandler
{
    public GameObject[] menuItems;
    bool shouldLoop;
    public int index = 0;
    int lastIndex;
    public GamepadMenuHandler(GameObject[] menuItems, bool shouldLoop) {
        this.menuItems = menuItems;
        this.shouldLoop = shouldLoop;
    }

    public void NavigateDown()
    {
        index ++;
        if (index == menuItems.Length) {
            if (shouldLoop) index = 0;
            else index --;
        }
        Animate(ref lastIndex, index);
    }

    public void NavigateUp()
    {
        index --;
        if (index < 0) {
            if (shouldLoop) index = menuItems.Length - 1;
            else index ++;
        }
        Animate(ref lastIndex, index);
        
    }

    public void Animate(ref int lastIndex, int newIndex)
    {
        menuItems[lastIndex].GetComponent<Animator>().SetBool("Normal", true);
        menuItems[newIndex].GetComponent<Animator>().SetBool("Highlighted", true);
        lastIndex = newIndex;
    }

    public void Validate()
    {
        menuItems[index].GetComponent<Animator>().SetBool("Pressed", true);
        menuItems[index].GetComponent<Button>().onClick.Invoke();
    }

    public void Reset()
    {
        index = 0;
        lastIndex = 0;
        menuItems[index].GetComponent<Animator>().SetBool("Highlighted", true);
    }

}

public class Helpers : MonoBehaviour
{
    static readonly Dictionary<float, WaitForSeconds> waitDictionary = new Dictionary<float, WaitForSeconds>();
    static readonly Dictionary<float, WaitForSecondsRealtime> waitDictionaryRealtime = new Dictionary<float, WaitForSecondsRealtime>();
    public static readonly WaitForEndOfFrame GetWaitFrame = new WaitForEndOfFrame();
    public static Helpers instance;

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

    public void WaitAndKill(float time, GameObject objectToDestroy) {
        StartCoroutine(WaitThenKill(time, objectToDestroy));
    }

    IEnumerator WaitThenKill(float time, GameObject objectToDestroy) {
        yield return GetWait(time);
        Destroy(objectToDestroy);
    }
    IEnumerator test()
    {
        yield return null;
    }

    public void LerpQuaternion(Transform objectTransform, Quaternion initialPos, Quaternion finalPos, float duration) {
        StartCoroutine(LerpQuaternionCoroutine(objectTransform, initialPos, finalPos, duration));
    }

    IEnumerator LerpQuaternionCoroutine(Transform objectTransform, Quaternion initialPos, Quaternion finalPos, float duration) {
        float invDuration = 1f/duration;
        float ratio = 0f;
        float startTime = Time.time;
        while (ratio < 1f) {
            ratio = (Time.time - startTime) * invDuration;
            objectTransform.rotation = Quaternion.Slerp(initialPos, finalPos, ratio);
            yield return GetWaitFrame;
        }
    }

}
