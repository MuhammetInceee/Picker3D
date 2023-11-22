using Picker.Extensions;
using UnityEngine;

namespace Picker.Managers
{
    public class PoolingManager : MonoBehaviour
    {
        [SerializeField] private ObjectPoolingSO ballPooling;
        [SerializeField] private ObjectPoolingSO allLevels;

        private void Awake()
        {
            ballPooling.InitializeObjectPool(ballPooling.name);
            allLevels.InitializeObjectPool(allLevels.name);
        }
    }
}
