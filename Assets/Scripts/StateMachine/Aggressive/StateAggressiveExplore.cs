using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AggressiveStates
{
    public class StateAggressiveExplore : IState
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
            controller.OnSeeTarget += OnTargetVisible;
            action = new ActionMoveTo(context);
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
                context.target = controller.GetClosestVisiblePawn();
            }

            Debug.Log("I have found no enemies on the way here, I will patrol here.");
            return new StateAggressiveAttack();
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
