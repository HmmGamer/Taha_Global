using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class AA
{
    public static class Arrays
    {
        public static bool _ArrayEqual(int[] first, int[] second)
        {
            if (first.Length > 0 && second.Length > 0)
            {
                for (int i = 0; i < first.Length; i++)
                {
                    if (first[i] != second[i])
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }
        public static int _ArrayZeroLength(int[] array)
        {
            int i = 0;
            foreach (int item in array)
            {
                if (!item.Equals(0))
                    i++;
            }
            return i - array.Length;
        }
        public static int _ArraySum(int[] array)
        {
            int _sum = 0;
            if (array.Length > 0)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    _sum += array[i];
                }
            }
            return 0;
        }
        public static int[] _MakeZeroArray(int[] first)
        {
            for (int i = 0; i < first.Length; i++)
            {
                first[i] = 0;
            }
            return first;
        }
    }
    public static class Random
    {
                public static int _RandomNumber(_RandomStruct iRandomType)
        {
            string iTime = Time.time.ToString().Replace('.', '0');

            //_UniqueTheRandomNumber(ref iTime, _RandomStruct._0_5);

            if (iRandomType == _RandomStruct._0_2)
            {
                float fractionalPart = Time.time - Mathf.Floor(Time.time);
                int i = Mathf.FloorToInt(fractionalPart * 3);
                if (i == 3) i = 2; // Ensure the upper limit is included
                return i;
            }
            else if (iRandomType == _RandomStruct._0_5)
            {
                float fractionalPart = Time.time - Mathf.Floor(Time.time);
                int i = Mathf.FloorToInt(fractionalPart * 6);
                if (i == 6) i = 5; // Ensure the upper limit is included
                return i;
            }
            else if (iRandomType == _RandomStruct._0_10)
            {
                float fractionalPart = Time.time - Mathf.Floor(Time.time);
                int i = Mathf.FloorToInt(fractionalPart * 10);
                if (i == 10) i = 9; // Ensure the upper limit is included
                return i;
            }
            else if (iRandomType == _RandomStruct._0_100)
            {
                int x = int.Parse(iTime[iTime.Length - 1].ToString());
                int y = int.Parse(iTime[iTime.Length - 2].ToString());
                int i = x + 10 * y;
                return i;
            }
            return -1;
        }
        static void _UniqueTheRandomNumber(ref string exitNum, _RandomStruct iRandomType)
        {

        }
    }
    public static class TimeTool
    {
        public static int[] ArrayTotalTime(float iTime)
        {
            int Hour = 0;
            int minutes = 0;
            int Seconds = 0;
            if (iTime / 3600 > 1)
            {
                Hour = (int)(iTime / 3600);
                iTime -= Hour * 3600;
            }
            if (iTime / 60 > 1)
            {
                minutes = (int)(iTime / 60);
                iTime -= minutes * 60;
            }
            Seconds = (int)iTime;
            int[] _Result = { Hour, minutes, Seconds };
            return _Result;
        }
        public static int TotalHour(float iTime)
        {
            int Hour = 0;
            if (iTime / 3600 > 1)
            {
                Hour = (int)(iTime / 3600);
            }
            return Hour;
        }
        public static int TotalMinutes(float iTime)
        {
            int Hour = 0;
            int minutes = 0;
            if (iTime / 3600 > 1)
            {
                Hour = (int)(iTime / 3600);
                iTime -= Hour * 3600;
            }
            if (iTime / 60 > 1)
            {
                minutes = (int)(iTime / 60);
            }
            return minutes;
        }
        public static int TotalSeconds(float iTime)
        {
            int Hour = 0;
            int minutes = 0;
            int Seconds = 0;
            if (iTime / 3600 > 1)
            {
                Hour = (int)(iTime / 3600);
                iTime -= Hour * 3600;
            }
            if (iTime / 60 > 1)
            {
                minutes = (int)(iTime / 60);
                iTime -= minutes * 60;
            }
            Seconds = (int)iTime;
            return Seconds;
        }
        public static string TotalStringTime(float iTime)
        {
            int Hour = 0;
            int minutes = 0;
            int Seconds = 0;
            if (iTime / 3600 > 1)
            {
                Hour = (int)(iTime / 3600);
                iTime -= Hour * 3600;
            }
            if (iTime / 60 > 1)
            {
                minutes = (int)(iTime / 60);
                iTime -= minutes * 60;
            }
            Seconds = (int)iTime;
            string _Result = System.String.Format("{0} : {1} : {2}", Hour, minutes, Seconds);
            return _Result;
        }
        public static string ReverseTimerString(float iTime, float iTimerTime)
        {
            iTimerTime -= iTime;
            int Hour = 0;
            int minutes = 0;
            int Seconds = 0;
            if (iTimerTime / 3600 > 1)
            {
                Hour = (int)(iTimerTime / 3600);
                iTimerTime -= Hour * 3600;
            }
            if (iTimerTime / 60 > 1)
            {
                minutes = (int)(iTimerTime / 60);
                iTimerTime -= minutes * 60;
            }
            if (iTimerTime <= 0)
            {
                iTimerTime = 0;
            }
            Seconds = (int)iTimerTime;
            string _Result = System.String.Format("{0} : {1} : {2}", Hour, minutes, Seconds);
            return _Result;
        }
        public static int ReverseTimerMin(float iTime, float iTimerTime)
        {
            iTimerTime -= iTime;
            int Hour = 0;
            int minutes = 0;
            if (iTimerTime / 3600 > 1)
            {
                Hour = (int)(iTimerTime / 3600);
                iTimerTime -= Hour * 3600;
            }
            if (iTimerTime / 60 > 1)
            {
                minutes = (int)(iTimerTime / 60);
                iTimerTime -= minutes * 60;
                return minutes;
            }
            return 0;

        }
        public static int ReverseTimerSec(float iTime, float iTimerTime)
        {
            iTimerTime -= iTime;
            int Hour = 0;
            int minutes = 0;
            int Seconds = 0;
            if (iTimerTime / 3600 > 1)
            {
                Hour = (int)(iTimerTime / 3600);
                iTimerTime -= Hour * 3600;
            }
            if (iTimerTime / 60 > 1)
            {
                minutes = (int)(iTimerTime / 60);
                iTimerTime -= minutes * 60;
            }
            if (iTimerTime <= 0)
            {
                iTimerTime = 0;
            }
            Seconds = (int)iTimerTime;
            return Seconds;
        }
    }
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
    }
    public static class Tools
    {
        public static string _MakeUniqueId(Vector2 iPosition)
        {
            string xPart = iPosition.x.ToString("F2").Replace(".", "");
            string yPart = iPosition.y.ToString("F2").Replace(".", "");

            if (xPart.Length > 6)
                xPart = xPart.Substring(0, 5);
            if (yPart.Length > 6)
                yPart = yPart.Substring(0, 5);

            // we add currentScene to ensure the id is unique in different scenes
            int currentScene;
            currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;

            return xPart + yPart + "_" + currentScene;
        }
        public static int _GetUniqueIdScene(string iUniqueId)
        {
            return int.Parse(iUniqueId.Split('_')[1]);
        }
                public static bool _IsUniqueIdInScene(string iUniqueId)
        {
            int uniqueIdScene = _GetUniqueIdScene(iUniqueId);
            return uniqueIdScene == 
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        }
        public static void _DontDestroyOnLoad(GameObject iGameObject)
        {
            while (iGameObject.transform.parent != null)
            {
                iGameObject = iGameObject.transform.parent.gameObject;
            }
            Object.DontDestroyOnLoad(iGameObject);
        }

    }
    public static class Vectors
    {
        public static Vector3 _Vector3Maker(Quaternion quaternion)
        {
            return new Vector3(quaternion.x, quaternion.y, quaternion.z);
        }
        public static Vector3 _VectorMultiplayer(Vector3 input, Vector3 scale)
        {
            input.x *= scale.x;
            input.y *= scale.y;
            input.z *= scale.z;
            return input;
        }
        public static Quaternion _QuaternionPlusVector(Quaternion iRotation, Vector3 iVector3)
        {
            iRotation.x += iVector3.x;
            iRotation.y += iVector3.y;
            iRotation.z += iVector3.z;
            return iRotation;
        }
    }
    static class test
    {
        public static float _GenerateRandomNumber(int iMin, int iMax) // 0 - 99
        {
            string iTime = Time.time.ToString();
            string exitValue;
            if (iMax - iMin < 10)
            {
                return iTime[iTime.Length - 1];
            }
            else if (100 > iMax - iMin && iMax - iMin >= 10)
            {
                exitValue = iTime[iTime.Length - 1].ToString() +
                    iTime[iTime.Length - 2].ToString();
                return float.Parse(exitValue);
            }
            else
            {
                Debug.Log("cant make this random range");
                return 0;
            }
        }
        public static bool _GenerateRandomNumber(int iMin, int iMax, float iChance)
        {
            string iTime = Time.time.ToString();


            return true;
        }
    }
    public enum _RandomStruct
    {
        _0_2, _0_5, _0_10, _0_100
    }
}
