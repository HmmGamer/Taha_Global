using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ConditionalEnumAttribute : PropertyAttribute
{
    public string _enumField;
    public HashSet<int> _targetEnumValues;

    public ConditionalEnumAttribute(string enumFieldName, params int[] targetEnumValues)
    {
        _enumField = enumFieldName;
        _targetEnumValues = new HashSet<int>(targetEnumValues);
    }
}
#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ConditionalEnumAttribute), true)]
public class ConditionalEnumDrawer : PropertyDrawer
{
    private SerializedProperty _cachedEnumProperty;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ConditionalEnumAttribute attribute = (ConditionalEnumAttribute)this.attribute;

        if (_cachedEnumProperty == null || _cachedEnumProperty.serializedObject != property.serializedObject)
        {
            _cachedEnumProperty = GetEnumProperty(property, attribute._enumField);
        }

        if (_cachedEnumProperty == null || !attribute._targetEnumValues.Contains(_cachedEnumProperty.enumValueIndex))
        {
            return;
        }

        EditorGUI.PropertyField(position, property, label, true);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ConditionalEnumAttribute attribute = (ConditionalEnumAttribute)this.attribute;

        if (_cachedEnumProperty == null || _cachedEnumProperty.serializedObject != property.serializedObject)
        {
            _cachedEnumProperty = GetEnumProperty(property, attribute._enumField);
        }

        if (_cachedEnumProperty == null || !attribute._targetEnumValues.Contains(_cachedEnumProperty.enumValueIndex))
        {
            return 0;
        }

        return EditorGUI.GetPropertyHeight(property, true);
    }

    private SerializedProperty GetEnumProperty(SerializedProperty property, string enumField)
    {
        string path = property.propertyPath.Replace(property.name, enumField);
        return property.serializedObject.FindProperty(path);
    }
}
#endif