# 🌍 Taha_Global

## 🚀 Unity Utility Toolkit  
A collection of **essential C# utility scripts** for Unity game development, designed to **simplify common tasks** and **eliminate hardcoding**, crafted by **[Taha Mirheidari](https://github.com/your-github-username)**.

---

## 📦 Attributes
| Name | Script | Description |
|------|--------|-------------|
| **🔖 Conditional Field** | [`Attr_ConditionField.cs`](#-conditionalfield) | Show or hide a field in the Inspector based on a bool |
| **🔖 Conditional Enum** | [`Attr_ConditionEnum.cs`](#-conditionalenum) | Show or hide a field based on the value of an enum |
| **🔖 Create Buttons** | [`Attr_CreateButton.cs`](#-createbutton) | Generate a button in the Inspector to invoke a method |
| **🔖 Read Only Field** | [`Attr_ReadOnly.cs`](#-readonly) | Make a field read-only and greyed-out for debug/visual purposes |

---

## ⚙️ Features & Static Tools  
| Category | Script | Description |
|----------|--------|-------------|
| **🔖 Constants** | [`A.cs`] | Centralized tags, layers, and animation names |
| **🔄 Arrays** | [`ArrayTools.cs`] | Array operations (comparison, sum, zeroing) |
| **⏱️ Time** | [`TimeTools.cs`] | Time conversion (H:M:S) and countdown timers |
| **💾 Saving** | [`SaveTools.cs`] | Save/Load arrays, lists, and ScriptableObjects |
| **🆔 Unique IDs** | [`UniqueIdTools.cs`](#uniqueidtools) | Generate scene-specific unique IDs based on position |
| **🧩 Pooling** | [`PoolManager.cs`] | Object pooling for optimal performance |
| **🧮 Vectors** | [`VectorsAndQuaTools.cs`] | Vector and Quaternion utilities |
| **📜 Enums** | [`EnumGenerator.cs`](#enumgenerator) | Auto-generate enums based on string fields |

---

## ⚙️ Features & Dynamic Tools  
| Name | Script | Description |
|------|--------|-------------|
| **🗨️ MessageBox** | [`MessageBoxManager.cs`](#messagebox) | A complete tool for showing confirmation/pop-ups without duplicating canvases |
| **🕵️ Raycast Debugger** | [`UiRaycastDebuger.cs`](#raycastdebugger) | Debug tool to inspect UI raycast issues with buttons/canvases |
| **🎬 EventController** | [`EventController.cs`](#eventcontroller) | Handle timeline events and debug them easily without signals |

---

## 📄 Script Details

### 🔖 ConditionalField  
Hide or show fields in the Inspector based on a single bool, making your Inspector super clean.

**Note:** Usable in nested classes and ScriptableObjects. Cannot be used on lists/arrays.  
![ConditionalField Demo](Github%20Docs/conditional_field_vid.gif)

**Usage:**
```csharp
public class Sample : MonoBehaviour
{
    [SerializeField] bool _showFields;
    [SerializeField, ConditionField(nameof(_showFields))] float _field1;
    [SerializeField, ConditionField(nameof(_showFields))] Vector3 _field2;

    [SerializeField] bool _hideFields;
    [SerializeField, ConditionField(nameof(_hideFields), true)] GameObject _field3;
}
```

### 🔖 ConditionalEnum  
Hide or show fields in the Inspector based on an enum value for better organization.

**Note:** Usable in nested classes and ScriptableObjects. Cannot be used on lists/arrays.  
![ConditionalEnum Demo](Github%20Docs/conditional_enum_vid.gif)

**Usage:**
```csharp
public class Sample : MonoBehaviour
{
    [SerializeField] _AllFields _fields;

    [SerializeField, ConditionalEnum(nameof(_fields), (int)_AllFields.field1)]
    float field1;

    [SerializeField, ConditionalEnum(nameof(_fields), (int)_AllFields.field2_3)]
    float field2;
    
    [SerializeField, ConditionalEnum(nameof(_fields), (int)_AllFields.field2_3)]
    float field3;
}
public enum _AllFields
{
    field1, field2_3, none
}
```

### 🔖 CreateButton  
Generate a button in the Inspector to invoke methods in both play and edit mode.

**Note:** Some methods (like PlayerPrefs access) may only work in play mode.  
![CreateButton Demo](Github%20Docs/create_button_vid.gif)
**Usage:**
```csharp
public class Sample : MonoBehaviour
{
    [CreateButton("Invoke Test 1")]
    public void _Test1()
    {
        Debug.Log("Test1 was invoked!");
    }

    [CreateButton("Invoke Test 2", 220, 40)]
    public void _Test2()
    {
        Debug.Log("Test2 was invoked!");
    }
}
```

### 🔖 ReadOnly  
Disable and grey out a field in the Inspector for visual clarity or debugging.

![ReadOnly Demo](Github%20Docs/readonly%20photo.png)

**Usage:**
```csharp
public class Sample : MonoBehaviour
{
    [SerializeField] float _normalField;
    [SerializeField, ReadOnly] float _readOnlyField = 3;
}
```

---
## 📌 Scene Unique ID Generator

This utility provides a way to automatically generate **unique identifiers** for GameObjects based on their transform position and scene index. It helps eliminate the need for manually assigning IDs in the inspector.

**✅ Features** 
- Generates unique IDs based on:
  - GameObject's X and Y position
  - Parent GameObject name
  - Scene build index
- Ensures IDs are consistent and scene-aware
- Avoids manual management of identifiers

**Usage:**

```csharp
string id = UniqueIdTools._MakeUniqueId(yourGameObject);
```

To check if an ID is from the currently active scene:

```csharp
bool isInScene = UniqueIdTools._IsUniqueIdInScene(id);
```

To extract the scene index from an ID:

```csharp
int sceneIndex = UniqueIdTools._GetUniqueIdScene(id);
```

### ⚠️ Guidelines
- **Do not** attach this ID tool to multiple scripts on the same GameObject or overlapping positions under the same parent—this can lead to duplicate IDs and logical bugs.

---

## 🔧 Enum Generator (Editor Only)

This script provides a streamlined interface to automatically generate enums from structured data directly in the Unity Editor.

### ✅ Features
#### 🗃 Generate Enums from Nested Class
Uses _NestedClass[] array as input.

Generates an enum named _AllTitles using the _enumNames field.

Triggered using the "Generate Nested Class Enums" button in the Inspector.

```csharp
public class Sample : MonoBehaviour
{
    [SerializeField] private _NestedClass[] _data;
    //[SerializeField] private _AllTitles _nestedClassTitles; // uncomment when the enum was generated

    [CreateButton("Generate Nested Class Enums")]
    public void _GenerateNestedClassEnum()
    {
        EnumGenerator.GenerateEnums("_AllTitles", _data, nameof(_NestedClass._enumNames));
    }
}

[System.Serializable]
public class _NestedClass
{
    public string _enumNames;
    public int _showInt;
    // Add more fields if needed
}
```
#### 🧾 Generate Enums from String Array
Uses a plain string[] array.

Generates an enum named _AllTitles2 using the string values as entries.

Triggered using the "Generate String Array Enums" button in the Inspector.

```csharp
public class Sample : MonoBehaviour
{
    [SerializeField] private string[] _enumNames2;
    //[SerializeField] private _AllTitles2 _showEnumValues; // uncomment when the enum was generated

    [CreateButton("Generate String Array Enums")]
    public void _GenerateStringArrayEnum()
    {
        EnumGenerator.GenerateEnums("_AllTitles2", _enumNames2);
    }
}
```
### 📌 Requirements

Custom [CreateButton] attribute must be present in your project for Inspector buttons.

### ⚠️ Notes
- Must be run **only in the Unity Editor**
- Will not work during play mode
- Handles basic name sanitization (spaces and dashes replaced with underscores)
- For accessing the enum first generate it in the inspector
- ![EnumGenerator Demo](Github%20Docs/EnumGenerator_Vid.gif)


## ✨ How to use
**installation:** Directly add/clone the folder to your game (recommended folder: Scripts)

Attributes: Add them directly on your fields or methods (as shown in usage examples).

Tools: Call static methods from utility classes when needed.

Dynamic Tools: Add the prefabs/managers to your scenes as needed.

## 📜 License
MIT License — Free to use and modify

## 💬 Contribute
Found a bug? Want to improve something? Open a PR!

🎮 **Happy Coding! 🚀**
