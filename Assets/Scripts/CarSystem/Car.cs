using UnityEngine;
using Path = Navigation.Path;

namespace CarSystem
{
    public class Car : MonoBehaviour
    {
        [SerializeField] private CarEngineProperties engineProperties;

        [SerializeField] private CarSteer steerSensor;
        [SerializeField] private CarEngine engine;
        [SerializeField] private CarSkin carSkin;

        [SerializeField] private CarWheels wheels;
        [SerializeField] private Path path;

        [SerializeField] private int startNode;
        [SerializeField] private float detectionRadius = 5f;

        [SerializeField] private Vector3 centerOfShpere;
        [SerializeField] private float shpereRadius = 1f;
        [SerializeField] private LayerMask mask;
        
        private int _currectNode;
        private Collider[] _colliderBuffers;
        public CarSkinProperties Skin { get => carSkin.Skin; set => carSkin.Skin = value; }
        
        private float _shortBrakeTime;

        private void Start()
        {
            engine.Destination = path[startNode];
            steerSensor.Destination = path[startNode];
            engine.SetWheels(wheels);
            steerSensor.SetWheels(wheels);
            _colliderBuffers = new Collider[20];
            _shortBrakeTime = engineProperties.timeBrake;
        }

        private void OnEnable()
        {
            
            steerSensor.onDetect += ShortBrake;
        }

        private void ShortBrake(float avoidance)
        {
            engine.ShortBrake();
        }

        private void ShortBrakeForTime(float time) => engine.ShortBrake(time);
        
        

        private void OnDisable()
        {
            steerSensor.onDetect -= ShortBrake;
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * detectionRadius);
            Gizmos.DrawWireSphere(transform.position + centerOfShpere, shpereRadius);
        }

        private void Update()
        {
            Autopilot();
        }

        private void Autopilot()
        {
            CheckWayPointDistance();
            SensorLook();
            steerSensor.Steer();
            steerSensor.LerpToSteerAngle();
        }

        private void FixedUpdate()
        {

            engine.Drive();
            engine.Braking();
        }

        private void SensorLook()
        {
            if (Physics.OverlapSphereNonAlloc(
                transform.position + centerOfShpere, shpereRadius, _colliderBuffers, mask) > 0)
            {
                steerSensor.Look();
            }
        }


        private void OnValidate()
        {
            carSkin.ColorCar();
        }


        private void CheckWaypointPassed()
        {
            
        }
        private void CheckWayPointDistance()
        {
            var car = transform.position;
            var node = path[_currectNode];
            var inRange =  car.InRangeFrom2D(node, detectionRadius);
            if (!inRange) return;
            
            if (_currectNode + 1 >= path.NodeCount)
            {
                if (path.circuit)
                {
                    SelectEndNode();
                }
                else
                {
                    EndOfPath();

                }
            }
            else
            {
                TurnChecks();
                SelectNextNode();
            }
        }

        private void SelectEndNode()
        {
            _currectNode = 0;
            engine.Destination = path[_currectNode];
            steerSensor.Destination = path[_currectNode];
        }

        private void SelectNextNode()
        {
            _currectNode++;
            engine.Destination = path[_currectNode];
            steerSensor.Destination = path[_currectNode];
        }

        private void TurnChecks()
        {
            var position = transform.position;
            var angle = path.GetTurnAngle(position, _currectNode + 1);
            var dot = path.GetTurnDotValue(position, _currectNode + 1, engine.CurrentSpeed);

            engine.BrakeToSpeed(dot);
            // if (angle >= engineProperties.maxSteerAngle - 10f)
            // {
            //     var brakeTimeFactor = dot > 0 ? 1 - dot : 1;
            //     ShortBrakeForTime(engineProperties.timeBrake * brakeTimeFactor);
            //     
            // }
        }

        private void EndOfPath()
        {
            var y = transform.position.y;
            var pos1 = path[0].GetWithY(y);
            var pos2 = path[1].GetWithY(y);
            var dir = pos2 - pos1;
            engine.StopCar();
            engine.ResetWheels();
            transform.rotation = Quaternion.LookRotation(dir);

            transform.position = pos1;
            _currectNode = 1;

            engine.StartCar();
        }
    }
}