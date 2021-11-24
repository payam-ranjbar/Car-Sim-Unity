using UnityEngine;

namespace Traffic
{
    [CreateAssetMenu(fileName = "Pedestrian Profile", menuName = "Pedestrian Profile", order = 0)]
    public class PedestrianProfile : ScriptableObject
    {
        [Header("animation Triggers")]
        public string walkTrigger = "walk";
        public string runTrigger = "run";
        public string idleTrigger = "idle";

        public float lookAtDuration;
        public float maxSpeed = 1.5f;
        public float maxRunSpeed = 3f;
        public float decisionMakingRadius = 0.5f;
    }
}