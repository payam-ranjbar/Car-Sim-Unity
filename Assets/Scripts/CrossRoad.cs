using System;
using System.Collections.Generic;
using Navigation;
using Traffic;
using UnityEngine;

namespace CarDemo
{
    public class CrossRoad : MonoBehaviour
    {
        private List<Pedestrian> _pedestrians;

        private void Awake()
        {
            _pedestrians = new List<Pedestrian>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Human"))
            {
                var p = other.GetComponent<Pedestrian>();
                if (p != null)
                {
                    _pedestrians.Add(p);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Human"))
            {
                var p = other.GetComponent<Pedestrian>();
                if (p != null)
                {
                    if(_pedestrians.Contains(p))
                        _pedestrians.Remove(p);
                }
            }

            if (other.CompareTag("TrafficBlocksCar"))
            {
                NotifyNpcsToRun();
            }
            
        }

        private void NotifyNpcsToRun()
        {
            for (int i = 0; i < _pedestrians.Count; i++)
            {
                _pedestrians[i].EnableRun();
            }
        }
    }
}