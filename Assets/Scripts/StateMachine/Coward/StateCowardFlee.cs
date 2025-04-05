using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CowardStates
{
    public class StateCowardFlee : IState
    {
        IAction action;
        float fleeTime = 0f; // time spent fleeing

        public bool CanEnter(AIController controller, AIContext context)
        {
            return true; // can always enter
        }

        public bool CanExit(AIController controller, AIContext context)
        {
            return action.IsDone || fleeTime >= context.mimimumFleeTime; // check if the action is done or minimum flee time is reached
        }

        public void Enter(AIController controller, AIContext context)
        {
            if (context.DEBUG)
                Debug.Log("Entering Flee State");

            context.target = controller.GetClosestVisiblePawn();
            action = new ActionFlee(context);
            fleeTime = 0f; // reset flee time
        }

        public void Execute(AIController controller, AIContext context)
        {
            action.Execute(controller, context);
            fleeTime += Time.deltaTime;
        }

        public IState Exit(AIController controller, AIContext context)
        {
            if (context.DEBUG)
                Debug.Log("Exiting Flee State");
            return new StateCowardDefault();
        }

        public void OnSound(AIController controller, AIContext context)
        {
            // No specific behavior on sound in flee state
        }

        public void OnTargetVisible(AIController controller, AIContext context)
        {
            // No specific behavior on target visible in flee state
        }
    }
}
