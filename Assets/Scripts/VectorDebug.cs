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
        // AngleAndDotProduct();
        ProjectionTest();
    }

    private void AngleAndDotProduct()
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
        Gizmos.DrawLine(nodePos, nodePos + (nodePos - carPos));

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(nodePos, nextNodePos);


        var lookVector = nodePos - carPos;

        var nodeVector = nextNodePos - nodePos;

        var angle = Vector3.Angle(lookVector, nodeVector);

        var dot = Vector3.Dot(velocity * lookVector.normalized, nodeVector.normalized);
        Handles.color = angle >= angleThreshold ? Color.red : Color.green;
        var style = new GUIStyle();
        style.fontSize = 20;
        Handles.Label(carPos + Vector3.right, $" angle : {angle}", style);
        Handles.Label(carPos + Vector3.left, $" dot : {dot}", style);
    }

    private void ProjectionTest()
    {
        var origin = car.position;
        var dest = node.position;
        var secondaryPoint = nextNode.position;

        var vector1 = dest - origin;
        var vector2 = secondaryPoint - origin;
        Gizmos.color = Color.red;

        Gizmos.DrawRay(origin, vector1);
        Gizmos.color = Color.blue;

        Gizmos.DrawRay(origin, vector2);

        var project = Vector3.Project(vector1, vector2.normalized);

        Gizmos.color = Color.yellow;
        // Gizmos.DrawRay(origin, project);

        var style = new GUIStyle();
        style.fontSize = 5;
        Handles.Label(origin + Vector3.left * 2, $" Point : {dest}", style);
        // x - origin = projected
        Handles.Label(origin + Vector3.right * 2, $" projected Point : {project - origin}", style);

        var d = Vector3.RotateTowards(vector1, vector2.normalized, 2*Mathf.PI, vector1.magnitude);
        
        Gizmos.DrawRay(origin, d);
    }
}