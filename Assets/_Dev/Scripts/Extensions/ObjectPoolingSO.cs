using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Picker.Extensions
{
    [CreateAssetMenu(menuName = "Picker/Extensions/Pooling", fileName = "ObjectPoolData")]
    public class ObjectPoolingSO : ScriptableObject
    {
        [FoldoutGroup("Pooling")]
        public List<GameObject> prefabs; // List of objects for which you want to create a pool.

        [FoldoutGroup("Pooling")] public int poolSize; // Number of elements in the object pool
        [FoldoutGroup("Pooling")] public bool isRandomize;

        private List<GameObject> _objectPool;
        private GameObject _parentObj;
        private int _count = 0;
        
        public void InitializeObjectPool(string parentName)
        {
            GameObject parentObj = new GameObject();
            _parentObj = parentObj;
            _parentObj.name = parentName;

            _objectPool = new List<GameObject>();

            for (int i = 0; i < poolSize; i++)
            {
                GameObject prefabToInstantiate = GetPrefabToInstantiate();
                GameObject obj = Instantiate(prefabToInstantiate, _parentObj.transform);
                obj.SetActive(false);
                _objectPool.Add(obj);
            }
        }

        private GameObject GetPrefabToInstantiate()
        {
            switch (prefabs.Count)
            {
                case 1:
                    // If there's only one prefab, return that.
                    return prefabs[0];
                case > 1:
                {
                    // If there are multiple prefabs, return a random one.
                    if (isRandomize)
                    {
                        int randomIndex = Random.Range(0, prefabs.Count);
                        return prefabs[randomIndex];
                    }
                    else
                    {
                        _count++;
                        return _count <= 0 ? prefabs[0] : prefabs[_count - 1];
                    }
                }
                default:
                    // If the list is empty, return null or handle it accordingly.
                    return null;
            }
        }

        public GameObject GetPooledObject()
        {
            for (int i = 0; i < _objectPool.Count; i++)
            {
                if (!_objectPool[i].activeInHierarchy)
                {
                    return _objectPool[i];
                }
            }

            // If no inactive object is found, create a new object.
            GameObject prefabToInstantiate = GetPrefabToInstantiate();
            GameObject newObj = Instantiate(prefabToInstantiate, _parentObj.transform);
            newObj.SetActive(false);
            _objectPool.Add(newObj);
            return newObj;
        }

        // ReSharper disable Unity.PerformanceAnalysis because of Debug.LogError
        public GameObject GetPoolObjectWithName(string existName)
        {
            for (int i = 0; i < _objectPool.Count; i++)
            {
                if (!_objectPool[i].activeInHierarchy && _objectPool[i].name.Contains(existName))
                {
                    return _objectPool[i];
                }
            }

            Debug.LogError("This object is not exist");
            return null;
        }

        public GameObject GetPoolObjectRandomize()
        {
            // Generate a random index within the pool's range
            int randomIndex = Random.Range(0, _objectPool.Count);
            // Retrieve the GameObject at the random index
            GameObject obj = _objectPool[randomIndex];

            // Check if the selected object is active
            return obj.activeInHierarchy ? GetPoolObjectRandomize() : obj;
        }

        public void ReturnToPool(GameObject obj)
        {
            obj.SetActive(false);
        }

        private void OnDisable()
        {
            _count = 0;
        }
    }
}