using System;
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
        [SerializeField] private bool usePathFinder;
        [SerializeField] private PathFinder pathFinder;
        [Range(5f, 20f)] public float bezierStep = 10f;

        [SerializeField]  Waypoint[] _waypoints;

        private Vector3[] _bakedNodes;
        
        public Vector3 this[int index] => _bakedNodes[index];

       [SerializeField] private string baseNameOfNodes;
        [SerializeField] private bool useBezier;

        public int NodeCount => _bakedNodes.Length;
        public Waypoint[] Waypoints => _waypoints;
        
        private void Awake()
        {
            BuildWaypoints();
            BuildBakedPositions();
        }

        private void Start()
        {
            if (usePathFinder)
            {
                _waypoints = pathFinder.GetWaypoints();
            }
        }

        public Waypoint GetNearestWaypoint(int point)
        {
            var fractionOfLine = 100 / (int) bezierStep;

            var index = Math.Abs( point / fractionOfLine);

            if (index < _waypoints.Length) return _waypoints[index];
            
            return _waypoints[_waypoints.Length - 1];

        }

        public float GetTurnDotValue(Vector3 currentPos, int frontNode)
        {
            if (frontNode >= NodeCount - 1) return 0f;
            
            var node = _bakedNodes[frontNode];
            var nextNode = _bakedNodes[frontNode + 1];

            
            var lookVector = node - currentPos;

            var nodeVector = nextNode - node;

            var dot = Vector3.Dot(lookVector, nodeVector);

            return dot;
        }
        public float GetTurnAngle(Vector3 currentPos, int frontNode)
        {
            if (frontNode >= NodeCount - 1) return 0f;
            
            var node = _bakedNodes[frontNode];
            var nextNode = _bakedNodes[frontNode + 1];

            
            var lookVector = node - currentPos;

            var nodeVector = nextNode - node;

            var angle = Vector3.Angle(lookVector, nodeVector);

            return angle;

        }


        [ContextMenu("Bake")]
        private void BuildBakedPositions()
        {
            
            var list = new List<Vector3>();
            for (var index = 0; index < _waypoints.Length; index++)
            {
                if(index + 1 >= _waypoints.Length) continue;
                var startOfList = list.Count <= 0 ? 0 : list.Count - 1;
                for (float t = 0f; t < 100f; t += bezierStep)
                {
                    var point = GetBezierPointsBetween(index, index + 1, t/100f);
                    list.Add(point);
                }
                 
            }

            _bakedNodes = list.ToArray();
        }



        
        
#if UNITY_EDITOR
        private void RenameNodes()
        {
            BuildWaypoints();
            for (int i = 0; i < _waypoints.Length; i++)
            {
                var number = i;
                if (_waypoints[i] != null)
                    _waypoints[i].gameObject.name = baseNameOfNodes + " " + number;
            }
        }
        private void OnDrawGizmosSelected()
        {
            RenameNodes();
            BuildWaypoints();
        }

        void OnDrawGizmos() 
        {
            if(!drawInScene) return;

            Gizmos.color = lineColor;
            

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
            var handle1 = _waypoints[start].LowerTangentOffset ;
            var handle2 = _waypoints[end].UpperTangentOffset ;
            
            var point = Mathf.Pow(1f-t,3f)*startPoint+3f*Mathf.Pow(1f-t,2f)*t*handle1+3f*(1f-t)*Mathf.Pow(t,2f)*handle2+Mathf.Pow(t,3f)*endPoint;
            return point;
        }

        private void DrawNodesCurvy()    
        {
            for (var i = 0; i < _waypoints.Length; i++)
            {
                if (i + 1 >= _waypoints.Length)
                {
                    if (circuit)
                    {
                        if(i < _waypoints.Length)
                            DrawBezierCurveBetween(i, 0);
                    }
                    else
                    {
                        continue;
                    }
                }
                
                DrawBezierCurveBetween(i, i+1);
            }
        }

        private void DrawBezierCurveBetween(int startPos, int endPos)
        {
            if(startPos >= _waypoints.Length || endPos >= _waypoints.Length) return;
            var node = _waypoints[startPos].Position;
            var nextNode = _waypoints[endPos].Position;
            var startTangent = _waypoints[startPos].LowerTangentOffset;
            var endTangent = _waypoints[endPos].UpperTangentOffset;
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

        private void BuildWaypoints()
        {
            var nodes = GetComponentsInChildren<Transform>();
            _waypoints = new Waypoint[nodes.Length - 1];
            for (int i = 1; i < nodes.Length; i++)
            {
                var waypoint = nodes[i].GetComponent<Waypoint>();
                if(waypoint is null) waypoint = nodes[i].gameObject.AddComponent<Waypoint>();
                _waypoints[i - 1] = waypoint;
                _waypoints[i - 1].DrawColor = lineColor;
            }
        }

        private void DrawLinearNodes()
        {

            for(var i = 0; i < _waypoints.Length; i++) 
            {
                var currentNode = _waypoints[i].Position;
                Vector3 previousNode;
                Gizmos.color = lineColor;
                if (i > 0)
                {
                    previousNode = _waypoints[i - 1].Position; 
                    Gizmos.color = lineColor;

                    Gizmos.DrawLine(previousNode, currentNode);
                    Handles.Label(new Vector3(currentNode.x  ,currentNode.y , currentNode.z+ 0.7f), _waypoints[i].name );
                    
                }
                else if(i == 0 && _waypoints.Length > 1)
                {
                    if (circuit)
                    {
                        Gizmos.color = lineColor;

                        previousNode = _waypoints[_waypoints.Length - 1].Position;
                        Gizmos.DrawLine(previousNode, _waypoints[0].Position); 
                    }


                }

            }
        }
        
#endif
        private void OnValidate()
        {
            RenameNodes();
            BuildWaypoints();
            
        }
        
        
    }
}
