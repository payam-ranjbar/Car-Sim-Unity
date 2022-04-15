using System;
using System.Globalization;
using System.Xml;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.XR;

public class VectorDebug : MonoBehaviour
{

    [SerializeField] private Transform car;
    [SerializeField] private Transform node;
    [SerializeField] private Transform nextNode;

    [Range(0f, 360f)]
    [SerializeField] private float angleThreshold = 40f;
    [Range(0f, 360f)]
    [SerializeField] private float velocity = 40f;
    private void OnDrawGizmos()
    {
        var carPos = car.position;
        var nodePos = node.position;
        var nextNodePos = nextNode.position;

        Gizmos.color = Color.black;
        Gizmos.DrawSphere(carPos, 0.2f);
        Gizmos.DrawSphere(nodePos, 0.2f);
        Gizmos.DrawSphere(nextNodePos, 0.2f);

        Gizmos.color = Color.gray;
        Gizmos.DrawLine(carPos, nodePos);
        
        Gizmos.color = Color.red;
        Gizmos.DrawLine(nodePos,  nodePos + (nodePos - carPos) );

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(nodePos, nextNodePos);
        
        
        var lookVector =nodePos - carPos;

        var nodeVector = nextNodePos - nodePos;

        var angle = Vector3.Angle(lookVector, nodeVector);

        var dot = Vector3.Dot(velocity * lookVector.normalized, nodeVector.normalized);
        Handles.color = angle >= angleThreshold ? Color.red : Color.green;
        var style = new GUIStyle();
        style.fontSize = 20;
        Handles.Label(carPos + Vector3.right, $" angle : {angle}", style);
        Handles.Label(carPos + Vector3.left, $" dot : {dot}", style);

    }
    
}