﻿using System;
using UnityEngine;
 using UnityEngine.UI;

 namespace TrafficSystem
{
    public enum Direction
    {
        NS, WE
    }
    public class Intersection : MonoBehaviour
    {    
        public TrafficLightConfig trafficLightConfig;
        
        public string TrafficBlockTag = "TrafficBlock";
        
        public GameObject[] RightLanesObjects;
        public GameObject[] LeftLanesObjects;

        /**
         *       N/z
         * W/-x            E/x
         *       S/-z
         */
        public GameObject[] carLaneNS;
        public GameObject[] carkLaneWE;


        private Transform[] _rightLaneObjecrts;
        private Transform[] _leftLaneObjecrts;

        public Text[] RightLaneTexts;
        public Text[] LeftLaneTexts;

        private GameObject[] _canvases;
        private void Start()
        {
            
            if (RightLanesObjects != null && LeftLaneTexts != null)
            {
                _canvases = new GameObject[RightLaneTexts.Length + LeftLaneTexts.Length];
                var newArr = RightLaneTexts.Concatenate(LeftLaneTexts);

                for (int i = 0; i < _canvases.Length; i++)
                {
                    _canvases[i] = newArr[i].gameObject;
                }
            }

            if (!trafficLightConfig.showNumbers)
            {
                DisableCanvases();
            }
            _rightLaneObjecrts = new Transform[RightLanesObjects.Length];
  
            
            for (int i = 0; i < RightLanesObjects.Length; i++)
            {
                _rightLaneObjecrts[i] = RightLanesObjects[i].transform;
            }
            
            _leftLaneObjecrts = new Transform[LeftLanesObjects.Length];
            for (int i = 0; i < LeftLanesObjects.Length; i++)
            {
                _leftLaneObjecrts[i] = LeftLanesObjects[i].transform;
            }
        }

        public void DisableCanvases(bool reverse = false)
        {
            foreach (var canvase in _canvases)
            {
                canvase.SetActive(reverse);
            }
        }

        public void SetNSLaneLightNumber(int number, bool red)
        {
            var color = red ? Color.red : Color.green;
            foreach (var text in LeftLaneTexts)
            {
                text.color = color;
                number = number >= 0 ? number : 0;
                if (number < 10)
                {
                    text.text = "0" + number;

                }
                else
                {
                    text.text = number.ToString();

                }
            }
        }
        
        public void SetWELaneLightNumber(int number, bool red)
        {
            var color = red ? Color.red : Color.green;
            foreach (var text in RightLaneTexts)
            {
                text.color = color;
                number = number >= 0 ? number : 0;
                if (number < 10)
                {
                    text.text = "0" + number;

                }
                else
                {
                    text.text = number.ToString();

                }
            }
        }
        public void ActiveNSGreenLightBlubs()
        {
            var right = true;
            var left = false;

            EnableObjectsButCollider(right);
            DisableObjectsButCollider(left);
        }

        public void ActiveWEGreenLightBlubs()
        {
            var right = true;
            var left = false;
            
            DisableObjectsButCollider(right);
            EnableObjectsButCollider(left);
        }
        public void SetCarLane(Direction direction, bool active)
        {
            switch (direction)
            {
                case Direction.NS:
                    ListActivation( carLaneNS, active);
                    break;
                case Direction.WE:
                    ListActivation( carkLaneWE, active);
                    break;

            }
        }
        
        private void ListActivation( GameObject[] list, bool active)
        {
            for (int i = 0; i < list.Length; i++)
            {
                var toMove = list[i].CompareTag(TrafficBlockTag);
                    
                if (toMove)
                {
                    list[i].transform.localPosition = active ? Vector3.zero : Vector3.up * 1000;
                }
                else
                {
                    list[i].SetActive(active);
                }
            }
        }
        public void DisableObjectsButCollider(bool right)
        {
            if (right)
            {
                // Disable all right lane objects

                for (int i = 0; i < RightLanesObjects.Length; i++)
                {
                    var toMove = RightLanesObjects[i].CompareTag(TrafficBlockTag);

                    if (toMove)
                    {
                        RightLanesObjects[i].transform.position = Vector3.up * 1000;
                    }
                    else
                    {
                        RightLanesObjects[i].SetActive(false);
                    }
                }
            }
            
            else
            {
                //Disable all left lane objects

                for (int i = 0; i < LeftLanesObjects.Length; i++)
                {
                    var toMove = LeftLanesObjects[i].CompareTag(TrafficBlockTag);

                    if (toMove)
                    {
                        LeftLanesObjects[i].transform.position = Vector3.up * 1000;
                    }
                    else
                    {
                        LeftLanesObjects[i].SetActive(false);
                    }
                }
            }
        }
        
        public void EnableObjectsButCollider(bool right)
        {
            
            if (right)
            {
                // enable all right lane objects
                for (int i = 0; i < RightLanesObjects.Length; i++)
                {
                    var toMove = RightLanesObjects[i].CompareTag(TrafficBlockTag);
                    
                    if (toMove)
                    {
                        RightLanesObjects[i].transform.localPosition = Vector3.zero;
                    }
                    else
                    {
                        RightLanesObjects[i].SetActive(true);
                    }
                }
            }
            else
            {
                // ennables all left lane objects
                for (int i = 0; i < LeftLanesObjects.Length; i++)
                {
                    var toMove = LeftLanesObjects[i].CompareTag(TrafficBlockTag);

                    if (toMove)
                    {
                        LeftLanesObjects[i].transform.localPosition = Vector3.zero;
                    }
                    else
                    {
                        LeftLanesObjects[i].SetActive(true);
                    }
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(200, 0, 255 , 0.5f);
            
            Gizmos.DrawSphere(transform.position, 5f);
        }
    }
    
    
}