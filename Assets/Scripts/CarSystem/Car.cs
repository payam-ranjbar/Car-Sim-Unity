using System;
using System.Collections;
using DG.Tweening.Plugins.Core.PathCore;
using UnityEngine;
using Path = Traffic.Path;

namespace CarSystem
{
    public class Car : MonoBehaviour
    {
        [SerializeField] private CarSteer steerSensor;
        [SerializeField] private CarEngine engine;
        [SerializeField] private CarSkin carSkin;

        [SerializeField] private CarWheels wheels;
        [SerializeField] private Path path;

        [SerializeField] private int startNode;
        [SerializeField] private float detectionRadius = 5f;

        [SerializeField] private Vector3 center;
        [SerializeField] private Vector3 center2;
        [SerializeField] private float capRadius = 1f;
        [SerializeField] private LayerMask mask;
        
        private int _currectNode;
        public CarSkinProperties Skin { get => carSkin.Skin; set => carSkin.Skin = value; }
        private void OnGUI()
        {
            var style = new GUIStyle();
            style.fontSize = 20;
            
            GUILayout.Label(_currectNode.ToString(), style);

        }
        private void Start()
        {
            engine.Destination = path[startNode].position;
            steerSensor.Destination = path[startNode].position;
            engine.SetWheels(wheels);
            steerSensor.SetWheels(wheels);
            _colliders = new Collider[20];
            // StartCoroutine(SensorUpdate());
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * detectionRadius);
            Gizmos.DrawWireSphere(transform.position + center, capRadius);
        }

        private int _frames;
        [SerializeField] private int howManyFrames = 10;
        private Collider[] _colliders;

        IEnumerator SensorUpdate()
        {
            while (true)
            {
                if (_frames < howManyFrames)
                {
                    _frames++;
                    yield return null;
                    continue;
                } 
                if (Physics.OverlapSphereNonAlloc(
                    transform.position + center, capRadius, _colliders, mask) > 0)
                {
                    steerSensor.Look();
                }
                _frames = 0;
                yield return null;

            }
        }
        private void Update()
        {
            // steerSensor.Look();
            // steerSensor.Steer();
            // CheckWayPointDistance();
            if (useCast)
            {
                if (Physics.OverlapSphereNonAlloc(
                    transform.position + center, capRadius, _colliders, mask) > 0)
                {
                    steerSensor.Look();
                }
            }
            else
            {
                steerSensor.Look();
            
            }
        }

        public bool useCast;
        private void FixedUpdate()
        {
            // engine.RunEngine();

            steerSensor.Steer();
            engine.Drive();
            CheckWayPointDistance();
            engine.Braking();
            steerSensor.LerpToSteerAngle();
        }


        private void OnValidate()
        {
            carSkin.ColorCar();
        }


        private void CheckWayPointDistance()
        {
            var car = transform.position;
            var node = path[_currectNode].position;
            var inRange =  car.InRangeFrom2D(node, detectionRadius);
            if (!inRange) return;
            
            if (_currectNode + 1 >= path.NodeCount)
            {
                EndOfPath();
            }
            else
            {
                SelectNextNode();
            }
        }

        private void SelectNextNode()
        {
            _currectNode++;
            engine.Destination = path[_currectNode].position;
            steerSensor.Destination = path[_currectNode].position;
        }

        private void EndOfPath()
        {
            var y = transform.position.y;
            var pos1 = path[0].position.GetWithY(y);
            var pos2 = path[1].position.GetWithY(y);
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