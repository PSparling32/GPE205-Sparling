using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefensiveStates
{
    public class StateDefensiveDefault : IState
    {
        private bool shouldFlee = false;
        private bool shouldAttack = false;

        public bool CanEnter(AIController controller, AIContext context)
        {
            if (context.DEBUG)
                Debug.Log("StateDefensiveDefault: I can always enter");
            return true;
        }

        public bool CanExit(AIController controller, AIContext context)
        {
            if (context.DEBUG)
                Debug.Log("StateDefensiveDefault: I can always exit");
            return true;
        }

        public void Enter(AIController controller, AIContext context)
        {
            controller.OnSeeTarget += OnTargetVisible;
        }

        public void Execute(AIController controller, AIContext context)
        {
            // Default state does nothing actively
            return;
        }

        public IState Exit(AIController controller, AIContext context)
        {
            // Check for visible targets
            if (context.visibleTargets.Length > 0)
            {
                if (context.DEBUG)
                    Debug.Log("There are targets");

                // Get the closest visible pawn
                Pawn pawn = controller.GetClosestVisiblePawn();

                // Check if the closest target is within danger distance or if there are multiple targets
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

                // If there is only one target, set it as the current target and switch to attack state
                if (context.visibleTargets.Length == 1)
                {
                    context.target = controller.GetClosestVisiblePawn();
                    return new StateDefensiveAttack();
                }
            }

            if (context.DEBUG)
                Debug.Log("I have nothing else to do, I will patrol");
            return new StateDefensivePatrol();
        }

        public void OnSound(AIController controller, AIContext context)
        {
            return;
        }

        public void OnTargetVisible(AIController controller, AIContext context)
        {
            // Check if there is only one target and it is within danger distance
            if (context.visibleTargets.Length == 1)
            {
                if (
                    Vector3.Distance(
                        context.visibleTargets[0].transform.position,
                        context.self.transform.position
                    ) < context.dangerDistance
                )
                {
                    if (context.DEBUG)
                        Debug.Log("There is one target, I should attack");
                    shouldAttack = true;
                }
            }
            // If there are multiple targets, flag to flee
            else if (context.visibleTargets.Length > 1)
            {
                if (context.DEBUG)
                    Debug.Log("There are multiple targets, I should flee");
                shouldFlee = true;
            }
        }
    }
}
