using System;
using UnityEngine;

namespace CarSystem
{
    public class CarWheels : MonoBehaviour
    {
        [SerializeField] private WheelCollider wheelFL;
        [SerializeField] private WheelCollider wheelFR;
        [SerializeField] private WheelCollider wheelRL;
        [SerializeField] private WheelCollider wheelRR;

        public void SetSteerAngle(float angle, params WheelPosition[] poses)
        {
            foreach (var position in poses)
            {
                SetSteerAngle(angle, position);
            }
        }
        public void SetSteerAngle(float angle,  WheelPosition pose)
        {
            GetWheel(pose).steerAngle = angle;
        }
        public void LerpAngle(float max, float i, params WheelPosition[] poses)
        {
            foreach (var position in poses)
            {
                LerpAngle(max, i, position);
            }
        }

        public void LerpAngle(float max, float i, WheelPosition position)
        {
            var wheel = GetWheel(position);
            wheel.steerAngle = Mathf.Lerp(wheel.steerAngle, max, i);
        }
        public void SetBrakeTorque(float trq, params WheelPosition[] poses)
        {
            foreach (var position in poses)
            {
                SetBrakeTorque(trq, position);
            }
        }

        public void SetBrakeTorque(float trq, WheelPosition position)
        {
            GetWheel(position).brakeTorque = trq;

        }

        public void SetBrakeTorque(float trq)
        {
            SetBrakeTorque(trq , WheelPosition.FL, WheelPosition.FR, WheelPosition.RL, WheelPosition.RR);

        }
        public void SetTorque(float trq, params WheelPosition[] poses)
        {
            foreach (var position in poses)
            {
                SetTorque(trq, position);
            }
        }
        public void SetTorque(float trq, WheelPosition position)
        {
            GetWheel(position).motorTorque = trq;
        }

        public void SetTorque(float trq)
        {
            SetTorque(trq , WheelPosition.FL, WheelPosition.FR, WheelPosition.RL, WheelPosition.RR);
        }


        public WheelCollider GetWheel(WheelPosition position)
        {
            return position switch
            {
                WheelPosition.FR => wheelFR,
                WheelPosition.FL => wheelFL,
                WheelPosition.RR => wheelRR,
                WheelPosition.RL => wheelRL,
                _ => throw new ArgumentOutOfRangeException(nameof(position), position, null)
            };
        }
    }
}