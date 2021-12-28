using System;
using CarAI;
using UnityEngine;

namespace CarSystem
{
    public enum DetectionData
    {
        Car, Human, Other
    }
    public class CarSteer : MonoBehaviour
    {
        [SerializeField] private CarEngineProperties engineProperties;
        [SerializeField] private Transform middleSensor;
        [SerializeField] private Transform leftSensor;
        [SerializeField] private Transform rightSensor;
        [SerializeField] private float sensorLength;
        [SerializeField] private float sensorCenterLength;
        [SerializeField] private float angleSensorLength;
        [SerializeField] private float sensorAngle;

        [SerializeField] private float middleSensorOffset = 1f;
        [SerializeField] private bool drawDebug;
        [SerializeField] private bool avoidObstacles;

        private float _length;
        public event Action<float> onDetect;

        private CarWheels _wheels;


        private bool _avoiding;
        private float _avoidMultiplier;

        private Vector3 _destination;
        public Vector3 Destination
        {
            set => _destination = value; 
        }

        private void Start()
        {
            _shortBrakeTime = engineProperties.timeBrake;
        }

        public void Look()
        {
            _avoiding = false;
            _avoidMultiplier = 0f;
            engineProperties.timeBrake = _shortBrakeTime;

            CenterRight();
            Right();
            CenterLeft();
            Left();
            Center();
        }

        private void OnGUI()
        {
            var style = new GUIStyle();
            style.fontSize = 20;
            
            GUILayout.Label(_avoiding.ToString(), style);
            GUILayout.Label(_avoidMultiplier.ToString(), style);

        }

        public void SetWheels(CarWheels wheels) => _wheels = wheels;

        private void OnDrawGizmos()
        {
            if (!drawDebug) return;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(middleSensor.position ,middleSensor.position  + middleSensor.forward * sensorCenterLength);
            var start = middleSensor.position.GetWithX(middleSensor.position.x + middleSensorOffset);
            Gizmos.DrawLine( start ,start + middleSensor.forward * sensorLength);
            start = middleSensor.position.GetWithX(middleSensor.position.x - middleSensorOffset);
            Gizmos.DrawLine(start ,start +middleSensor.forward * sensorLength);
            var dir = Quaternion.AngleAxis(sensorAngle, transform.up) * rightSensor.forward;
            Gizmos.DrawLine(rightSensor.position  ,rightSensor.position  + dir * angleSensorLength);
            dir = Quaternion.AngleAxis(-sensorAngle, transform.up) * leftSensor.forward;
            Gizmos.DrawLine(leftSensor.position  ,leftSensor.position  + dir * angleSensorLength);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(middleSensor.position, _destination);

        }

        private RaycastHit _hit;
        private void ShootRay(Vector3 start, Vector3 direction, Action< RaycastHit> action)
        {
            var detected =
                Physics.Raycast(start, direction, out _hit, _length, engineProperties.AvoidingLayers);
            if (!detected) return;

            action?.Invoke (_hit);
            onDetect?.Invoke(_avoidMultiplier);

            if(drawDebug)
                Debug.DrawLine(start, start+ direction* _length, Color.green);
        }

        private Vector3 _look;
        private float _targetSteerAngle;
        private float _shortBrakeTime;

        public void Steer()
        {
            ApplySteer();
            LerpToSteerAngle();
        }
        public void ApplySteer()
        {
            if (avoidObstacles && _avoiding)
            {
                _targetSteerAngle = engineProperties.maxSteerAngle * _avoidMultiplier;

                return;
            }
            _look = transform.InverseTransformPoint(_destination);
            float newSteer = (_look.x / _look.magnitude) * engineProperties.maxSteerAngle;
            _targetSteerAngle = newSteer;
        }

        public void LerpToSteerAngle()
        {
            var turnSpeed = Time.deltaTime * engineProperties.turnSpeed;
            _wheels.LerpAngle(_targetSteerAngle, turnSpeed , WheelPosition.FL, WheelPosition.FR);

        }



        private void SetAvoiding(float multiplier)
        {
            _avoiding = true;
            _avoidMultiplier = multiplier;
        }

        private void Right()
        {
            _length = angleSensorLength;

            var dir = Quaternion.AngleAxis(sensorAngle, rightSensor.up) * rightSensor.forward;
            var start = rightSensor.position;
            ShootRay(start, dir, (hit) =>
            {
                SetAvoiding(_avoidMultiplier - 0.5f);
            });

        }

        private void Left()
        {
            var dir = Quaternion.AngleAxis(-sensorAngle, leftSensor.up) * leftSensor.forward;
            var start = leftSensor.position;
            _length = angleSensorLength;
            ShootRay(start, dir, (hit) => {

                SetAvoiding(_avoidMultiplier + 0.5f);

            });
        }

        private void CenterRight()
        {
            var dir = middleSensor.forward;
            var start = middleSensor.position;
            _length = sensorLength;

            start += middleSensor.right * middleSensorOffset;        
            ShootRay(start, dir, (hit) =>
            {
                SetAvoiding(_avoidMultiplier - 1f);
            });
        }

        private void CenterLeft()
        {
            var dir = middleSensor.forward;
            var start = middleSensor.position;
            _length = sensorLength;

            start -= middleSensor.right * middleSensorOffset;     
            ShootRay(start, dir, (hit) =>
            {
                SetAvoiding(_avoidMultiplier + 1f);


            });
        }

        private void Center()
        {
            _length = sensorCenterLength;

            var dir = middleSensor.forward;
            var start = middleSensor.position;
            ShootRay(start, dir, (hit) =>
            {
                if (hit.normal.x < 0) 
                {
                   SetAvoiding(-1);
                } 
                else
                {
                    SetAvoiding(1);
                }
            });
        }
    }
}