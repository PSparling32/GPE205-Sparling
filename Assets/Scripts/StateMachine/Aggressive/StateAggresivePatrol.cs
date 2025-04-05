using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// for the Aggressive AI
namespace AggressiveStates
{
    /// <summary>
    /// Aggressive AI Patrol State
    /// </summary>
    public class StateAggresivePatrol : IState
    {
        IAction currentAction;

        bool forceSateChange = false;

        public bool CanEnter(AIController controller, AIContext context)
        {
            return true; //can always enter
        }

        public bool CanExit(AIController controller, AIContext context)
        {
            //print debug messages if the DEBUG flag is set
            if (context.DEBUG)
                if (currentAction.IsDone)
                {
                    Debug.Log("I have finished patroling.");
                }
                else
                {
                    if (context.visibleTargets.Length > 0)
                        Debug.Log("I have spotted an enemy.");
                    else
                        Debug.Log("I have not spotted any enemies.");
                }
            //if forced state or patrol is done, return true to change state
            return currentAction.IsDone || forceSateChange; //check if the action is done
        }

        public void Enter(AIController controller, AIContext context)
        {
            Debug.Log("I will patrol now.");
            //setup patrol action
            currentAction = new ActionPatrol(context);
            controller.OnSeeTarget += OnTargetVisible; //subscribe to the event
        }

        public void Execute(AIController controller, AIContext context)
        {
            //preform patrol action
            currentAction.Execute(controller, context);
        }

        public IState Exit(AIController controller, AIContext context)
        {
            //if an enemy is spotted
            if (context.visibleTargets.Length > 0)
            {
                Debug.Log("I have found an enemy, I will attack.");
                context.target = controller.GetClosestVisiblePawn();
                return new StateAggressiveAttack();
            }

            //Debug.Log("I have found no enemies, I will go somewhere else and try again.");
            return new StateAggressiveExplore();
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
