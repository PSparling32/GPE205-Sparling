using UnityEngine;

/// <summary>
/// PID Controller, used for controlling the movement of the tank.
/// </summary>
public class PIDController
{
    private float pGain; // proportional gain, current error
    private float iGain; // integral gain, accumulated error
    private float dGain; // potential future error
    private float previousError; // error during the last frame
    private float integral; // accumulated error
    private AIContext aiContext; // AI context for collision prediction

    public PIDController(float pGain, float iGain, float dGain, AIContext aiContext)
    {
        this.pGain = pGain;
        this.iGain = iGain;
        this.dGain = dGain;
        this.previousError = 0.0f;
        this.integral = 0.0f;
        this.aiContext = aiContext;
    }

    // Compute the PID output based on the error and delta time
    public float Compute(
        float error,
        float deltaTime,
        TankPawn self,
        out Vector3 avoidanceDirection
    )
    {
        avoidanceDirection = Vector3.zero;

        // PID calculations
        integral += error * deltaTime;
        float derivative = (error - previousError) / deltaTime;
        float output = pGain * error + iGain * integral + dGain * derivative;
        previousError = error;

        // Collision prediction
        if (PredictCollision(self, aiContext.patrolDistance, out avoidanceDirection))
        {
            // Adjust the output to avoid the obstacle
            output *= 0.5f; // Reduce speed to half when avoiding
            self.GetMovement().Rotate(Vector3.Dot(avoidanceDirection, self.transform.right) > 0);
        }

        return output;
    }

    public void Reset()
    {
        previousError = 0.0f;
        integral = 0.0f;
    }

    private bool PredictCollision(
        TankPawn self,
        float predictionTime,
        out Vector3 avoidanceDirection
    )
    {
        avoidanceDirection = Vector3.zero;
        Vector3 currentPosition = self.transform.position;
        Vector3 currentDirection = self.transform.forward;
        float speed = self.GetMovement().Speed;

        Vector3 predictedPosition = currentPosition + currentDirection * speed * predictionTime;

        int layerMask = 1 << 6; // Only consider layer 6

        RaycastHit hit;
        if (
            Physics.Raycast(
                currentPosition,
                currentDirection,
                out hit,
                speed * predictionTime,
                layerMask
            )
        )
        {
            avoidanceDirection = Vector3.Reflect(currentDirection, hit.normal);
            return true;
        }

        return false;
    }
}
