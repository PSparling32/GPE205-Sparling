using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AggressiveStates
{
    public class StateAggressiveChase : IState
    {
        IAction action;

        public bool CanEnter(AIController controller, AIContext context)
        {
            return context.target != null; //can only enter if there is a target
        }

        public bool CanExit(AIController controller, AIContext context)
        {
            return Vector3.Distance(
                    context.target.transform.position,
                    controller.transform.position
                )
                    > controller.GetPawn().GetShooter().GetFireDistance() * .9f
                || context.target == null
                || action.IsDone; //can exit if the target is out of range or null
        }

        public void Enter(AIController controller, AIContext context)
        {
            action = new ActionChase(context);
            controller.OnSeeTarget += OnTargetVisible;
        }

        public void Execute(AIController controller, AIContext context)
        {
            action.Execute(controller, context);
        }

        public IState Exit(AIController controller, AIContext context)
        {
            if (context.target == null)
            {
                return new StateAggresivePatrol();
            }
            else
            {
                return new StateAggressiveAttack();
            }
        }

        public void OnSound(AIController controller, AIContext context)
        {
            return; // all that matters is destroying the target
        }

        public void OnTargetVisible(AIController controller, AIContext context)
        {
            return; // all that matters is destroying the target
        }
    }
}
