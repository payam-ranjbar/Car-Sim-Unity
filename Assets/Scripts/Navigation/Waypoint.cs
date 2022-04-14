using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Navigation
{
    public class Waypoint : MonoBehaviour
    {
        [SerializeField] [Range(0f, 2f)] private float rightOffset;
        [SerializeField] [Range(0f, 2f)] private float leftOffset;

        
        [SerializeField] private Vector3 upperTangentOffset;
        [SerializeField] private Vector3 lowerTangentOffset;
        [SerializeField] private bool useWings;
        
        
        [SerializeField]  [HideInInspector]private bool xAxis = true;
        public Color DrawColor { get; set; }
        public float RightOffset => rightOffset;

        public float LeftOffset => leftOffset;

        public Vector3 UpperTangentOffset
        {
            get => upperTangentOffset + transform.position;
            set => upperTangentOffset = value;
        }

        public Vector3 LowerTangentOffset
        {
            get => lowerTangentOffset + transform.position;
            set => lowerTangentOffset = value;
        }

        private Vector3 recentPos;
        

        public bool XAxis => xAxis;

        public Vector3 Position => GetPosition();

        public void RotatePolarity()
        {
            xAxis = !xAxis;
        }

        private Vector3 GetPosition()
        {
            if (!useWings) return transform.position;
            if (xAxis)
            {
                var x = transform.position.x + Random.Range(-LeftOffset, RightOffset);
                return new Vector3(x, transform.localPosition.y, transform.position.z);
            }
            else
            {
                var z = transform.position.z + Random.Range(-LeftOffset, RightOffset);
                return new Vector3(transform.position.x, transform.localPosition.y, z);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = DrawColor;   
            Gizmos.DrawSphere(transform.position, 0.1f);
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(UpperTangentOffset, 0.1f);
            Gizmos.DrawLine(transform.position, UpperTangentOffset);

            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(LowerTangentOffset, 0.1f);
            Gizmos.DrawLine(transform.position, LowerTangentOffset);

            if (useWings)
            {
                var currentNode = transform.position;
                if (XAxis)
                {
                    var right = new Vector3(currentNode.x + RightOffset, currentNode.y, currentNode.z);
                    Gizmos.DrawLine(currentNode, right);
                    var left = new Vector3(currentNode.x - LeftOffset, currentNode.y, currentNode.z);
                    Gizmos.DrawLine(currentNode, left);


                }
                else
                {
                    var right = new Vector3(currentNode.x, currentNode.y, currentNode.z + RightOffset);
                    Gizmos.DrawLine(currentNode, right);
                    var left = new Vector3(currentNode.x, currentNode.y, currentNode.z - LeftOffset);
                    Gizmos.DrawLine(currentNode, left);
                }
            }


        }
    }
}