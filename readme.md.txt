# 🚀 Unity Utility Toolkit  

A collection of **essential C# utility scripts** for Unity game development, designed to **simplify common tasks** and **avoid hardcoding**.  

## 📦 Features & Tools  

| Category | Script | Description |
|----------|--------|-------------|
| **🔖 Constants** | [`A.cs`](#a) | Centralized tags, layers, and animation names |
| **🔄 Arrays** | [`AA.cs`](#aa) / [`ArraysTools.cs`](#arraytools) | Array operations (comparison, sum, zeroing) |
| **⏱️ Time** | [`TimeTools.cs`](#timetools) | Time conversion (H:M:S) and countdown timers |
| **💾 Saving** | [`SaveTools.cs`](#savetools) | Save/Load arrays, lists, and ScriptableObjects |
| **🆔 Unique IDs** | [`UniqueIdTools.cs`](#uniqueidtools) | Generate scene-specific unique IDs |
| **🧩 Pooling** | [`PoolManager.cs`](#poolmanager) | Object pooling for optimal performance |
| **🧮 Vectors** | [`VectorsAndQuaTools.cs`](#vectortools) | Vector/Quaternion utilities |
| **🎲 Random** | [`AA.cs`](#aa) | Custom random number generators |
| **📜 Enums** | [`EnumGenerator.cs`](#enumgenerator) | Auto-generate enums from data |

## 📄 Script Details  

### 🔷 `A.cs`  
**Avoid hardcoded strings!** Stores:  
- **Tags** (`Player`, `Enemy`, etc.)  
- **Layers** (Player, Floor, etc.)  
- **Animation Parameters** (e.g., `"isJumping"`)  

```csharp
// Usage:
if (gameObject.CompareTag(A.Tags.player)) { ... }
🔷 AA.cs
Multi-purpose utilities:

Array Tools: Compare, sum, or reset arrays

Random Numbers: Generate values in ranges (0-2, 0-100)

Time Tools: Convert seconds to H:M:S format

csharp
// Random number between 0-5:
int roll = AA.Random._RandomNumber(_RandomStruct._0_5);
🔷 ArraysTools.cs
Array operations:

_ArrayEqual(): Check if two arrays match

_ArraySum(): Sum all elements (⚠️ Note: Currently returns 0; needs fix)

csharp
if (ArraysTools._ArrayEqual(array1, array2)) { ... }
⏱️ TimeTools.cs
Convert seconds to hours, minutes, seconds:

csharp
string time = TimeTools.TotalStringTime(3665); // "1 : 1 : 5"
💾 SaveTools.cs
Save/Load data (JSON):

Arrays, Lists, ScriptableObjects

csharp
SaveTools._SaveArrayToDisk(ref myArray, "data.json");
🆔 UniqueIdTools.cs
Generate scene-unique IDs from positions:

csharp
string id = UniqueIdTools._MakeUniqueId(transform.position);
🧩 PoolManager.cs
Object pooling for performance:

csharp
// Spawn:
GameObject obj = PoolManager._instantiate(prefab, pos, rot);
// Despawn:
PoolManager._despawn(obj);
🧮 VectorsAndQuaTools.cs
Vector/Quaternion utilities:

csharp
Vector3 scaled = VectorsAndQuaTools._VectorMultiplayer(input, scale);
📜 EnumGenerator.cs (Editor-only)
Auto-generate enums from class data:

csharp
EnumGenerator.GenerateEnums("WeaponType", weapons, "name");
🛠️ How to Use
Import scripts into your Unity project

Call methods directly (e.g., A.Tags.player)

For EnumGenerator.cs, use in Editor mode

📜 License
MIT License - Free to use and modify

💬 Contribute
Found a bug? Want to improve something? Open a PR!

⭐ Star this repo if it helps you!
GitHub stars

🔗 Full Code Examples: See each script's header comments for details

🎮 Happy Coding! 🚀