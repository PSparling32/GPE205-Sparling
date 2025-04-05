using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefensiveStates
{
    public class StateDefensiveFlee : IState
    {
        IAction action;
        public float fleeTime = 0f; //time spent fleeing

        public bool CanEnter(AIController controller, AIContext context)
        {
            return true; //can always enter
        }

        public bool CanExit(AIController controller, AIContext context)
        {
            return action.IsDone
                || fleeTime > context.mimimumFleeTime
                || fleeTime > context.maximumFleeTime; //check if the action is done
        }

        public void Enter(AIController controller, AIContext context)
        {
            context.target = controller.GetClosestVisiblePawn();
            action = new ActionFlee(context);
        }

        public void Execute(AIController controller, AIContext context)
        {
            action.Execute(controller, context);
            fleeTime += Time.deltaTime;
        }

        public IState Exit(AIController controller, AIContext context)
        {
            if (context.DEBUG)
                Debug.Log("I have fled from the enemy.");
            return new StateDefensivePatrol();
        }

        public void OnSound(AIController controller, AIContext context)
        {
            return;
        }

        public void OnTargetVisible(AIController controller, AIContext context)
        {
            return;
        }
    }
}
