using UnityEngine;

namespace GameGlobals
{
    [CreateAssetMenu(fileName = "Game Properties", menuName = "Game Properties", order = 0)]
    public class GameProperties : ScriptableObject
    {
        public string npcTag = "Human";
        public string playerTag = "Player";
        public string carTag;
        public string trafficBlockTag = "TrafficBlocksCar";
        public string backOfCarTag = "BackOfCar";

        public float interSectionElevation = .01f;
        public float maxInterSectionElevation = 0.1f;
        public float minInterSectionElevation = 0;
        public bool useWaypointOffset;

        public float InterSectionElevation => Mathf.Clamp(interSectionElevation, minInterSectionElevation, maxInterSectionElevation);
    }
}