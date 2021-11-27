using System;
using CarAI;
using CarSystem;
using UnityEngine;

namespace CarDemo
{
    public class BrakeSensor : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            var car =other.GetComponent<CarEngine>();
            if (car != null)
            {
                car.Brake();
            }
        }
        private void OnTriggerExit(Collider other)
        {
            var car =other.GetComponent<CarEngine>();
            if (car != null)
            {
                car.UnBrake();
            }
        }
    }
}