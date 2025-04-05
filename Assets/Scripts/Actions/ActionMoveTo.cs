using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generic move to that uses a PID controller to move to a position.
/// </summary>
public class ActionMoveTo : IAction
{
    private bool isDone = false;
    private PIDController pidController;
    private Vector3 targetPosition;

    public ActionMoveTo(AIContext context)
    {
        targetPosition = context.destination;
        pidController = new PIDController(1.0f, 0.0f, 0.0f, context);
    }

    public bool IsDone => isDone;

    public void Execute(AIController controller, AIContext context)
    {
        TankPawn self = context.self;

        if (self == null)
        {
            isDone = true;
            return;
        }

        Vector3 directionToTarget = targetPosition - self.transform.position;
        float distanceToTarget = directionToTarget.magnitude;

        // Check if the target position is reached
        if (distanceToTarget < 0.1f)
        {
            isDone = true;
            return;
        }

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
        Vector3 moveDirection = directionToTarget.normalized;
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
    }

    public void Reset()
    {
        isDone = false;
        pidController.Reset();
    }
}
