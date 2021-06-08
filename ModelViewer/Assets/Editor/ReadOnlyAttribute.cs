using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;



[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyAttributeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
    {
        Rect textFieldPosition = position;
        textFieldPosition.height = 16;
        EditorGUI.LabelField(position, label, new GUIContent(prop.stringValue));
    }
}