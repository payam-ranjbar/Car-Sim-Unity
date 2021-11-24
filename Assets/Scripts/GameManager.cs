using System.Collections;
using System.Collections.Generic;
using GameGlobals;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameProperties gameProperties;

    [SerializeField] private Transform peopleTransform;

    [SerializeField] private Transform intersectionTransform;
    // Start is called before the first frame update
    void Start()
    {
        SetIntersectionElevation();
    }

    private void SetIntersectionElevation()
    {
        var elevation = gameProperties.InterSectionElevation;
        var peoplePos = peopleTransform.position;
        peopleTransform.position = new Vector3(peoplePos.x, peoplePos.y + elevation,
            peoplePos.z);
    
        var intersectionPos =intersectionTransform.position;
        intersectionTransform.position = new Vector3(intersectionPos.x, intersectionPos.y + elevation,
            intersectionPos.z);
    }
    
}
