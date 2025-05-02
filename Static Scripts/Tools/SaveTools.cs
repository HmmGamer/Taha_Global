using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class SaveTools
{
    #region Array
    public static void _SaveArrayToDisk<T>(ref T[] _array, string _fileName)
    {
        string filePath = Application.persistentDataPath + "/" + _fileName;
        string jsonData = JsonUtility.ToJson(new _ArrayContainer<T>(_array), true);
        File.WriteAllText(filePath, jsonData);
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.SaveAssets();
#endif
    }
    public static void _LoadArrayFromDisk<T>(ref T[] _array, string _fileName)
    {
        string filePath = Application.persistentDataPath + "/" + _fileName;
        if (File.Exists(filePath))
        {
            string loadedData = File.ReadAllText(filePath);
            _array = JsonUtility.FromJson<_ArrayContainer<T>>(loadedData)._dataArray;
        }
    }
    [System.Serializable]
    private class _ArrayContainer<T>
    {
        public T[] _dataArray;

        public _ArrayContainer(T[] dataArray)
        {
            _dataArray = dataArray;
        }
    }
    #endregion
    #region List
    public static void _SaveListToDisk<T>(ref List<T> _list, string _fileName)
    {
        string filePath = Application.persistentDataPath + "/" + _fileName;
        string jsonData = JsonUtility.ToJson(new _ListContainer<T>(_list), true);
        File.WriteAllText(filePath, jsonData);
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.SaveAssets();
#endif
    }
    public static void _LoadListFromDisk<T>(ref List<T> _list, string _fileName)
    {
        string filePath = Application.persistentDataPath + "/" + _fileName;
        if (File.Exists(filePath))
        {
            string loadedData = File.ReadAllText(filePath);
            _list = JsonUtility.FromJson<_ListContainer<T>>(loadedData)._dataList;
        }
    }
    [System.Serializable]
    private class _ListContainer<T>
    {
        public List<T> _dataList;

        public _ListContainer(List<T> dataList)
        {
            _dataList = dataList;
        }
    }
    #endregion
    #region ScriptableObjects
    // this is used to save/load scriptable objects in runtime
    public static void _SaveSOToDisk(ScriptableObject _target, string _fileName = null)
    {
        if (_fileName == null)
        {
            _fileName = _target.name + ".json";
        }
        else if (!_fileName.EndsWith(".json"))
        {
            _fileName += ".json";
            return;
        }

        string jsonData = JsonUtility.ToJson(_target, true);
        string filePath = Path.Combine(Application.persistentDataPath, _fileName);
        File.WriteAllText(filePath, jsonData);
#if UNITY_EDITOR
        AssetDatabase.SaveAssets();
#endif
    }
    public static void _LoadSOFromDisk(ScriptableObject _target, string _fileName)
    {
        if (_fileName == null)
        {
            _fileName = _target.name + ".json";
        }
        else if (!_fileName.EndsWith(".json"))
        {
            _fileName += ".json";
            return;
        }

        string filePath = Path.Combine(Application.persistentDataPath, _fileName);
        if (File.Exists(filePath))
        {
            string loadedData = File.ReadAllText(filePath);
            JsonUtility.FromJsonOverwrite(loadedData, _target);
        }
    }
    #endregion
}
