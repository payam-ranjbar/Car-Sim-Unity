using System;
using System.Collections;
using System.Collections.Generic;
using CarSystem;
using TMPro;
using UnityEngine;

namespace CarAI
{
    public class CarEngine : MonoBehaviour
    {
                
        public CarEngineProperties engineProperties;
        public CarAIProperties aiProperties;
        public CarSkin carSkin;
        public Transform path;
        public int startedNodeIndex;
        public WheelCollider wheelFL;
        public WheelCollider wheelFR;
        public WheelCollider wheelRL;
        public WheelCollider wheelRR;
        public Vector3 centerOfMass;
        private float _currentSpeed;
        private bool _isBraking;

        [Header("Positioning")] 
        // public Transform center;
        // public Transform back;
        // public Transform front;
        
        // public GameObject BackBlock;
        [Header("Sensors")]
        public CarSight sensor;

        // public CarEngineSensor sensor;
        // public CarEngineSensor humanSensor;
        private List<Transform> nodes;
        private int currectNode = 0;
        private bool avoiding = false;
        private bool startCheckingForActivation;
        private float targetSteerAngle = 0;

        private float _motorTorque;
        private float _speed;
        private float _distanceToNextNode;
        // private float _carLength;

        private bool _redTrafficLight;
        private Rigidbody _rg;
        public CarSkinProperties Skin { get => carSkin.Skin; set => carSkin.Skin = value; }

        private void Start()
        {
        
            // carSkin.ColorCar();
            // sensor.SetEngine(this);
            // humanSensor.SetEngine(this);
            // BackBlock.transform.localPosition = aiProperties.GetBackDistancePos();
            _rg = GetComponent<Rigidbody>();
            _motorTorque = engineProperties.maxMotorTorque;
            _speed = engineProperties.maxSpeed;
            currectNode = startedNodeIndex;
            _rg.centerOfMass = centerOfMass;
            GetListOfNode(path);
            // _carLength = (front.position - back.position).magnitude;

        }

        private void OnValidate()
        {
            // BackBlock.transform.localPosition = aiProperties.GetBackDistancePos();
            carSkin.ColorCar();
        }

        private void Update()
        {
            if(sensor.HardObjectAhead) Brake();
        }

        private void FixedUpdate()
        {
            if (!_redTrafficLight)
            {
                // Sensors();
                ApplySteer();
                Drive();
                CheckWayPointDistance();
                Braking();
                LerpToSteerAngle();
            }

        }
        
        
        private void GetListOfNode(Transform obj)
        {
            Transform[] pathTransforms = path.GetComponentsInChildren<Transform>();
            nodes = new List<Transform>();

            for (int i = 0; i < pathTransforms.Length; i++)
            {
                if (pathTransforms[i] != path.transform)
                {
                    nodes.Add(pathTransforms[i]);
                }
            }
        }

        private void OnGUI()
        {
            var style = new GUIStyle();
            style.fontSize = 20;
            
            GUILayout.Label(_rg.velocity.ToString(), style);
            GUILayout.Label(_currentSpeed.ToString(), style);
            GUILayout.Label(_isBraking.ToString(), style);
        }

        private void ApplySteer()
        {
            if (avoiding) return;
            Vector3 relativeVector = transform.InverseTransformPoint(nodes[currectNode].position); 
            float newSteer = (relativeVector.x / relativeVector.magnitude) * engineProperties.maxSteerAngle;
            targetSteerAngle = newSteer;
        }

        private void Drive()
        {
            _currentSpeed = 2 * Mathf.PI * wheelFL.radius * wheelFL.rpm * 60 / 1000;

            if (_currentSpeed < engineProperties.maxSpeed && !_isBraking)
            {
                wheelFL.motorTorque = engineProperties.maxMotorTorque;
                wheelFR.motorTorque = engineProperties.maxMotorTorque;
            }
            else
            {
                wheelFL.motorTorque = 0;
                wheelFR.motorTorque = 0;
            }
        }

