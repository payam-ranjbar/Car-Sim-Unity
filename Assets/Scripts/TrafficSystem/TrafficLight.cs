using System.Collections;
using GameEventSystem;
using TrafficSystem;
using UnityEngine;

public class TrafficLight : MonoBehaviour
{
    public Intersection TargetIntersection;
    public TrafficLightConfig Config;
    public CarDetector detector;

    [Header("Override config")] public bool OverRide;
    public int RedLightTime;
    public int GreenLightTime;
    
    private int _redLightTimeCounter;
    private int _greenLightTimeCounter;

    private int _redNumber;
    private int _greenNumber;

    IEnumerator CountDown(bool we_red)
    {
        int redTime;
        int greenTime;
        var redCounter = redTime = GetRedLightTime();
        var greenCounter = greenTime  = GetGreenLightTime();

         // if WE == RED ==> NS == Green
        
        while (redCounter >= 0 || greenCounter >= 0)
        {
            var westEastCounter = we_red ? redCounter : greenCounter;
            var northSouthCounter = !we_red ? redCounter : greenCounter;

            if (westEastCounter >= 0)
            {
                TargetIntersection.SetWELaneLightNumber(westEastCounter, we_red);
            }

            if (northSouthCounter >= 0)
            {
                TargetIntersection.SetNSLaneLightNumber(northSouthCounter, !we_red);

            }

            greenCounter--;
            redCounter--;
            yield return new WaitForSeconds(1f);
        }

        yield return null;
    }

    IEnumerator RedLight(Direction direction)
    {
        yield return CountDown(direction == Direction.WE);
    }


    private void Start()
    {
        StartCoroutine(TrafficLightCounter());
    }

    /**
         *       N/z
         * W/-x            E/x
         *       S/-z
         */
    private IEnumerator TrafficLightCounter()
    {
        var sideWalkLight = true; // true -> we green sn red, false -> we red, sn green
        while (true)
        {
            // NS Red
            TargetIntersection.SetCarLane(Direction.NS, true);

            yield return new WaitUntil(() => !detector.HasCarsOnIt);

            // WE Green
            TargetIntersection.SetCarLane(Direction.WE, false);
            TargetIntersection.ActiveWEGreenLightBlubs();

            yield return RedLight(Direction.NS);


            // WE Red
            TargetIntersection.SetCarLane(Direction.WE, true);

            yield return new WaitUntil(() => !detector.HasCarsOnIt);
            // NS Green
            TargetIntersection.SetCarLane(Direction.NS, false);

            TargetIntersection.ActiveNSGreenLightBlubs();
            yield return RedLight(Direction.WE);
        }
    }

    private int GetRedLightTime()
    {
        var val = OverRide ? RedLightTime : Config.RedLightTime;
        return val;
    }
    
    private int GetGreenLightTime()
    {
        var val = OverRide ?  GreenLightTime: Config.GreenLightTime;
        return val;
    }
    
}
