using UnityEditor;
using UnityEngine;
using System;


[CustomPropertyDrawer(typeof(InventoryCategoryId))]
public class InventoryCategoryIdProperty : PropertyDrawer
{    
    private static string[] options = null;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {        
        if (options == null)
        {
            string[] guids1 = AssetDatabase.FindAssets("InventoryCategoryList t:InventoryCategoryList", null);
            
            if (guids1.Length >= 1)
            {
                var settings = (InventoryCategoryList)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guids1[0]), typeof(InventoryCategoryList));
                if (settings != null)
                {
                    options = settings._categories;
                }
            }
        }
        if (options == null)
        {
            Debug.LogError("File not found InventoryCategoryList");
            return;
        }
        
        var propertyId = property.FindPropertyRelative("_id");
        if (propertyId == null)
        {
            Debug.LogError("Didn't find subproperty _id");
            return;            
        }

        int index = Mathf.Max(0, Array.IndexOf(options, propertyId.stringValue));

        index = EditorGUI.Popup(position, label.text, index, options);

        propertyId.stringValue = options[index];
    }

}