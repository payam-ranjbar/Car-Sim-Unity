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
        [Range(5f, 20f)] public float bezierStep = 10f;
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

        }


        private Vector3 GetBezierPointsBetween(int start, int end, float t)
        {
            var startPoint = _waypoints[start].Position;
            var endPoint = _waypoints[end].Position;
            var handle1 = _waypoints[start].TangentOffset ;
            var handle2 = _waypoints[end].TangentOffset ;
            
            var point = Mathf.Pow(1f-t,3f)*startPoint+3f*Mathf.Pow(1f-t,2f)*t*handle1+3f*(1f-t)*Mathf.Pow(t,2f)*handle2+Mathf.Pow(t,3f)*endPoint;
            return point;
        }
        private void DrawNodesCurvy()    
        {
            for (var i = 0; i < _waypoints.Length; i++)
            {
                if(i + 1 >= _waypoints.Length) continue;
                
                DrawBezierCurveBetween(i, i+1);
            }
        }

        private void DrawBezierCurveBetween(int startPos, int endPos)
        {
            var node = _waypoints[startPos].Position;
            var nextNode = _waypoints[endPos].Position;
            var startTangent = _waypoints[startPos].TangentOffset;
            var endTangent = _waypoints[startPos + 1].TangentOffset;
            Gizmos.color = lineColor;
            Handles.DrawBezier(node, nextNode, startTangent, endTangent
                , lineColor, EditorGUIUtility.whiteTexture, 1f);

            var total = 100f;
            
            for (float i = 0; i < total; i+= bezierStep)
            {
                var point = GetBezierPointsBetween(startPos, endPos, i/total);
                Gizmos.color = Color.gray;
                ;    
                Gizmos.DrawSphere(point, 0.1f);
            }
        }

        private void DrawLinearNodes()
        {

            for(var i = 0; i < nodes.Count; i++) 
            {
                var currentNode = nodes[i].position;
                Vector3 previousNode;
                Gizmos.color = lineColor;
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
        
#endif                        

        private void BuildWaypoints()
        {
            _waypoints = new Waypoint[nodes.Count];
            for (int i = 0; i < nodes.Count; i++)
            {
                var waypoint = nodes[i].GetComponent<Waypoint>();
                 if(waypoint is null) waypoint = nodes[i].gameObject.AddComponent<Waypoint>();
                _waypoints[i] = waypoint;
                _waypoints[i].DrawColor = lineColor;
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
