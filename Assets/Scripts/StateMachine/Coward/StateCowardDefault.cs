using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CowardStates
{
    public class StateCowardDefault : IState
    {
        public bool CanEnter(AIController controller, AIContext context)
        {
            if (context.DEBUG)
                Debug.Log("StateCowardDefault: I can always enter");
            return true;
        }

        public bool CanExit(AIController controller, AIContext context)
        {
            if (context.DEBUG)
                Debug.Log("StateCowardDefault: I can always exit");
            return true;
        }

        public void Enter(AIController controller, AIContext context)
        {
            controller.OnSeeTarget += OnTargetVisible;
        }

        public void Execute(AIController controller, AIContext context)
        {
            // Minimal logic for the default state
            if (context.DEBUG)
                Debug.Log("StateCowardDefault: Executing default behavior");
        }

        public IState Exit(AIController controller, AIContext context)
        {
            // Are there any targets in sight?
            if (context.visibleTargets.Length > 0)
            {
                if (context.DEBUG)
                    Debug.Log("There are targets");

                // Is it inside the danger distance?
                Pawn pawn = controller.GetClosestVisiblePawn();
                if (
                    pawn != null
                        && Vector3.Distance(controller.transform.position, pawn.transform.position)
                            < context.dangerDistance
                    || context.visibleTargets.Length > 1
                )
                {
                    if (context.DEBUG)
                        Debug.Log("Enemy too close, run away!");
                    return new StateCowardFlee();
                }

                // Is there only one target?
                if (context.visibleTargets.Length == 1)
                {
                    context.target = controller.GetClosestVisiblePawn();
                }
            }

            if (context.DEBUG)
                Debug.Log("I have nothing else to do, I will patrol");
            return new StateCowardPatrol();
        }

        public void OnSound(AIController controller, AIContext context)
        {
            if (context.DEBUG)
                Debug.Log("Heard a sound, investigating...");
        }

        public void OnTargetVisible(AIController controller, AIContext context) { }
    }
}
