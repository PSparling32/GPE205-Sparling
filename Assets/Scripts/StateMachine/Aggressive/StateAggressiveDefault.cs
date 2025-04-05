using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AggressiveStates
{
    public class StateAggressiveDefault : IState
    {
        public bool CanEnter(AIController controller, AIContext context)
        {
            Debug.Log("StateAggressiveDefault: I can always enter");
            return true;
        }

        public bool CanExit(AIController controller, AIContext context)
        {
            Debug.Log("StateAggresiveDefault: I can always exit");
            return true;
        }

        public void Enter(AIController controller, AIContext context)
        {
            controller.OnSeeTarget += OnTargetVisible;
        }

        public void Execute(AIController controller, AIContext context)
        {
            return;
        }

        public IState Exit(AIController controller, AIContext context)
        {
            //if there is a target, attack it
            if (context.visibleTargets.Length > 0)
            {
                context.target = controller.GetClosestVisiblePawn();
                return new StateAggressiveAttack();
            }

            Debug.Log("I have nothing else to do, i will patrol");
            return new StateAggresivePatrol();
        }

        public void OnSound(AIController controller, AIContext context) { }

        public void OnTargetVisible(AIController controller, AIContext context)
        {
            if (context.visibleTargets.Length == 1)
            {
                //is it inside the danger distance?
                if (
                    Vector3.Distance(
                        context.visibleTargets[0].transform.position,
                        context.self.transform.position
                    ) < context.dangerDistance
                )
                {
                    Debug.Log("There is one target, i should attack");
                }
            }
        }
    }
}
