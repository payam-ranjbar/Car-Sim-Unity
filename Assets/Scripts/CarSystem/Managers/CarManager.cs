using System;
using CarAI;
using GameGlobals;
using Traffic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CarSystem.Managers
{
    public class CarManager : MonoBehaviour
    {
        [SerializeField] private CarManagerProperties properties;
        [SerializeField] private GameProperties gameProperties;
        [SerializeField] private CarEngine[] trackingCars;
        [SerializeField] private CarEngine[] lightTraffic;
        [SerializeField] private CarEngine[] mediumTraffic;
        [SerializeField] private CarEngine[] heavyTraffic;
        [SerializeField] private Path[] paths;
        [SerializeField] private CarSkinProperties[] carSkins;

        private void Awake()
        {
            ColorAllCars();
            SetTrafficSize();
            InitializePaths();
        }

        [ContextMenu("Get All Cars")]
        public void GetAllCars()
        {
            trackingCars = FindObjectsOfType<CarEngine>();
            
        }
        
        [ContextMenu("Random Color All Cars")]
        public void ColorAllCars()
        {
            for (int i = 0; i < trackingCars.Length; i++)
            {
                // trackingCars[i].Skin = GetRandomSkin();
                Debug.Log("Nothing is happening yet");
            }
        }

        public CarSkinProperties GetRandomSkin()
        {
            var index = Random.Range(0, carSkins.Length);
            return carSkins[index];
        }

        private void SetTrafficSize()
        {
            switch (properties.trafficMode)
            {
                case TrafficMode.Light:
                    LightTraffic();
                    break;
                case TrafficMode.Medium:
                    MediumTraffic();
                    break;
                case TrafficMode.Heavy:
                    HeavyTraffic();
                    break;
            }
        }
        [ContextMenu("Light Traffic")]
        private void LightTraffic()
        {
            SetListActive(lightTraffic, false);
        }
        [ContextMenu("Medium Traffic")]

        private void MediumTraffic()
        {
            SetListActive(mediumTraffic, false);
        }
        [ContextMenu("Heavy Traffic")]

        private void HeavyTraffic()
        {
            SetListActive(heavyTraffic, false);
        }
        
        [ContextMenu("All Traffic")]
        private void ActiveAll()
        {
            SetListActive(trackingCars, true);
        }        
        [ContextMenu("No Traffic")]
        private void DeactiveAll()
        {
            SetListActive(trackingCars, false);
        }

        private void InitializePaths()
        {
            foreach (var path in paths)
            {
                path.useWaypoint = gameProperties.useWaypointOffset;
            }
        }

        private static void SetListActive(CarEngine[] cars, bool active)
        {
            for (int i = 0; i < cars.Length; i++)
            {
                cars[i].gameObject.SetActive(active);
            }
        }
    }
}