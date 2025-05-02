using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ReadOnlyAttribute : PropertyAttribute
{
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    private static HashSet<Object> _selectedObjects = new HashSet<Object>();

    static ReadOnlyDrawer()
    {
        Selection.selectionChanged += _UpdateSelectionCache;
        _UpdateSelectionCache();
    }

    private static void _UpdateSelectionCache()
    {
        _selectedObjects.Clear();
        foreach (var obj in Selection.objects)
        {
            _selectedObjects.Add(obj);
        }
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        bool _isVisible = _selectedObjects.Contains(property.serializedObject.targetObject);

        using (new EditorGUI.DisabledScope(!_isVisible))
        {
            EditorGUI.PropertyField(position, property, label);
        }
    }
}
#endif
