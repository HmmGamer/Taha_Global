using System.Collections.Generic;
using UnityEngine;

public static class PoolManager
{
    private static Dictionary<GameObject, Queue<GameObject>> poolDictionary = new Dictionary<GameObject, Queue<GameObject>>();

    public static GameObject _instantiate(GameObject _prefab, Vector3 _position, Quaternion _rotation)
    {
        if (!poolDictionary.ContainsKey(_prefab))
        {
            poolDictionary[_prefab] = new Queue<GameObject>();
        }

        if (poolDictionary[_prefab].Count > 0)
        {
            GameObject _objectToReuse = poolDictionary[_prefab].Dequeue();
            _objectToReuse.transform.position = _position;
            _objectToReuse.transform.rotation = _rotation;
            _objectToReuse.SetActive(true);
            return _objectToReuse;
        }
        else
        {
            GameObject _newObject = Object.Instantiate(_prefab, _position, _rotation);
            _newObject.AddComponent<PooledObject>()._prefab = _prefab;
            return _newObject;
        }
    }

    public static void _despawn(GameObject _obj)
    {
        if (_obj.TryGetComponent(out PooledObject _pooledObj))
        {
            _obj.SetActive(false);
            if (!poolDictionary.ContainsKey(_pooledObj._prefab))
            {
                poolDictionary[_pooledObj._prefab] = new Queue<GameObject>();
            }
            poolDictionary[_pooledObj._prefab].Enqueue(_obj);
        }
        else
        {
            Object.Destroy(_obj);
        }
    }

    private class PooledObject : MonoBehaviour
    {
        public GameObject _prefab;
    }
}
