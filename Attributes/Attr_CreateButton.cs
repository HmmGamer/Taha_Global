using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System.Drawing;


#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
[CustomEditor(typeof(UnityEngine.Object), true, isFallback = true)]
public class ButtonEditor : Editor
{
    private Dictionary<string, (MethodInfo method, CreateButtonAttribute attribute)> buttonMethods = new Dictionary<string, (MethodInfo, CreateButtonAttribute)>();
    private bool buttonClicked = false;
    private string buttonToInvoke = string.Empty;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (buttonMethods.Count == 0)
        {
            CacheButtonMethods();
        }
        foreach (var buttonMethod in buttonMethods)
        {
            var buttonName = buttonMethod.Key;
            var attribute = buttonMethod.Value.attribute;
            var size = attribute.Size;
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(buttonName, GUILayout.Width(size.x), GUILayout.Height(size.y)))
            {
                buttonClicked = true;
                buttonToInvoke = buttonName;
                EditorApplication.update += HandleButtonClicks;
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
                buttonMethods.Add(attribute.ButtonName, (method, attribute));
            }
        }
    }

    private void HandleButtonClicks()
    {
        if (buttonClicked && !string.IsNullOrEmpty(buttonToInvoke) && buttonMethods.ContainsKey(buttonToInvoke))
        {
            var method = buttonMethods[buttonToInvoke].method;
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
                buttonClicked = false;
                buttonToInvoke = string.Empty;
                EditorApplication.update -= HandleButtonClicks;
            }
        }
    }
}
#endif

[AttributeUsage(AttributeTargets.Method)]
public class CreateButtonAttribute : Attribute
{
    public string ButtonName { get; }
    public Vector2 Size { get; }

    public CreateButtonAttribute(string buttonName, float xSize = 250, float ySize = 30)
    {
        ButtonName = buttonName;
        Size = new Vector2(xSize, ySize);
    }
}
