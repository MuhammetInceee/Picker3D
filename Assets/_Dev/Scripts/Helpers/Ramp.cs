using UnityEngine;

namespace Picker.Helpers
{
    public class Ramp : MonoBehaviour
    {
        //TODO Reset Colliders
        
        [SerializeField] private GameObject multipleTiles;

        internal void TileCollidersDisabled()
        {
            foreach (Collider col in multipleTiles.GetComponentsInChildren<Collider>())
            {
                col.enabled = false;
            }
        }
    }
}
