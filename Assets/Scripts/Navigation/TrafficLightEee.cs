using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Traffic
{
    public class TrafficLightEee : MonoBehaviour
    {
        [SerializeField] private int redLightTime;
        [SerializeField] private List<GameObject> collidersLaneOne;
        [SerializeField] private List<GameObject> collidersLaneTwo;
        [SerializeField] private int time;

        [SerializeField]private List<Vector3> collidersLaneOneTransforms;
        [SerializeField]private List<Vector3> collidersLaneTwoTransforms;

        private void Start()
        {
            collidersLaneTwoTransforms = new List<Vector3>();
            collidersLaneOneTransforms = new List<Vector3>();
            foreach (var item in GetComponentsInChildren<BoxCollider>())
            {
                item.GetComponent<BoxCollider>().isTrigger = true;
            }

            foreach (var item in collidersLaneOne)
            {
                collidersLaneOneTransforms.Add(item.transform.position);
            }
            
            foreach (var item in collidersLaneTwo)
            {
                collidersLaneTwoTransforms.Add(item.transform.position);
            }
 
            SwitchLights(false);
            StartCoroutine(StartRedLight(true));

        }

        public void LaneOneLight(bool red)
        {

            foreach (var item in collidersLaneOne)
            {
                if (red)
                {
                    item.transform.position = new Vector3(item.transform.position.x,
                        item.transform.position.y + 200,
                        item.transform.position.z);
                }
                else
                {
                    item.transform.position = collidersLaneOneTransforms[collidersLaneOne.IndexOf(item)];
                }
            }
        }

        public void LaneTwoLight(bool red)
        {
            foreach (var item in collidersLaneTwo)
            {
                if (red)
                {
                    item.transform.position = new Vector3(item.transform.position.x,
                        item.transform.position.y + 200,
                        item.transform.position.z);
                }
                else
                {
                    item.transform.position = collidersLaneTwoTransforms[collidersLaneTwo.IndexOf(item)];
                }
            }
        }

        public void SwitchLights(bool laneOneRed)
        {
            LaneOneLight(laneOneRed);
            LaneTwoLight(!laneOneRed);
        }
        
        public IEnumerator StartRedLight(bool laneOne)
        {
            time = redLightTime;
            SwitchLights(laneOne);
            while (time >= 0)
            {
                yield return new WaitForSeconds(1f);
                time--;
            }
//            SwitchLights(!laneOne);
            StartCoroutine(StartRedLight(!laneOne));
        }
    }
}