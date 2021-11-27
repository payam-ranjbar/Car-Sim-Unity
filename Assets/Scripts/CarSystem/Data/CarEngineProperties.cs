using UnityEngine;

namespace CarSystem
{
    [CreateAssetMenu(fileName = "Engine Properties", menuName = "Cars/Car Properties", order = 0)]
    public class CarEngineProperties : ScriptableObject
    {
        public LayerMask avoidingLayer;
        public double speedLimitForBumps;
        public float maxSteerAngle = 45f;
        public float turnSpeed = 5f;
        public float maxMotorTorque = 80f;
        public float maxBrakeTorque = 150f;
        public float currentSpeed;
        public float maxSpeed = 100f;
        public float timeBrake = 1f;
        public float brakeTorque = 100f;
        public int AvoidingLayers => avoidingLayer;
    }
}