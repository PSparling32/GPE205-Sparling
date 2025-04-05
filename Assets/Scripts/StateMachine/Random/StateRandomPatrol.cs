using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomStates
{
    public class StateRandomPatrol : IState
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
                if (currentAction.IsDone)
                {
                    Debug.Log("I have finished patrolling.");
                }
                else
                {
                    if (context.visibleTargets.Length > 0)
                        Debug.Log("I have spotted an enemy.");
                    else
                        Debug.Log("I have not spotted any enemies.");
                }
            return currentAction.IsDone || forceStateChange; // check if the action is done
        }

        public void Enter(AIController controller, AIContext context)
        {
            Debug.Log("I will patrol randomly now.");
            currentAction = new ActionPatrol(context);
            controller.OnSeeTarget += OnTargetVisible; // subscribe to the event
        }

        public void Execute(AIController controller, AIContext context)
        {
            currentAction.Execute(controller, context);
        }

        public IState Exit(AIController controller, AIContext context)
        {
            // if an enemy is spotted
            if (context.visibleTargets.Length > 0)
            {
                Debug.Log("I have found an enemy, I will flee.");
                context.target = controller.GetClosestVisiblePawn();
                return new StateRandomFlee();
            }

            // Debug.Log("I have found no enemies, I will go somewhere else and try again.");
            return new StateRandomExplore();
        }

        public void OnSound(AIController controller, AIContext context)
        {
            throw new System.NotImplementedException();
        }

        public void OnTargetVisible(AIController controller, AIContext context)
        {
            forceStateChange = true;
        }
    }
}
