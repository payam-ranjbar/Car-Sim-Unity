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
        [SerializeField] private float sensorAngle;

        [SerializeField] private float middleSensorOffset = 1f;
        [SerializeField] private bool drawDebug;
        [SerializeField] private bool avoidObstacles;


        private CarWheels _wheels;

        private bool _avoiding;
        private float _avoidMultiplier;

        private Vector3 _destination;
        public Vector3 Destination
        {
            set => _destination = value; 
        }

        private void OnGUI()
        {
            var style = new GUIStyle();
            style.fontSize = 20;
            
            GUILayout.Label(_avoiding.ToString(), style);
            GUILayout.Label(_avoidMultiplier.ToString(), style);

        }
        public void Look()
        {
            _avoiding = false;
            _avoidMultiplier = 0f;
            Center();
            CenterLeft();
            CenterRight();
            Left();
            Right();
        }

        public void SetWheels(CarWheels wheels) => _wheels = wheels;

        private void OnDrawGizmos()
        {
            if (!drawDebug) return;
            if (middleSensor != null)
            {

                
                Gizmos.color = Color.red;

                Gizmos.DrawLine(middleSensor.position ,middleSensor.position  + middleSensor.forward * sensorLength);
                var start = middleSensor.position.GetWithX(middleSensor.position.x + middleSensorOffset);
                Gizmos.DrawLine( start ,start +middleSensor.forward * sensorLength);
                start = middleSensor.position.GetWithX(middleSensor.position.x - middleSensorOffset);
                Gizmos.DrawLine(start ,start +middleSensor.forward * sensorLength);
                var dir = Quaternion.AngleAxis(sensorAngle, transform.up) * rightSensor.forward;
                Gizmos.DrawLine(rightSensor.position  ,rightSensor.position  + dir * sensorLength);
                dir = Quaternion.AngleAxis(-sensorAngle, transform.up) * leftSensor.forward;
                Gizmos.DrawLine(leftSensor.position  ,leftSensor.position  + dir * sensorLength);

            }
        }

        private void ShootRay(Vector3 start, Vector3 direction, Action< RaycastHit> action)
        {
            var detected = Physics.Raycast(start, direction, out var hit, sensorLength, engineProperties.AvoidingLayers);
            if (!detected) return;
            action?.Invoke (hit);
            if(drawDebug)
                Debug.DrawLine(middleSensor.position , hit.point, Color.green);

        }

        private Vector3 _look;
        private float _targetSteerAngle;

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
            ShootRay(start, dir, (hit) => {

                SetAvoiding(_avoidMultiplier + 0.5f);

            });
        }

        private void CenterRight()
        {
            var dir = middleSensor.forward;
            var start = middleSensor.position + middleSensor.position.GetWithX(middleSensor.position.x + middleSensorOffset);
            ShootRay(start, dir, (hit) =>
            {
                SetAvoiding(_avoidMultiplier - 1f);
            });
        }

        private void CenterLeft()
        {
            var dir = middleSensor.forward;
            var start = middleSensor.position  + middleSensor.position.GetWithX(middleSensor.position.x - middleSensorOffset);
            ShootRay(start, dir, (hit) =>
            {
                SetAvoiding(_avoidMultiplier + 1f);


            });
        }

        private void Center()
        {
            
            var dir = middleSensor.forward;
            var start = middleSensor.position  + middleSensor.position.GetWithX(middleSensor.position.x + middleSensorOffset);
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