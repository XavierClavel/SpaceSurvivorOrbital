using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEditor;
using UnityEngine.UI;

public class CustomArrayAttribute : PropertyAttribute {
    public Type TargetEnum;
    public CustomArrayAttribute(Type TargetEnum) {
        this.TargetEnum = TargetEnum;
    }
}

 [CustomPropertyDrawer(typeof(CustomArrayAttribute))]
 public class ArrayAttribute : PropertyDrawer
 {
     private float a = 17;
     public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
     {
         float width = position.width * 0.25f;
 
         Rect rect = new Rect(position);
         rect.width = width;
 
         EditorGUI.LabelField(rect, label);
         rect.x += width;
 
         float labelWidthTmp = EditorGUIUtility.labelWidth;
         EditorGUIUtility.labelWidth = 24f; // your label width (8) x3
         EditorGUI.PropertyField(rect, prop.FindPropertyRelative("audioClip"), new GUIContent("A"));
         rect.x += width;
         EditorGUI.PropertyField(rect, prop.FindPropertyRelative("volume"), new GUIContent("B"));
 
         EditorGUIUtility.labelWidth = labelWidthTmp;
     }
 }

/*
[CustomPropertyDrawer(typeof(CustomArrayAttribute))]
     public class ResourceReferenceDrawer : PropertyDrawer
 {
     List<string> debugList = new List<string>();
     int index = 0;
     public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        // Properly configure height for expanded contents.
        return EditorGUI.GetPropertyHeight(property, label, property.isExpanded);
    }
     public override void OnGUI (Rect pos,SerializedProperty prop,GUIContent label)
     {         
         //UnityEngine.Object newObj = null;

            /*var config = attribute as NamedArrayAttribute;
            var enum_names = System.Enum.GetNames(config.TargetEnum);
            int posx = int.Parse(prop.propertyPath.Split('[', ']')[1]);
            var enum_label = enum_names.GetValue(posx) as string;

            // Make names nicer to read (but won't exactly match enum definition).
            enum_label = ObjectNames.NicifyVariableName(enum_label);
            label = new GUIContent(enum_label);

         GUIStyle overflowStyle = new GUIStyle();
        overflowStyle.clipping = TextClipping.Overflow;
        GUILayoutOption w8 = GUILayout.Width(8);
        GUILayoutOption w25 = GUILayout.Width(25);
        GUILayoutOption xw = GUILayout.ExpandWidth(true);
        
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("My Ints");
        EditorGUILayout.LabelField("A", overflowStyle, w8);
        EditorGUILayout.PropertyField(prop.FindPropertyRelative("audioClips"), GUIContent.none, w25, xw);
        EditorGUILayout.LabelField("B", overflowStyle, w8);
        EditorGUILayout.PropertyField(prop.FindPropertyRelative("volume"), GUIContent.none, w25, xw);
        EditorGUILayout.EndHorizontal();
     }
 }*/

/*
[CustomPropertyDrawer(typeof(CustomArrayAttribute))]
     public class ResourceReferenceDrawer : PropertyDrawer
 {
     List<string> debugList = new List<string>();
     int index = 0;
     public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        // Properly configure height for expanded contents.
        return EditorGUI.GetPropertyHeight(property, label, property.isExpanded);
    }
     public override void OnGUI (Rect pos,SerializedProperty prop,GUIContent label)
     {         
         UnityEngine.Object newObj = null;

            var config = attribute as NamedArrayAttribute;
            var enum_names = System.Enum.GetNames(config.TargetEnum);
            int posx = int.Parse(prop.propertyPath.Split('[', ']')[1]);
            var enum_label = enum_names.GetValue(posx) as string;

            // Make names nicer to read (but won't exactly match enum definition).
            enum_label = ObjectNames.NicifyVariableName(enum_label);
            label = new GUIContent(enum_label);

        if(prop.isArray && index > 0){
             while(index > prop.arraySize)
                 prop.InsertArrayElementAtIndex(prop.arraySize);
             while(index < prop.arraySize)
                 prop.DeleteArrayElementAtIndex(prop.arraySize-1);
         }
         
         if(prop.arraySize>0){
             for(int i = 0; i<prop.arraySize;i++){
                 UnityEngine.Object obj = AssetDatabase.LoadMainAssetAtPath (prop.GetArrayElementAtIndex(i).stringValue);
                 newObj = EditorGUI.ObjectField (pos, label, obj, typeof(GameObject), false);
                 EditorGUI.FloatField(pos, 1f);
                 if (obj != newObj) {
                     prop.GetArrayElementAtIndex(i).stringValue = AssetDatabase.GetAssetPath (newObj);
                 }
                 debugList.Add(prop.GetArrayElementAtIndex(i).stringValue);
                 pos.y += 20;
             }
         }
         if(!prop.isArray || prop.arraySize == 0){
             
             newObj = EditorGUI.ObjectField (pos, label, newObj, typeof(GameObject), false);
             if (newObj != null) {
                 prop.InsertArrayElementAtIndex(0);
                 prop.GetArrayElementAtIndex(0).stringValue = AssetDatabase.GetAssetPath (newObj);
             }            
         }
         foreach(string s in debugList)
             Debug.Log(s);
         debugList.Clear();
     }
 }*/

public class TestDrawer : MonoBehaviour
{
    [CustomArray(typeof(sfx))] public sfxContainer[] audioClips;
}
