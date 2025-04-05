using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : Controller
{
    [SerializeField]
    PerceptionData perceptionData;

    IAction currentAction;

    [SerializeField]
    protected AIContext context;

    //Delegates
    public delegate void OnSeeTargetHandler(AIController controller, AIContext context);
    public event OnSeeTargetHandler OnSeeTarget;

    protected StateMachine stateMachine;

    // Start is called before the first frame update
    void Start()
    {
        name = "AIController";
        SetUp();
        context.self = GetPawn();
    }

    protected void AIUpdate()
    {
        VisionCheck();

        stateMachine.Update(this, context);
    }

    // Update is called once per frame
    protected void ExecuteAction()
    {
        currentAction?.Execute(this, context);
    }

    protected void SetAction(IAction action)
    {
        action.Reset();
        currentAction = action;
    }

    public void CanHear(NoiseData data)
    {
        Vector3 noiseLocation = data.Location;
        float noiseRange = data.Distance;

        float distance = Vector3.Distance(transform.position, noiseLocation) - noiseRange;

        if (distance < perceptionData.HearingDistance)
        {
            Debug.Log("Heard something");
        }
    }

    public bool CanSee(TankPawn target)
    {
        return true;
    }

    public TankPawn GetPawn()
    {
        return GetComponent<TankPawn>();
    }

    //returns true if the barrel is facing the target
    public bool IsBarrelIsFacingTarget(TankPawn target)
    {
        Vector3 barrelPosition = GetPawn().GetShooter().GetBarrelPosition();
        Vector3 barrelForward = GetPawn().GetShooter().GetBarrelForward();

        RaycastHit hit;
        if (Physics.Raycast(barrelPosition, barrelForward, out hit))
        {
            if (hit.transform.CompareTag("Tank"))
            {
                TankPawn tank = hit.transform.GetComponent<TankPawn>();
                if (tank == target)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void VisionCheck() //works fov based on the information in AIContext
    {
        // Get the barrel position and forward direction
        Vector3 barrelPosition = GetPawn().GetShooter().GetBarrelPosition();
        Vector3 barrelForward = GetPawn().GetShooter().GetBarrelForward();

        // Calculate the angle per ray
        float anglePerRay = context.visionAngle / context.visionRays;

        HashSet<TankPawn> visibleTargets = new HashSet<TankPawn>();

        for (int i = 0; i < context.visionRays; i++)
        {
            // Calculate the direction for each ray
            float currentAngle = -context.visionAngle / 2 + anglePerRay * i;
            Vector3 direction = Quaternion.Euler(0, currentAngle, 0) * barrelForward;

            // Perform the raycast
            if (
                Physics.Raycast(
                    barrelPosition,
                    direction,
                    out RaycastHit hit,
                    context.visionDistance
                )
            )
            {
                TankPawn tankPawn = hit.collider.GetComponent<TankPawn>();
                if (tankPawn != null && tankPawn != GetPawn())
                {
                    visibleTargets.Add(tankPawn);
                    OnSeeTarget?.Invoke(this, context);
                }
            }
        }

        context.visibleTargets = new List<TankPawn>(visibleTargets).ToArray();
    }

    public TankPawn GetClosestVisiblePawn()
    {
        TankPawn closestPawn = null;
        float closestDistance = Mathf.Infinity;
        foreach (TankPawn pawn in context.visibleTargets)
        {
            float distance = Vector3.Distance(transform.position, pawn.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPawn = pawn;
            }
        }
        return closestPawn;
    }

    public void OnDrawGizmosSelected()
    {
        //draws the danger distance
        Gizmos.color = Color.red;
        if (context.dangerDistance != null || context.dangerDistance != 0)
        {
            Gizmos.DrawWireSphere(transform.position, context.dangerDistance);
        }

        //draws the vision cone
        Gizmos.color = Color.yellow;

        if (context.visionAngle != 0 && context.visionDistance != 0 && context.visionRays != 0)
        {
            Vector3 position = transform.position;
            Vector3 forward = transform.forward;

            float anglePerRay = context.visionAngle / context.visionRays;

            for (int i = 0; i < context.visionRays; i++)
            {
                // Calculate the direction for each ray
                float currentAngle = -context.visionAngle / 2 + anglePerRay * i;
                Vector3 direction = Quaternion.Euler(0, currentAngle, 0) * forward;
                Vector3 rayEnd = position + direction * context.visionDistance;
                Gizmos.DrawLine(position, rayEnd);
            }
        }
    }
}
