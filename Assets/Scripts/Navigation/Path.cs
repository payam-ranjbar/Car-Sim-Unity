using System.Collections;
using System.Collections.Generic;
using Traffic;
using UnityEditor;
using UnityEngine;

namespace Navigation
{
    public class Path : MonoBehaviour 
    {

        public Color lineColor;
        public bool circuit;
        public bool drawInScene;
        [HideInInspector]
        public List<Transform> nodes = new List<Transform>();

        [SerializeField]  Waypoint[] _waypoints;

        public Transform this[int index] => nodes[index];

        private string baseNameOfNodes;
        [SerializeField] private bool useBezier;

        public int NodeCount => nodes.Count;
        public Waypoint[] Waypoints => _waypoints;
        

        public Vector3 GetPosition(int index)
        {
            return _waypoints[index].Position;
        }

        private void Start()
        {
            BuildNodeList();
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

        
#if UNITY_EDITOR

        private void OnDrawGizmosSelected()
        {
            RenameNodes();
            BuildNodeList();
            BuildWaypoints();
        }

        void OnDrawGizmos() 
        {
            if(!drawInScene) return;

            Gizmos.color = lineColor;

            if(nodes is null)
                BuildNodeList();

            if(Waypoints is null)
                BuildWaypoints();
            if (useBezier)
            {
                DrawNodesCurvy();
            }
            else
            {
                DrawLinearNodes();    

            }
            DrawWaypointTangents();
            // 
        }
        
        
        private void DrawNodesCurvy()
        {
            for (var i = 0; i < _waypoints.Length; i++)
            {
                var node = _waypoints[i].Position;
                if(i + 1 >= nodes.Count) continue;
                var nextNode = _waypoints[i + 1].Position;
            
                var halfHeight = (nextNode.y - node.y) * .5f;
            
                var offset = Vector3.up * halfHeight;
                var startTangent = _waypoints[i].TangentOffset;
                var endTangent = _waypoints[i + 1].TangentOffset;

                Gizmos.color = lineColor;
                Gizmos.DrawSphere(node, 0.1f);
                Handles.DrawBezier(node, nextNode, startTangent, endTangent
                    , lineColor, EditorGUIUtility.whiteTexture, 1f);
            }
        }
//         Vector3 GetBezierPosition(float t)
//         {
//             // Vector3 p0 = transformBegin.position;
//             // Vector3 p1 = p0+transformBegin.forward;
//             // Vector3 p3 = transformEnd.position;
//             // Vector3 p2 = p3-transformEnd.back;
//  
// // here is where the magic happens!
//             // return Mathf.Pow(1f-t,3f)*p0+3f*Mathf.Pow(1f-t,2f)*t*p1+3f*(1f-t)*Mathf.Pow(t,2f)*p2+Mathf.Pow(t,3f)*p3;
//         }

        private void DrawLinearNodes()
        {

            for(var i = 0; i < nodes.Count; i++) 
            {
                var currentNode = nodes[i].position;
                Vector3 previousNode;
                Gizmos.color = lineColor;
                Gizmos.DrawSphere(currentNode, 0.1f);
                DrawWaypointWings(currentNode, i);
                if (i > 0)
                {
                    previousNode = nodes[i - 1].position; 
                    Gizmos.color = lineColor;

                    Gizmos.DrawLine(previousNode, currentNode);
                    Handles.Label(new Vector3(currentNode.x  ,currentNode.y , currentNode.z+ 0.7f), nodes[i].name );
                    
                }
                else if(i == 0 && nodes.Count > 1)
                {
                    if (circuit)
                    {
                        Gizmos.color = lineColor;

                        previousNode = nodes[nodes.Count - 1].position;
                        Gizmos.DrawLine(previousNode, nodes[0].position); 
                    }


                }

            }
        }

        private void DrawWaypointTangents()
        {
            for (var i = 0; i < _waypoints.Length; i++)
            {
                var waypoint = _waypoints[i];
                
                Gizmos.color = Color.yellow;
    
                Gizmos.DrawSphere(waypoint.TangentOffset, 0.1f);
                Gizmos.DrawLine(waypoint.transform.position, waypoint.TangentOffset);
            }
        }
#endif                        

        private void DrawWaypointWings(Vector3 currentNode, int i)
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

        private void BuildWaypoints()
        {
            _waypoints = new Waypoint[nodes.Count];
            for (int i = 0; i < nodes.Count; i++)
            {
                var waypoint = nodes[i].GetComponent<Waypoint>();
                 if(waypoint is null) waypoint = nodes[i].gameObject.AddComponent<Waypoint>();
                _waypoints[i] = waypoint;
            }
        }

        private void BuildNodeList()
        {
            Transform[] pathTransforms = GetComponentsInChildren<Transform>();
            nodes = new List<Transform>();

            for (var i = 1; i < pathTransforms.Length; i++)
            {
                var nodeTransform = pathTransforms[i];
                if (nodeTransform != transform)
                {
                    nodes.Add(nodeTransform);
                }
            }
        }
        private void OnValidate()
        {
            RenameNodes();
            BuildNodeList();
            BuildWaypoints();        }

        public List<Transform> GetAllNodes()
        {
            return nodes;
        }
    }
}
