using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
/// <summary>
/// This attribute generates a button in the inspector that invokes the linked method directly when clicked.
/// </summary>
[CustomEditor(typeof(UnityEngine.Object), true, isFallback = true)]
public class ButtonEditor : Editor
{
    private Dictionary<string, MethodInfo> buttonMethods = new Dictionary<string, MethodInfo>();
    private bool buttonClicked = false;
    private string buttonToInvoke = string.Empty;

    public override void OnInspectorGUI()
    {
        // Base Inspector GUI
        DrawDefaultInspector();

        // Cache methods once
        if (buttonMethods.Count == 0)
        {
            CacheButtonMethods();
        }

        // Render buttons for each method
        foreach (var buttonMethod in buttonMethods)
        {
            var buttonName = buttonMethod.Key;

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(buttonName, GUILayout.Width(250), GUILayout.Height(40)))
            {
                buttonClicked = true;
                buttonToInvoke = buttonName;
                EditorApplication.update += HandleButtonClicks; // Subscribe only when needed
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
    }

    private void CacheButtonMethods()
    {
        var methods = target.GetType().GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        foreach (var method in methods)
        {
            var attribute = method.GetCustomAttribute<CreateButtonAttribute>();
            if (attribute != null)
            {
                buttonMethods.Add(attribute.ButtonName, method);
            }
        }
    }

    private void HandleButtonClicks()
    {
        if (buttonClicked && !string.IsNullOrEmpty(buttonToInvoke) && buttonMethods.ContainsKey(buttonToInvoke))
        {
            var method = buttonMethods[buttonToInvoke];
            try
            {
                method.Invoke(target, null);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to invoke method {method.Name}: {ex.Message}");
            }
            finally
            {
                buttonClicked = false; // Reset after invocation
                buttonToInvoke = string.Empty; // Clear the method name
                EditorApplication.update -= HandleButtonClicks; // Unsubscribe immediately after handling
            }
        }
    }
}
#endif

/// <summary>
/// Attribute to create a button in the inspector for any Unity Object (ScriptableObject or MonoBehaviour).
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class CreateButtonAttribute : Attribute
{
    public string ButtonName { get; }

    public CreateButtonAttribute(string buttonName)
    {
        ButtonName = buttonName;
    }
}
