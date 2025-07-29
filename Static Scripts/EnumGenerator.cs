using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
class _Comments
{
    //[SerializeField] _Test[] tests;

    //[CreateButton("Make Enum")]
    //private void _MakeEnum()
    //{
    //    EnumGenerator.GenerateEnums("testEnum", tests, nameof(_Test._name));
    //    EnumGenerator.AddValue("testEnum", "None");
    //}
    //[Serializable]
    //public class _Test
    //{
    //    public string _name;
    //    public int _value;
    //}
}
public static class EnumGenerator
{
    private const string GENERATION_PATH = "Assets/Others/GeneratedEnums";

    public static void GenerateEnums<T>(string enumName, T[] dataArray, string fieldName) where T : class
    {
#if UNITY_EDITOR
        if (Application.isPlaying)
        {
            Debug.LogError("Enum Generator Only Works in Editor");
            return;
        }
        if (dataArray == null || dataArray.Length == 0)
        {
            Debug.LogWarning("Data array is empty or null.");
            return;
        }
        Directory.CreateDirectory(GENERATION_PATH);
        string filePath = Path.Combine(GENERATION_PATH, enumName + ".cs");
        string enumCode = "using System;\n\n[Serializable]\npublic enum " + enumName + "\n{\n";
        for (int i = 0; i < dataArray.Length; i++)
        {
            var field = dataArray[i].GetType().GetField(fieldName);
            if (field == null)
            {
                Debug.LogError($"Class does not contain a field named '{fieldName}'.");
                return;
            }
            string enumValue = field.GetValue(dataArray[i]) as string;
            if (string.IsNullOrEmpty(enumValue))
            {
                Debug.LogWarning($"{fieldName} value is null or empty at index {i}.");
                continue;
            }
            enumValue = enumValue.Replace(" ", "_").Replace("-", "_"); // Remove spaces and invalid characters
            enumCode += "    " + enumValue + (i < dataArray.Length - 1 ? "," : "") + "\n";
        }
        enumCode += "}";
        File.WriteAllText(filePath, enumCode);
        AssetDatabase.Refresh();
#endif
    }

    public static void AddValue(string enumName, string newValue)
    {
#if UNITY_EDITOR
        if (Application.isPlaying)
        {
            Debug.LogError("Enum Generator Only Works in Editor");
            return;
        }

        if (string.IsNullOrEmpty(newValue))
        {
            Debug.LogWarning("New enum value cannot be null or empty.");
            return;
        }

        string filePath = Path.Combine(GENERATION_PATH, enumName + ".cs");

        if (!File.Exists(filePath))
        {
            Debug.LogError($"Enum file '{enumName}.cs' does not exist. Generate the enum first.");
            return;
        }

        string fileContent = File.ReadAllText(filePath);

        // Clean the new value (same rules as in GenerateEnums)
        string cleanValue = newValue.Replace(" ", "_").Replace("-", "_");

        // Check if the value already exists
        if (fileContent.Contains("    " + cleanValue))
        {
            Debug.LogWarning($"Enum value '{cleanValue}' already exists in {enumName}.");
            return;
        }

        // Find the last enum value and add the new one
        int lastBraceIndex = fileContent.LastIndexOf('}');
        if (lastBraceIndex == -1)
        {
            Debug.LogError($"Invalid enum file format for '{enumName}.cs'.");
            return;
        }

        // Find the opening brace to insert at the beginning
        int openingBraceIndex = fileContent.IndexOf("{\n");
        if (openingBraceIndex == -1)
        {
            Debug.LogError($"Invalid enum file format for '{enumName}.cs'.");
            return;
        }

        string beforeEnum = fileContent.Substring(0, openingBraceIndex + 2); // Include "{\n"
        string afterOpening = fileContent.Substring(openingBraceIndex + 2);
        string beforeClosingBrace = afterOpening.Substring(0, afterOpening.LastIndexOf('}')).TrimEnd();

        string newContent;
        if (string.IsNullOrWhiteSpace(beforeClosingBrace))
        {
            // Empty enum, add first value
            newContent = beforeEnum + "    " + cleanValue + "\n}";
        }
        else
        {
            // Add new value at the beginning and comma after it
            newContent = beforeEnum + "    " + cleanValue + ",\n" + beforeClosingBrace + "\n}";
        }

        File.WriteAllText(filePath, newContent);
        AssetDatabase.Refresh();

        Debug.Log($"Added '{cleanValue}' to enum '{enumName}'.");
#endif
    }
}