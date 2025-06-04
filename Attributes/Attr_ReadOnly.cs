using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public class ReadOnlyAttribute : PropertyAttribute
{
    public enum ReadOnlyMode
    {
        Always,           // Always read-only
        EditModeOnly,     // Read-only only in edit mode
        PlayModeOnly,     // Read-only only in play mode
        WhenNotSelected   // Read-only when object is not selected
    }

    public ReadOnlyMode Mode { get; }

    public ReadOnlyAttribute(ReadOnlyMode mode = ReadOnlyMode.Always)
    {
        Mode = mode;
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    private static readonly HashSet<UnityEngine.Object> _selectedObjects = new HashSet<UnityEngine.Object>();
    private static bool _cacheInitialized = false;

    // Initialize selection cache when first needed
    private static void EnsureCacheInitialized()
    {
        if (!_cacheInitialized)
        {
            Selection.selectionChanged += UpdateSelectionCache;
            UpdateSelectionCache();
            _cacheInitialized = true;
        }
    }

    private static void UpdateSelectionCache()
    {
        _selectedObjects.Clear();

        if (Selection.objects != null)
        {
            foreach (var obj in Selection.objects)
            {
                if (obj != null)
                {
                    _selectedObjects.Add(obj);
                }
            }
        }
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EnsureCacheInitialized();

        var readOnlyAttr = (ReadOnlyAttribute)attribute;
        bool shouldBeReadOnly = ShouldBeReadOnly(property, readOnlyAttr);

        // Store original GUI state
        var originalEnabled = GUI.enabled;
        var originalColor = GUI.color;

        // Apply read-only styling
        if (shouldBeReadOnly)
        {
            GUI.enabled = false;
            GUI.color = new Color(0.8f, 0.8f, 0.8f, 1f); // Slightly grayed out
        }

        // Draw the property field
        EditorGUI.PropertyField(position, property, label, true);

        // Restore original GUI state
        GUI.enabled = originalEnabled;
        GUI.color = originalColor;
    }

    private bool ShouldBeReadOnly(SerializedProperty property, ReadOnlyAttribute readOnlyAttr)
    {
        switch (readOnlyAttr.Mode)
        {
            case ReadOnlyAttribute.ReadOnlyMode.Always:
                return true;

            case ReadOnlyAttribute.ReadOnlyMode.EditModeOnly:
                return !Application.isPlaying;

            case ReadOnlyAttribute.ReadOnlyMode.PlayModeOnly:
                return Application.isPlaying;

            case ReadOnlyAttribute.ReadOnlyMode.WhenNotSelected:
                return !IsObjectSelected(property);

            default:
                return true;
        }
    }

    private bool IsObjectSelected(SerializedProperty property)
    {
        if (property?.serializedObject?.targetObject == null)
            return false;

        var targetObject = property.serializedObject.targetObject;

        // Check if the target object itself is selected
        if (_selectedObjects.Contains(targetObject))
            return true;

        // For nested objects, check if parent GameObjects are selected
        if (targetObject is Component component && component.gameObject != null)
        {
            return _selectedObjects.Contains(component.gameObject);
        }

        return false;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // Support for complex properties (arrays, custom classes, etc.)
        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    // Clean up when Unity recompiles
    [UnityEditor.Callbacks.DidReloadScripts]
    private static void OnScriptsReloaded()
    {
        _cacheInitialized = false;
        _selectedObjects.Clear();
    }
}

// Example usage classes for testing
[System.Serializable]
public class NestedData
{
    [ReadOnly] public int readOnlyInt = 42;
    [ReadOnly(ReadOnlyAttribute.ReadOnlyMode.EditModeOnly)] public string editModeOnly = "Edit Mode Only";
    [ReadOnly(ReadOnlyAttribute.ReadOnlyMode.PlayModeOnly)] public bool playModeOnly = true;
    [ReadOnly(ReadOnlyAttribute.ReadOnlyMode.WhenNotSelected)] public float whenNotSelected = 3.14f;
    public string normalField = "Editable";
}

public class TestReadOnlyComponent : MonoBehaviour
{
    [ReadOnly] public int alwaysReadOnly = 100;
    [ReadOnly(ReadOnlyAttribute.ReadOnlyMode.EditModeOnly)] public string editModeReadOnly = "Can't edit in Edit Mode";
    [ReadOnly(ReadOnlyAttribute.ReadOnlyMode.PlayModeOnly)] public string playModeReadOnly = "Can't edit in Play Mode";
    [ReadOnly(ReadOnlyAttribute.ReadOnlyMode.WhenNotSelected)] public string selectionBasedReadOnly = "Read-only when not selected";

    [Header("Nested Class Support")]
    public NestedData nestedData = new NestedData();

    [Header("Array Support")]
    [ReadOnly] public int[] readOnlyArray = { 1, 2, 3, 4, 5 };

    [Header("Complex Types")]
    [ReadOnly] public Vector3 readOnlyVector = Vector3.one;
    [ReadOnly] public Color readOnlyColor = Color.red;
}
#endif