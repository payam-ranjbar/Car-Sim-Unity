using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Navigation;
using UnityEngine;
using UnityEngine.AI;

public class PathFinder : MonoBehaviour
{

    public Transform target;
    private NavMeshPath path;
    private float elapsed = 0.0f;
    public bool draw;
    public float jumpLenght = 1f;
    public float jumpHeight = 1f;
    private Vector3 _dest;

    private int _index;

    private bool _endOfPath;
    private bool _isGrounded;
    private Vector3 _startPos;
    [SerializeField] private float speed;
    [SerializeField] private float minDistance = 0f;
    [SerializeField] private float radius;
    [SerializeField] private AnimationCurve curve;
    
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private float _fallMultiplier = 1.6f;
    [SerializeField] private float rayLength = 1f;

    private bool _isJumping;

    private float Distance => Vector3.Distance(transform.position, Destination());
    // Start is called before the first frame update
    void Start()
    {
        path = new NavMeshPath();
        _startPos = transform.position;
        CalculatePath();
    }


    // Update is called once per frame
    void Update()
    {
        if (!draw) return;
        Debug.DrawRay(transform.position, Vector3.down * rayLength, Color.blue);
        Debug.DrawLine(transform.position, _dest, Color.yellow);
        for (int i = 0; i < path.corners.Length - 1; i++)
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
    }
    
    [ContextMenu("Calculate")]
    private void CalculatePath()
    {
        NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, path);
    }
    
    private Vector3 Destination()
    {
        var pos = transform.position;
        return new Vector3(_dest.x, pos.y, _dest.z);
    }

    private float DistanceTo(Vector3 v)
    {
        var pos = transform.position;
        return Vector3.Distance(pos, new Vector3(v.x, pos.y, v.z));
    }

    private bool ReachedDest()
    {
        if (Vector3.Distance(transform.position, Destination()) <= minDistance) return true;
        return false;
    }


    private void OnDrawGizmos()
    {
        if (!draw) return;
        Debug.DrawLine(transform.position, transform.position +Vector3.down * rayLength, Color.blue);
        if (path is null) return;
        Gizmos.color = Color.yellow;
        Debug.DrawLine(transform.position, _dest, Color.yellow);
        for (int i = 0; i < path.corners.Length - 1; i++)
            Gizmos.DrawSphere(path.corners[i], radius);
    }

    public Waypoint[] GetWaypoints()
    {
        var waypoint = new Waypoint[path.corners.Length];

        for (int i = 0; i < path.corners.Length; i++)
        {
            var way = new GameObject();
            
        }

        return null;
    }
}
