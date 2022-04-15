using System.Collections;
using System.Collections.Generic;
using CarAI;
using Navigation;
using Traffic;
using UnityEngine;

namespace CarSystem
{
    [RequireComponent(typeof(Rigidbody))]

    public class CarEngine : MonoBehaviour
    {
                
        [SerializeField] private CarEngineProperties engineProperties;
        [SerializeField] private Vector3 centerOfMass;

        
        private float _currentSpeed;
        private bool _isBraking;
        private Path _path;
        private Rigidbody _rg;
        private CarWheels _wheels;

        private Vector3 _destination;

        public float CurrentSpeed => _currentSpeed;

        public Vector3 Destination
        {
            get => _destination;
            set => _destination = value;
        }

        private void Start()
        {
            _rg = GetComponent<Rigidbody>();
            _rg.centerOfMass = centerOfMass;
        }

        public void RunEngine()
        {
            Drive();
            Braking();
        }
        



        public void Drive()
        {
            CalculateSpeed();

            if (_currentSpeed <= engineProperties.maxSpeed && !_isBraking)
            {
                _wheels.SetTorque(engineProperties.maxMotorTorque, WheelPosition.FL, WheelPosition.FR);

            }
            else
            {
                _wheels.SetTorque(0, WheelPosition.FL, WheelPosition.FR);

            }
        }

        private void CalculateSpeed()
        {
            var wheel = _wheels.GetWheel(WheelPosition.FL);
            _currentSpeed = 2 * Mathf.PI * wheel.radius * wheel.rpm * 60 / 1000;
        }

        public void Braking()
        {
            if (_isBraking)
            {
                Brake();
            }
            else
            {
                _wheels.SetBrakeTorque(0f);
            }
        }

        public void UnBrake() => _isBraking = false;

        public void ShortBrake()
        {
            if (_currentSpeed > engineProperties.speedLimitForBumps)
            {
                _isBraking = true;

                StartCoroutine(TimeBrake());
            }
        }

        public void ShortBrake(float time)
        {
            StartCoroutine(TimeBrake(time));
        }

        private IEnumerator BrakeToCompensateSpeedTo(float speed)
        {
            if(_isBraking) yield break;

            speed = Mathf.Abs(speed ) ;
            
            if (_currentSpeed <= speed) yield break;


            _isBraking = true;
            
            yield return new WaitUntil(() => _currentSpeed <= speed + engineProperties.minSpeedOnTurn);
            
            _isBraking = false;
        }

        public void BrakeToSpeed(float speed) => StartCoroutine(BrakeToCompensateSpeedTo(speed));
        public void StopCar()
        {
            ResetTorque();
            _isBraking = true;
            _rg.isKinematic = true;
        }

        public void ResetWheels()
        {
            _wheels.SetSteerAngle(0f, WheelPosition.FL, WheelPosition.FR);
        }
        public void StartCar()
        {
            _rg.isKinematic = false;
            _isBraking = false;
            AddMaxTorque();
        }

        private IEnumerator TimeBrake()
        {
            _isBraking = true;

            yield return new WaitForSeconds(engineProperties.timeBrake);
            _isBraking = false;
        }
        private IEnumerator TimeBrake(float time)
        {
            _isBraking = true;

            yield return new WaitForSeconds(time);
            _isBraking = false;
        }

        private void SetTorque(float trq)
        {
            _wheels.SetTorque(trq, WheelPosition.FL, WheelPosition.FR);
        }

        private void AddMaxTorque() => SetTorque(engineProperties.maxMotorTorque);

        private void ResetTorque() => SetTorque(0f);

        public void Brake() => StartCoroutine(ApplayBrake());

        public IEnumerator ApplayBrake()
        {
            _isBraking = true;
            var brakeTorque = engineProperties.maxBrakeTorque;
            _wheels.SetBrakeTorque(brakeTorque);
            yield break;
        }

        public void SetWheels(CarWheels wheels)
        {
            _wheels = wheels;
        }
    }

}
