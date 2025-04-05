using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains all information for AI about the world, both
/// what it can percieve as well as some control parameters.
/// </summary>
[System.Serializable]
public struct AIContext
{
    public bool DEBUG;

    public TankPawn target;
    public TankPawn self;
    public float patrolDistance;
    public float patrolWaypointWaitTime;
    public float patrolWaypointCount;

    //decision making
    public float dangerDistance;
    public float roamDistance;
    public float mimimumFleeTime;
    public float maximumFleeTime;

    //senses
    public float visionDistance;
    public float visionAngle;
    public float visionRays;

    //data and information
    public TankPawn[] visibleTargets;
    public Vector3 destination;
}
