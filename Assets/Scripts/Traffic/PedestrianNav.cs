using System;
using UnityEngine;

namespace Traffic
{
    public class PedestrianNav : MonoBehaviour
    {
        public Pedestrian Ped;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Human")|| other.CompareTag("Player") || other.CompareTag("CarSide"))
            {
                Ped.StopWalking();
            }
        }

        private void OnTriggerExit(Collider other)
        { 
            if (other.CompareTag("Human") || other.CompareTag("Player")|| other.CompareTag("CarSide"))
            {
                Ped.StartWalkingAfterCollisionWith(true);

            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.collider.CompareTag("CarAgent"))
            {
                // Ped.onTrigger(true, true);

            }
    
        }
        
        private void OnCollisionExit(Collision other)
        {
            if (other.collider.CompareTag("CarAgent"))
            {
                // Ped.onTrigger(false, true);

            }
        }
    }
}