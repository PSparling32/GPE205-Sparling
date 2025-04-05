using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefensiveStates
{
    public class StateDefensiveExplore : IState
    {
        IAction action;

        bool forceChangeState = false; //force change state if no enemies are found

        public bool CanEnter(AIController controller, AIContext context)
        {
            return true; //can always enter
        }

        public bool CanExit(AIController controller, AIContext context)
        {
            return action.IsDone || forceChangeState; //check if the action is done
        }

        public void Enter(AIController controller, AIContext context)
        {
            action = new ActionMoveTo(context);
            controller.OnSeeTarget += OnTargetVisible;
        }

        public void Execute(AIController controller, AIContext context)
        {
            action.Execute(controller, context);
        }

        public IState Exit(AIController controller, AIContext context)
        {
            //if an enemy is spotted
            if (context.visibleTargets.Length > 0)
            {
                if (context.DEBUG)
                    Debug.Log("There are targets");
                //is it inside the danger distance?
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
                    return new StateDefensiveFlee();
                }

                //Is there only one target?
                if (context.visibleTargets.Length == 1)
                {
                    context.target = controller.GetClosestVisiblePawn();
                    return new StateDefensiveAttack();
                }
            }
            if (context.DEBUG)
                Debug.Log("I have found no enemies on the way here, I will patrol here.");
            return new StateDefensivePatrol();
        }

        public void OnSound(AIController controller, AIContext context)
        {
            throw new System.NotImplementedException();
        }

        public void OnTargetVisible(AIController controller, AIContext context)
        {
            forceChangeState = true; //force change state if an enemy is found
        }
    }
}
