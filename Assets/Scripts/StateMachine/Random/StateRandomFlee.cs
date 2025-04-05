using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomStates
{
    public class StateRandomFlee : IState
    {
        IAction action;
        public float fleeTime = 0f; // time spent fleeing

        public bool CanEnter(AIController controller, AIContext context)
        {
            return true; // can always enter
        }

        public bool CanExit(AIController controller, AIContext context)
        {
            return action.IsDone
                || fleeTime > context.mimimumFleeTime
                || fleeTime > context.maximumFleeTime; // check if the action is done
        }

        public void Enter(AIController controller, AIContext context)
        {
            context.target = GetRandomVisiblePawn(context.visibleTargets);
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
                Debug.Log("I have fled randomly from the enemy.");
            return new StateRandomPatrol();
        }

        public void OnSound(AIController controller, AIContext context)
        {
            return;
        }

        public void OnTargetVisible(AIController controller, AIContext context)
        {
            return;
        }

        private TankPawn GetRandomVisiblePawn(TankPawn[] visibleTargets)
        {
            if (visibleTargets == null || visibleTargets.Length == 0)
                return null;

            int randomIndex = Random.Range(0, visibleTargets.Length);
            return visibleTargets[randomIndex];
        }
    }
}
