using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CowardStates
{
    public class StateCowardAttack : IState
    {
        IAction action;

        public bool CanEnter(AIController controller, AIContext context)
        {
            if (context.target == null)
            {
                context.target = controller.GetClosestVisiblePawn();
                if (context.target == null)
                {
                    Debug.Log("I have no target to attack.");
                    return false; // no target to attack
                }
            }
            return true;
        }

        public bool CanExit(AIController controller, AIContext context)
        {
            return action.IsDone;
        }

        public void Enter(AIController controller, AIContext context)
        {
            if (action != null)
            {
                action.Reset();
            }
            action = new ActionFire();
            Debug.Log("Entering Attack State");
        }

        public void Execute(AIController controller, AIContext context)
        {
            action.Execute(controller, context);
        }

        public IState Exit(AIController controller, AIContext context)
        {
            // Check if there are any targets in sight
            if (context.visibleTargets.Length > 0)
            {
                if (context.DEBUG)
                    Debug.Log("There are targets");

                // Check if the closest target is within the danger distance
                Pawn pawn = controller.GetClosestVisiblePawn();
                if (
                    pawn != null
                    && Vector3.Distance(controller.transform.position, pawn.transform.position)
                        < context.dangerDistance
                )
                {
                    Debug.Log("Enemy too close, run away!");
                    return new StateCowardFlee();
                }
            }

            if (context.DEBUG)
                Debug.Log("I have nothing else to do, I will patrol");

            return new StateCowardPatrol();
        }

        public void OnSound(AIController controller, AIContext context) { }

        public void OnTargetVisible(AIController controller, AIContext context) { }
    }
}
