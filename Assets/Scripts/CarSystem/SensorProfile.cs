using UnityEngine;

namespace CarAI
{
    [CreateAssetMenu(fileName = "Sensor Profile", menuName = "Sensor Profile", order = 0)]
    public class SensorProfile : ScriptableObject
    {
        [SerializeField] private LayerMask senseLayers;


        public LayerMask SenseLayers => senseLayers;

        public bool CompareLayer(LayerMask mask)
        {
            return LayerMask.LayerToName(mask) == LayerMask.LayerToName(senseLayers);
        }
    }
}