using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDetector : MonoBehaviour
{
    private bool _carsOnIntersection;

    public int _numberOfCars = 0;

    public bool HasCarsOnIt => _numberOfCars > 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TrafficBlocksCar") || other.CompareTag("CarAgent") )
        {
            _carsOnIntersection = true;
            _numberOfCars++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TrafficBlocksCar") || other.CompareTag("CarAgent") )
        {
            _carsOnIntersection = true;
            _numberOfCars--;
        }
    }
}
