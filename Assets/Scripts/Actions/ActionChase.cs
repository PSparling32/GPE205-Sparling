using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// For chasing after a target.
/// </summary>
public class ActionChase : IAction
{
    private bool isDone = false;
    private PIDController pidController;

    public ActionChase(AIContext context)
    {
        pidController = new PIDController(1.0f, 0.0f, 0.0f, context);
    }

    bool IAction.IsDone => isDone;

    void IAction.Execute(AIController controller, AIContext context)
    {
        //get the self and target pawn
        TankPawn self = context.self;
        TankPawn target = context.target;

        //if anything is null, set isDone. The needed components are not present
        if (self == null || target == null)
        {
            isDone = true;
            return;
        }

        //calculate where to go
        Vector3 directionToTarget = target.transform.position - self.transform.position;

        // Normalize the direction vector for easier movement calculations
        float distanceToTarget = directionToTarget.magnitude;

        // PID controller calculations

        // Check for collision prediction
        Vector3 avoidanceDirection;

        //get the output from the PID controller
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

    void IAction.Reset()
    {
        isDone = false;
        pidController.Reset();
    }
}
