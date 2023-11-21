using Picker.Extensions;
using UnityEngine;

namespace Picker.Managers
{
    public class PoolingManager : MonoBehaviour
    {
        [SerializeField] private ObjectPoolingSO ballPooling;

        private void Awake()
        {
            ballPooling.InitializeObjectPool(this);
        }
    }
}
