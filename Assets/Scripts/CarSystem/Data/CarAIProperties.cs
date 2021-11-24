using System;
using UnityEngine;

namespace CarSystem
{
    [CreateAssetMenu(fileName = "CarAIProperties", menuName = "Cars/Car AI", order = 0)]
    public class CarAIProperties : ScriptableObject
    {
        public Vector3 oneMeterBackPos;
        public Vector3 twoMeterBackPos;
        public Vector3 threeMeterBackPos;

        public bool oneMeterCarDistance;
        public bool twoMeterCarDistance;
        public bool threeMeterCarDistance;

        private void OnValidate()
        {
            if (oneMeterCarDistance)
            {
                twoMeterCarDistance = false;
                threeMeterCarDistance = false;
            }
            
            if (twoMeterCarDistance)
            {
                oneMeterCarDistance = false;
                threeMeterCarDistance = false;

            }
            
            if (threeMeterCarDistance)
            {
                twoMeterCarDistance = false;
                oneMeterCarDistance = false;
            }
        }


        public Vector3 GetBackDistancePos()
        {
            if (oneMeterCarDistance)
            {
                return oneMeterBackPos;
            } 
            
            if (twoMeterCarDistance)
            {

                return twoMeterBackPos;

            }

            return threeMeterBackPos;
        }

        public void SetOneUnitCarDistance()
        {
            oneMeterCarDistance = true;
            twoMeterCarDistance = false;
            threeMeterCarDistance = false;
        }

        public void SetTwoUnitCarDistance()
        {
            oneMeterCarDistance = false;
            twoMeterCarDistance = true;
            threeMeterCarDistance = false;
        }

        public void SetThreeUnitCarDistance()
        {
            oneMeterCarDistance = false;
            twoMeterCarDistance = false;
            threeMeterCarDistance = true;
        }
        
        
    }
}