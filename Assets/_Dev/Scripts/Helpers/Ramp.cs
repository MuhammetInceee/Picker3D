using System;
using UnityEngine;

namespace Picker.Helpers
{
    public class Ramp : MonoBehaviour
    {
        //TODO Reset Colliders
        
        [SerializeField] private GameObject multipleTiles;

        internal void TileCollidersControl(bool isActive)
        {
            foreach (Collider col in multipleTiles.GetComponentsInChildren<Collider>())
            {
                col.enabled = isActive;
            }
        }

        private void OnDisable()
        {
            TileCollidersControl(true);
        }
    }
}
