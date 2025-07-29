using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
[CustomEditor(typeof(UnityEngine.Object), true, isFallback = true)]
public class ButtonEditor : Editor
{
    // Cached button data structure for better memory layout
    private struct ButtonData
    {
        public readonly MethodInfo method;
        public readonly GUIContent content;
        public readonly GUILayoutOption[] options;
        public readonly bool requiresDirty;

        public ButtonData(MethodInfo method, CreateButtonAttribute attribute)
        {
            this.method = method;
            this.content = new GUIContent(attribute.ButtonName);
            this.options = new GUILayoutOption[]
            {
                GUILayout.Width(attribute.Size.x),
                GUILayout.Height(attribute.Size.y)
            };
            // Pre-calculate if target needs to be marked dirty
            var declaringType = method.DeclaringType;
            this.requiresDirty = typeof(MonoBehaviour).IsAssignableFrom(declaringType) ||
                               typeof(ScriptableObject).IsAssignableFrom(declaringType);
        }
    }

    // Static cache using more efficient data structure
    private static readonly Dictionary<Type, ButtonData[]> typeButtonCache =
        new Dictionary<Type, ButtonData[]>();

    private ButtonData[] cachedButtons;
    private bool hasButtons = false;

    private void OnEnable()
    {
        CacheButtonMethods();
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (hasButtons)
        {
            DrawButtons();
        }
    }

    private void CacheButtonMethods()
    {
        var targetType = target.GetType();

        // Check static cache first
        if (typeButtonCache.TryGetValue(targetType, out cachedButtons))
        {
            hasButtons = cachedButtons.Length > 0;
            return;
        }

        // Build button data for this type
        var buttonList = new List<ButtonData>();

        // More efficient: traverse inheritance hierarchy manually to avoid duplicate method scanning
        var currentType = targetType;
        var processedMethods = new HashSet<string>(); // Avoid processing overridden methods multiple times

        while (currentType != null && currentType != typeof(UnityEngine.Object))
        {
            // Use DeclaredOnly to avoid scanning inherited methods multiple times
            var methods = currentType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic |
                                               BindingFlags.Instance | BindingFlags.DeclaredOnly);

            foreach (var method in methods)
            {
                // Skip if we've already processed this method signature
                if (!processedMethods.Add(method.Name))
                    continue;

                // Quick parameter check before attribute lookup
                if (method.GetParameters().Length != 0)
                    continue;

                var attribute = method.GetCustomAttribute<CreateButtonAttribute>();
                if (attribute != null)
                {
                    buttonList.Add(new ButtonData(method, attribute));
                }
            }

            currentType = currentType.BaseType;
        }

        // Convert to array for better cache performance
        cachedButtons = buttonList.ToArray();
        typeButtonCache[targetType] = cachedButtons;
        hasButtons = cachedButtons.Length > 0;
    }

    private void DrawButtons()
    {
        for (int i = 0; i < cachedButtons.Length; i++)
        {
            var buttonData = cachedButtons[i];

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button(buttonData.content, buttonData.options))
            {
                InvokeMethod(buttonData);
            }

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }
    }

    private void InvokeMethod(ButtonData buttonData)
    {
        try
        {
            buttonData.method.Invoke(target, null);

            // Use pre-calculated dirty check
            if (buttonData.requiresDirty)
            {
                EditorUtility.SetDirty(target);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to invoke method {buttonData.method.Name}: {ex.Message}\n{ex.StackTrace}");
        }
    }

    // Clean up static cache when Unity recompiles
    [InitializeOnLoadMethod]
    private static void ClearCacheOnRecompile()
    {
        typeButtonCache.Clear();
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