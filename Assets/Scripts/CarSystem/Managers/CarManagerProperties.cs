using UnityEngine;

namespace CarSystem.Managers
{
    public enum TrafficMode
    {
        Light, Medium, Heavy
    }
    [CreateAssetMenu(fileName = "Car-Manager-profile", menuName = "Profiles/Car Manager", order = 0)]
    public class CarManagerProperties : ScriptableObject
    {
        public TrafficMode trafficMode = TrafficMode.Medium;
    }
}