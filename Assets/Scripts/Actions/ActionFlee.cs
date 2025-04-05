using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ActionFlee is used to move away from a speified target.
/// </summary>
public class ActionFlee : IAction
{
    private bool isDone = false;
    private PIDController pidController;

    public ActionFlee(AIContext context)
    {
        pidController = new PIDController(1.0f, 0.0f, 0.0f, context);
    }

    bool IAction.IsDone => isDone;

    void IAction.Execute(AIController controller, AIContext context)
    {
        TankPawn self = context.self;
        TankPawn target = context.target;

        if (self == null || target == null)
        {
            isDone = true;
            return;
        }

        Vector3 directionToTarget = target.transform.position - self.transform.position;
        float distanceToTarget = directionToTarget.magnitude;

        // PID controller calculations
        Vector3 avoidanceDirection;
        float output = pidController.Compute(
            distanceToTarget,
            Time.deltaTime,
            self,
            out avoidanceDirection
        );

        // Determine movement direction
        bool moveForward = output > 0;

        // Determine rotation direction
        Vector3 fleeDirection = -directionToTarget.normalized;
        float angleToFleeDirection = Vector3.SignedAngle(
            self.transform.forward,
            fleeDirection,
            Vector3.up
        );
        bool rotateLeft = angleToFleeDirection < 0;

        // Adjust direction if necessary
        if (avoidanceDirection != Vector3.zero)
        {
            fleeDirection = avoidanceDirection;
            rotateLeft = Vector3.SignedAngle(self.transform.forward, fleeDirection, Vector3.up) < 0;
        }

        // Apply movement and rotation
        self.Move(moveForward);
        self.Rotate(rotateLeft);
    }

    void IAction.Reset()
    {
        isDone = false;
        pidController.Reset();
    }
}
