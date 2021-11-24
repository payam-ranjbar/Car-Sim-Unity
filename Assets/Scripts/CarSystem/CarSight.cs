using System;
using UnityEngine;

namespace CarAI
{
    public class CarSight : MonoBehaviour
    {
        [SerializeField] private Transform middleSensor;
        [SerializeField] private float sensorLength;

        private bool _hardObjectAhead;

        private Vector3 sensorStartPos;


        private Vector3 _hardObjectPos;

        public Vector3 HardObjectPos => _hardObjectPos;

        public bool HardObjectAhead => _hardObjectAhead;

        public void Start()
        {
            sensorStartPos = middleSensor.position;
        }

        private void Update()
        {
            MiddleSensor();
        }

        private void OnDrawGizmos()
        {
            if (middleSensor != null)
            {
                sensorStartPos = middleSensor.position;
                if(_hardObjectAhead) Gizmos.color = Color.blue;
                else                 Gizmos.color = Color.red;

                Gizmos.DrawLine(sensorStartPos,sensorStartPos +middleSensor.forward * sensorLength);
            }
        }

        private void MiddleSensor()
        {
            // Debug.DrawLine(sensorStartPos, sensorStartPos + sensorStartPos * sensorLength, Color.red);

            if (Physics.Raycast(sensorStartPos, middleSensor.forward, out var hit, sensorLength)) {
                if (hit.collider.CompareTag("Hard Block"))
                {
                    _hardObjectPos = hit.point;
                    _hardObjectAhead = true;
                    Debug.DrawLine(sensorStartPos, hit.point, Color.green);
                }
                else
                {
                    _hardObjectAhead = false;

                }
            }
        }
        
    }
}