        private void CheckWayPointDistance()
        {
            var car = transform.position;
            var node = nodes[currectNode].position;
            _distanceToNextNode = Vector2.Distance(new Vector2(car.x, car.z), new Vector2(node.x, node.z));
            if (_distanceToNextNode < 5f)
            {
                // StartCoroutine(TimeBrake());
                if (currectNode == nodes.Count - 1)
                {
                    var pos1 = new Vector3(nodes[0].position.x, transform.position.y, nodes[0].position.z);
                    var pos2 = new Vector3(nodes[1].position.x, transform.position.y, nodes[1].position.z);
                    var dir = pos2 - pos1;
                    StopCar();
                    ResetWheels();
                    transform.rotation = Quaternion.LookRotation(dir);
                    // transform.LookAt(pos1);

                    transform.position = pos1;
                    currectNode = 1;

                    StartCar();
                    // StartCoroutine(TimeBrake());
                }
                else
                {
                    currectNode++;
                }
            }
        }

        private void Braking()
        {
            if (_isBraking)
            {
                Brake();
                // BackBlock.SetActive(true);
            }
            else
            {
                wheelRL.brakeTorque = 0;
                wheelRR.brakeTorque = 0;
                wheelFL.brakeTorque = 0;
                wheelFR.brakeTorque = 0;
            }
        }

        private void LerpToSteerAngle()
        {
            wheelFL.steerAngle = Mathf.Lerp(wheelFL.steerAngle, targetSteerAngle, Time.deltaTime );
            wheelFR.steerAngle = Mathf.Lerp(wheelFR.steerAngle, targetSteerAngle, Time.deltaTime );
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

        public void StopCar()
        {
            ResetTorque();
            _isBraking = true;
            _rg.isKinematic = true;
        }

        public void ResetWheels()
        {
            wheelFL.steerAngle = 0f;
            wheelFR.steerAngle = 0f;
        }
        public void StartCar()
        {
            _rg.isKinematic = false;

            _isBraking = false;

            AddMaxTorque();
            _redTrafficLight = false;
        }

        private IEnumerator TimeBrake()
        {
            _isBraking = true;

            yield return new WaitForSeconds(engineProperties.timeBrake);
            _isBraking = false;
        }

        private void SetTorque(float trq)
        {
            wheelFL.motorTorque = trq;
            wheelFR.motorTorque = trq;
        }

        private void AddMaxTorque() => SetTorque(engineProperties.maxMotorTorque);

        private void ResetTorque() => SetTorque(0f);

        public void Brake() => StartCoroutine(ApplayBrake());

        public IEnumerator ApplayBrake()
        {
            _isBraking = true;
            var brakeTorque = engineProperties.maxBrakeTorque;
            wheelFL.brakeTorque = brakeTorque;
            wheelRL.brakeTorque = brakeTorque;
            wheelFR.brakeTorque = brakeTorque;
            wheelRR.brakeTorque = brakeTorque;
            brakeTorque = engineProperties.brakeTorque;
            if (sensor.HardObjectAhead)
            {
                var dis = Vector3.Distance(transform.position, sensor.HardObjectPos);

                while (dis > 0f) 
                {
                    if(!_isBraking) yield break;
                    // brakeTorque += engineProperties.brakeTorque;
                    wheelFL.brakeTorque += brakeTorque;
                    wheelRL.brakeTorque += brakeTorque;
                    wheelFR.brakeTorque += brakeTorque;
                    wheelRR.brakeTorque += brakeTorque;
                    yield return null;
                    dis = Vector3.Distance(transform.position, sensor.HardObjectPos);

                }
            }
            // while (_currentSpeed > 0f)
            // {
            //     if(!_isBraking) yield break;
            //     // brakeTorque += engineProperties.brakeTorque;
            //     yield return null;
            //     wheelFL.brakeTorque += brakeTorque;
            //     wheelRL.brakeTorque += brakeTorque;
            //     wheelFR.brakeTorque += brakeTorque;
            //     wheelRR.brakeTorque += brakeTorque;
            // }
            // yield return null;

        }
        
    }

}
