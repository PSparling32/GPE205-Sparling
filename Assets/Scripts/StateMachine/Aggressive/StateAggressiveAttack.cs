using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AggressiveStates
{
    public class StateAggressiveAttack : IState
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
                    return false; //no target to attack
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
            action = new ActionFire();
        }

        public void Execute(AIController controller, AIContext context)
        {
            action.Execute(controller, context);
        }

        public IState Exit(AIController controller, AIContext context)
        {
            //are there any targets in sight?
            Debug.Log("I have nothing else to do, i will patrol");
            if (context.target != null) //is it still alive
            {
                return new StateAggressiveAttack();
            }
            else
            {
                return new StateAggresivePatrol();
            }
        }

        public void OnSound(AIController controller, AIContext context) { }

        public void OnTargetVisible(AIController controller, AIContext context) { }
    }
}
