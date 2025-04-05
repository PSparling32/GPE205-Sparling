using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefensiveStates
{
    public class StateDefensivePatrol : IState
    {
        IAction currentAction;

        bool forceSateChange = false;

        public bool CanEnter(AIController controller, AIContext context)
        {
            return true; //can always enter
        }

        public bool CanExit(AIController controller, AIContext context)
        {
            if (currentAction.IsDone)
            {
                if (context.DEBUG)
                    Debug.Log("I have finished patrolling.");
            }
            else
            {
                if (context.DEBUG)
                    Debug.Log("Enemy Spotted!");
            }
            return currentAction.IsDone || forceSateChange; //check if the action is done
        }

        public void Enter(AIController controller, AIContext context)
        {
            if (context.DEBUG)
                Debug.Log("I will patrol now.");
            currentAction = new ActionPatrol(context);
            controller.OnSeeTarget += OnTargetVisible; //subscribe to the event
        }

        public void Execute(AIController controller, AIContext context)
        {
            currentAction.Execute(controller, context);
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
                Debug.Log("I have found no enemies, I will go somewhere else and try again.");
            return new StateDefensiveExplore();
        }

        public void OnSound(AIController controller, AIContext context)
        {
            throw new System.NotImplementedException();
        }

        public void OnTargetVisible(AIController controller, AIContext context)
        {
            forceSateChange = true;
        }
    }
}
