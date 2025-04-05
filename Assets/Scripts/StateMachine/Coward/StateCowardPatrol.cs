using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CowardStates
{
    public class StateCowardPatrol : IState
    {
        IAction currentAction;
        bool forceStateChange = false;

        public bool CanEnter(AIController controller, AIContext context)
        {
            return true; // can always enter
        }

        public bool CanExit(AIController controller, AIContext context)
        {
            if (context.DEBUG)
            {
                Debug.Log(currentAction.IsDone ? "I have finished patrolling." : "Enemy Spotted!");
            }
            return currentAction.IsDone || forceStateChange; // check if the action is done
        }

        public void Enter(AIController controller, AIContext context)
        {
            if (context.DEBUG)
                Debug.Log("I will patrol now.");
            currentAction = new ActionPatrol(context);
            controller.OnSeeTarget += OnTargetVisible; // subscribe to the event
            forceStateChange = false; // reset the flag
        }

        public void Execute(AIController controller, AIContext context)
        {
            if (currentAction == null || currentAction.IsDone)
            {
                currentAction = new ActionPatrol(context);
            }
            currentAction.Execute(controller, context);
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
                Debug.Log("I have found no enemies, I will go somewhere else and try again.");
            return new StateCowardExplore();
        }

        public void OnSound(AIController controller, AIContext context)
        {
            if (context.DEBUG)
                Debug.Log("Heard a sound, investigating...");
            forceStateChange = true;
        }

        public void OnTargetVisible(AIController controller, AIContext context)
        {
            forceStateChange = true;
        }
    }
}
