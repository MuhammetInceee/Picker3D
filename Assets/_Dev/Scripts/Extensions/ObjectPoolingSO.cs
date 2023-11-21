using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Picker.Extensions
{
    [CreateAssetMenu(menuName = "Picker/Extensions/Pooling", fileName = "ObjectPoolData")]
    public class ObjectPoolingSO : ScriptableObject
    {
        [FoldoutGroup("Pooling")] public List<GameObject> prefabs; // List of objects for which you want to create a pool.
        [FoldoutGroup("Pooling")] public int poolSize = 10; // Number of elements in the object pool

        private List<GameObject> _objectPool;
        private GameObject _parentObj;

        public void InitializeObjectPool<T>(T clas)
        {
            GameObject parentObj = new GameObject();
            _parentObj = parentObj;
            string className = clas.ToString().Replace($"({clas.GetType().ToString()})", "");
            _parentObj.name = $"{className}Pooling";

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
                    int randomIndex = Random.Range(0, prefabs.Count);
                    return prefabs[randomIndex];
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

        public void ReturnToPool(GameObject obj)
        {
            obj.SetActive(false);
        }
    }
}