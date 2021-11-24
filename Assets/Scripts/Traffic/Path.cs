using System;
using System.Collections.Generic;
using CarAI;
using Traffic;
using UnityEditor;
using UnityEngine;

namespace CarAI
{
    public class Path : MonoBehaviour {

        public Color lineColor;
        public bool Circuit;
        public bool drawInScene;
        public bool useWaypoint;
        [HideInInspector]
        public List<Transform> nodes = new List<Transform>();

        [SerializeField]  Waypoint[] _waypoints;

        
        private string baseNameOfNodes;

        public bool UseWaypoint => useWaypoint;

        public int NodeCount => nodes.Count;

        public Vector3 GetPosition(int index)
        {
            if (useWaypoint)
            {
                return _waypoints[index].Position;
            }

            return nodes[index].position;
        }

        private void Start()
        {
            BuildWaypoints();
        }

        private void RenameNodes()
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                var number = i;
                if (nodes[i] != null)
                    nodes[i].gameObject.name = baseNameOfNodes + " " + number;
            }
            
            BuildWaypoints();

        }

        
        
        private void OnDrawGizmosSelected()
        {
            RenameNodes();
        }

        void OnDrawGizmos() {
            if(!drawInScene) return;


        Gizmos.color = lineColor;

            Transform[] pathTransforms = GetComponentsInChildren<Transform>();
            nodes = new List<Transform>();

            for(int i = 0; i < pathTransforms.Length; i++) {
                if(pathTransforms[i] != transform) {
                    nodes.Add(pathTransforms[i]);
                }
            }

            for(int i = 0; i < nodes.Count; i++) {
                Vector3 currentNode = nodes[i].position;
                Vector3 previousNode = Vector3.zero;
                Gizmos.DrawSphere(currentNode, 0.1f);
                DrawWaypointWings(currentNode, i);
                if (i > 0) {
                    previousNode = nodes[i - 1].position; 
                    Gizmos.DrawLine(previousNode, currentNode);
//                    Gizmos.DrawLine(nodes[i].position, nodes[0].position);

                    var distance = Vector3.Distance(previousNode, currentNode);
//                    Handles.color = Color.yellow;
//                  Handles.Label(new Vector3(currentNode.x  ,currentNode.y + 0.6f, currentNode.z), distance.ToString() );

#if UNITY_EDITOR
                    Handles.Label(new Vector3(currentNode.x  ,currentNode.y , currentNode.z+ 0.7f), nodes[i].name );
#endif                        


                } else if(i == 0 && nodes.Count > 1) {
                    if (Circuit)
                    {
                        previousNode = nodes[nodes.Count - 1].position;
                        Gizmos.DrawLine(previousNode, nodes[0].position); 
                    }


                }

            }
        }

        private void DrawWaypointWings(Vector3 currentNode, int i)
        {
            if (useWaypoint)
            {
                if (_waypoints == null)
                {
                    BuildWaypoints();
                }

                if (_waypoints[i].XAxis)
                {
                    var right = new Vector3(currentNode.x + _waypoints[i].RightOffset, currentNode.y, currentNode.z);
                    Gizmos.DrawLine(currentNode, right);
                    var left = new Vector3(currentNode.x - _waypoints[i].LeftOffset, currentNode.y, currentNode.z);
                    Gizmos.DrawLine(currentNode, left);
                }
                else
                {
                    var right = new Vector3(currentNode.x, currentNode.y, currentNode.z + _waypoints[i].RightOffset);
                    Gizmos.DrawLine(currentNode, right);
                    var left = new Vector3(currentNode.x, currentNode.y, currentNode.z - _waypoints[i].LeftOffset);
                    Gizmos.DrawLine(currentNode, left);
                }

            }
        }

        private void BuildWaypoints()
        {
            if (useWaypoint)
            {
                _waypoints = new Waypoint[nodes.Count];
                for (int i = 0; i < nodes.Count; i++)
                {
                    _waypoints[i] = nodes[i].GetComponent<Waypoint>();
                }

            }
            
        }

        private void OnValidate()
        {
            BuildWaypoints();
        }

        public List<Transform> GetAllNodes()
        {
            return nodes;
        }
    }
}
