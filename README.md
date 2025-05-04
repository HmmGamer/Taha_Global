# 🚀 Unity Utility Toolkit by Taha Mirheidari

A comprehensive collection of **essential C# utility scripts** for Unity game development, designed to **simplify common tasks** and **avoid hardcoding**.

![GitHub](https://img.shields.io/github/license/taha-mirheidari/unity-utility-toolkit?color=blue) 
![GitHub stars](https://img.shields.io/github/stars/taha-mirheidari/unity-utility-toolkit?style=social)

## 📦 Features Overview

### 🔧 Attributes
| Attribute | Description | Example |
|-----------|-------------|---------|
| [**`ConditionField`**](#conditionfield) | Show/hide fields based on bool values | `[ConditionField(nameof(_showField))]` |
| [**`ConditionEnum`**](#conditionenum) | Show/hide fields based on enum values | `[ConditionEnum(nameof(_mode), (int)Mode.Advanced)]` |
| [**`CreateButton`**](#createbutton) | Create inspector buttons to invoke methods | `[CreateButton("Save Data")]` |
| [**`ReadOnly`**](#readonly) | Make fields non-editable in inspector | `[ReadOnly] public float score;` |

### 🛠️ Static Tools
| Category | Description | Key Features |
|----------|-------------|--------------|
| [**Constants**](#a) | Centralized game tags/layers | `A.Tags.player`, `A.Layers.ui` |
| [**Array Tools**](#array-tools) | Advanced array operations | Comparison, summation, filtering |
| [**Time Tools**](#time-tools) | Time conversion utilities | `TimeTools.TotalStringTime(seconds)` |

### 🎮 Dynamic Tools
| Tool | Description | Use Case |
|------|-------------|----------|
| [**MessageBox**](#messagebox) | Popup/confirmation system | Player notifications |
| [**Raycast Debugger**](#raycast-debugger) | UI interaction analyzer | Fixing non-clickable buttons |
| [**EventController**](#eventcontroller) | Timeline event handler | Cutscene triggers |

## 📚 Detailed Documentation

### <a name="conditionfield"></a>🔷 `ConditionField` Attribute
Make your inspector cleaner by hiding unwanted fields conditionally.

**Features**:
✔ Works with nested classes and ScriptableObjects  
✔ Supports boolean inversion  
❌ Doesn't work with lists/arrays  

```csharp
public class Character : MonoBehaviour
{
    [SerializeField] bool showStats;
    
    [ConditionField(nameof(showStats))]
    [SerializeField] float health;
    
    [ConditionField(nameof(showStats))]
    [SerializeField] float stamina;
}

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
