using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//handles patrolling
public class ActionPatrol : IAction
{
    //control variables
    private bool isDone = false;
    private List<Vector3> waypoints;
    private int currentWaypointIndex = 0;
    private float waitTime = 0.0f;

    private PIDController pidController;

    public ActionPatrol(AIContext context)
    {
        pidController = new PIDController(1.0f, 0.0f, 0.0f, context);
    }

    public bool IsDone => isDone;

    public void Execute(AIController controller, AIContext context)
    {
        TankPawn self = context.self; //get self

        if (self == null) //if there is not self, exit
        {
            isDone = true;
            return;
        }

        if (waypoints == null || waypoints.Count == 0) //if there are no waypoints, generate them
        {
            GenerateWaypoints(
                self.transform.position,
                context.patrolDistance,
                context.patrolWaypointCount
            );
        }

        if (waitTime > 0) //how long to wait at the waypoint
        {
            waitTime -= Time.deltaTime;
            return;
        }

        //used to determine the direction to the next waypoint
        Vector3 targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 directionToWaypoint = targetWaypoint - self.transform.position;
        float distanceToWaypoint = directionToWaypoint.magnitude;

        // PID controller calculations
        Vector3 avoidanceDirection;
        float output = pidController.Compute(
            distanceToWaypoint,
            Time.deltaTime,
            self,
            out avoidanceDirection
        );

        // Determine movement direction
        bool moveForward = output > 0;

        // Determine rotation direction
        Vector3 moveDirection = directionToWaypoint.normalized;
        float angleToMoveDirection = Vector3.SignedAngle(
            self.transform.forward,
            moveDirection,
            Vector3.up
        );
        bool rotateLeft = angleToMoveDirection < 0;

        // Adjust direction if necessary
        if (avoidanceDirection != Vector3.zero)
        {
            moveDirection = avoidanceDirection;
            rotateLeft = Vector3.SignedAngle(self.transform.forward, moveDirection, Vector3.up) < 0;
        }

        // Apply movement and rotation
        self.Move(moveForward);
        self.Rotate(rotateLeft);

        if (distanceToWaypoint < 1.0f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Count)
            {
                isDone = true;
            }
            else
            {
                waitTime = context.patrolWaypointWaitTime;
            }
        }
    }

    /// Resets the action
    public void Reset()
    {
        isDone = false;
        waypoints = null;
        currentWaypointIndex = 0;
        waitTime = 0.0f;
        pidController.Reset();
    }

    /// Generates waypoints in a circular pattern around the center point
    private void GenerateWaypoints(Vector3 center, float radius, float waypointCount)
    {
        waypoints = new List<Vector3>();
        float angleStep = 360.0f / waypointCount;

        for (int i = 0; i < waypointCount; i++)
        {
            float angle = i * angleStep;
            float radian = angle * Mathf.Deg2Rad;
            Vector3 waypoint = new Vector3(
                center.x + radius * Mathf.Cos(radian),
                center.y,
                center.z + radius * Mathf.Sin(radian)
            );
            waypoints.Add(waypoint);
        }
    }
}
