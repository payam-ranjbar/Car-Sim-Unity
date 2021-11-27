using CarSystem;
using GameGlobals;
using UnityEngine;

namespace CarAI
{
    public class CarEngineSensor : MonoBehaviour
    {

        [SerializeField] private GameProperties globals;
        [SerializeField] private bool detectNpcs = true;
        [SerializeField] private bool detectBumps = true;
        [SerializeField] private bool detectPlayer = true;
        [SerializeField] private bool detectCars = true;
        [SerializeField] private bool detectTrafficBlocks = true;
        private CarEngine _engine;
        private const string BumpTag = "Bump";
        public void SetEngine(CarEngine engine)
        {
            _engine = engine;
        }

        private bool NpcDetectionActive(Collider other) => detectNpcs && other.CompareTag(globals.npcTag);
        private bool CarDetectionActive(Collider other) => detectCars && other.CompareTag(globals.backOfCarTag);
        private bool PlayerDetectionActive(Collider other) => detectPlayer && other.CompareTag(globals.playerTag);
        private bool BumpDetectionActive(Collider other) => detectBumps && other.CompareTag(BumpTag);
        private bool TrafficBlocksDetectionActive(Collider other) => detectTrafficBlocks && other.CompareTag(globals.trafficBlockTag);
        
        private void OnTriggerEnter(Collider other)
        {
            //if object is block itself ignore collision
            if(gameObject.CompareTag(globals.trafficBlockTag) || gameObject.CompareTag(globals.backOfCarTag)) return;
            
            //if object is agent:
            if (TrafficBlocksDetectionActive(other) || PlayerDetectionActive(other) || NpcDetectionActive(other))
            {
                _engine.StopCar();
            }

            if (BumpDetectionActive(other))
            {
                // _engine.ShortBrake();
            }

        }

        private void OnTriggerExit(Collider other)
        {

            if (TrafficBlocksDetectionActive(other) || PlayerDetectionActive(other) || NpcDetectionActive(other))
            {
                _engine.StartCar();
            }

            if (BumpDetectionActive(other))
            {
                _engine.UnBrake();
            }
        }
        
    }
}