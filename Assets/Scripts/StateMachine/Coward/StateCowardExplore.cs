using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CowardStates
{
    public class StateCowardExplore : IState
    {
        IAction action;
        bool forceChangeState = false; // force change state if no enemies are found

        public bool CanEnter(AIController controller, AIContext context)
        {
            return true; // can always enter
        }

        public bool CanExit(AIController controller, AIContext context)
        {
            return action.IsDone || forceChangeState; // check if the action is done
        }

        public void Enter(AIController controller, AIContext context)
        {
            action = new ActionMoveTo(context);
            controller.OnSeeTarget += OnTargetVisible;
            forceChangeState = false; // reset the flag
        }

        public void Execute(AIController controller, AIContext context)
        {
            if (action == null || action.IsDone)
            {
                action = new ActionMoveTo(context);
            }
            action.Execute(controller, context);
        }

        public IState Exit(AIController controller, AIContext context)
        {
            controller.OnSeeTarget -= OnTargetVisible; // unsubscribe from the event

            if (context.visibleTargets.Length > 0)
            {
                if (context.DEBUG)
                    Debug.Log("There are targets");

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

                if (context.visibleTargets.Length == 1)
                {
                    context.target = controller.GetClosestVisiblePawn();
                }
            }

            if (context.DEBUG)
                Debug.Log("I have found no enemies on the way here, I will patrol here.");
            return new StateCowardPatrol();
        }

        public void OnSound(AIController controller, AIContext context)
        {
            if (context.DEBUG)
                Debug.Log("Heard a sound, investigating...");
            forceChangeState = true;
        }

        public void OnTargetVisible(AIController controller, AIContext context)
        {
            forceChangeState = true; // force change state if an enemy is found
        }
    }
}
