﻿using GameEventSystem;
using UnityEngine;

namespace TrafficSystem
{
    [CreateAssetMenu(fileName = "Traffic Light", menuName = "TrafficSystem/ Traffic Light", order = 0)]
    public class TrafficLightConfig : ScriptableObject
    {
        [Header("Timers")]
        public int RedLightTime;
        public int GreenLightTime;


        public bool showNumbers;

        public void SetTime(int greenTime, int redTime)
        {
             GreenLightTime = greenTime;
             RedLightTime = redTime;
        }
    }
}