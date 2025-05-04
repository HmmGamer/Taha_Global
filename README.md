# Taha_Global

# 🚀 Unity Utility Toolkit  

A collection of **essential C# utility scripts** for Unity game development, designed to **simplify common tasks** and **avoid hardcoding** produced by **Taha Mirheidari**

## 📦 Attributes
| Name | Script | Description |
|------|--------|-------------|
| **🔖 Conditional Field** | [`Attr_ConditionField.cs`](#ConditionField) | show or hide a field in the inspector based on a bool |
| **🔖 Conditional Enum** | [`Attr_ConditionEnum.cs`](#ConditionEnum) | show or hide a field based on the value of an enum |
| **🔖 Create Buttons** | [`Attr_CreateButton.cs`](#CreateButton) | generate a button in the inspector to invoke a method or lambda |
| **🔖 Read Only Field** | [`Attr_ReadOnly.cs`](#ReadOnly) | make a field read only and grey for debug or visual purposes |

## 📦 Features & Static Tools  
| Category | Script | Description |
|----------|--------|-------------|
| **🔖 Constants** | [`A.cs`](#a) | Centralized tags, layers, and animation names |
| **🔄 Arrays** | [`AA.cs`](#aa) / [`ArraysTools.cs`](#arraytools) | Array operations (comparison, sum, zeroing) |
| **⏱️ Time** | [`TimeTools.cs`](#timetools) | Time conversion (H:M:S) and countdown timers |
| **💾 Saving** | [`SaveTools.cs`](#savetools) | Save/Load arrays, lists, and ScriptableObjects |
| **🆔 Unique IDs** | [`UniqueIdTools.cs`](#uniqueidtools) | Generate scene-specific unique IDs based on position|
| **🧩 Pooling** | [`PoolManager.cs`](#poolmanager) | Object pooling for optimal performance |
| **🧮 Vectors** | [`VectorsAndQuaTools.cs`](#vectortools) | Vector/Quaternion utilities |
| **📜 Enums** | [`EnumGenerator.cs`](#enumgenerator) | Auto-generate enums based on fields of strings |

## 📦  Features & dynamic Tools  
| Name | Script | Description |
|------|--------|-------------|
| **🔖 MessageBox** | [`MessageBoxManager.cs`](#MessageBox) | a complete and efficient tool for showing confirmation and pop-ups without duplicating canvases |
| **🔖 Raycast Debugger** | [`UiRaycastDebuger.cs`](#RaycastDebuger) | a debuging tool for times you dont know what is the problem with your buttons and canvases |
| **🔖 EventController** | [`EventController.cs`](#EventController) | a simple yet practical tool for debugs and handling timeline events without signals |

## 📄 Script Details
ConditionalField : with this attribute you can make you inspector super clean by hiding the unwanted fields using a single bool in the inspector.

Note : you can use this attribute in almost any condition ( like nested classes or Scriptable Objects ) the only limitation is that it cant be used on lists/arrays.
( video is placed in here )
how to use :
------------------------------------
public class Sample : MonoBehaviour
{
    [SerializeField] bool _showFields;
    [SerializeField, ConditionField(nameof(_showFields))] float _field1;
    [SerializeField, ConditionField(nameof(_showFields))] Vector3 _field2;

    [SerializeField] bool _hideFields;
    [SerializeField, ConditionField(nameof(_hideFields), true)] GameObject _field3;
}
------------------------------------

ConditionEnum : with this attribute you can make you inspector super clean by hiding the unwanted fields using a single enum in the inspector.

Note : you can use this attribute in almost any condition ( like nested classes or Scriptable Objects ) the only limitation is that it cant be used on lists/arrays.
( video is placed in here )
how to use :
------------------------------------
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
------------------------------------

CreateButton : with this attribute you can generate a button in the inspector to invoke a method in both inside and outside the play mode

Note : you can invoke any method inside the play mode but some methods wont work outside the play mode like ( getting and setting for the PlayerPrfs )
( video is placed in here )
how to use :
------------------------------------
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
------------------------------------

CreateButton : with this attribute you can disable and grey out a field in the inspector for both visual and debugging purposes 
( video is placed in here )
how to use :
------------------------------------
public class Sample : MonoBehaviour
{
    [SerializeField] float _normalField;
    [SerializeField, ReadOnly] float _readOnlyField = 3;
}
------------------------------------


📜 License
MIT License - Free to use and modify

💬 Contribute
Found a bug? Want to improve something? Open a PR!

🎮 Happy Coding! 🚀
