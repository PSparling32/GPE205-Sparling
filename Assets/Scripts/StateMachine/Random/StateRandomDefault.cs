using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomStates
{
    public class StateRandomDefault : IState
    {
        private System.Random random = new System.Random();

        public bool CanEnter(AIController controller, AIContext context)
        {
            if (context.DEBUG)
                Debug.Log("StateRandomDefault: I can always enter");
            return true;
        }

        public bool CanExit(AIController controller, AIContext context)
        {
            if (context.DEBUG)
                Debug.Log("StateRandomDefault: I can always exit");
            return true;
        }

        public void Enter(AIController controller, AIContext context)
        {
            controller.OnSeeTarget += OnTargetVisible;
        }

        public void Execute(AIController controller, AIContext context)
        {
            if (context.DEBUG)
                Debug.Log("StateRandomDefault: Executing random behavior");

            // Randomly decide to move, rotate, or do nothing
            int action = random.Next(0, 3);
            switch (action)
            {
                case 0:
                    controller.GetPawn().Move(true);
                    if (context.DEBUG)
                        Debug.Log("StateRandomDefault: Moving forward");
                    break;
                case 1:
                    controller.GetPawn().Rotate(random.Next(0, 2) == 0);
                    if (context.DEBUG)
                        Debug.Log("StateRandomDefault: Rotating");
                    break;
                case 2:
                    if (context.DEBUG)
                        Debug.Log("StateRandomDefault: Doing nothing");
                    break;
            }
        }

        public IState Exit(AIController controller, AIContext context)
        {
            // Randomly decide the next state
            int nextState = random.Next(0, 3);
            switch (nextState)
            {
                case 0:
                    if (context.DEBUG)
                        Debug.Log("StateRandomDefault: Transitioning to StateCowardFlee");
                    return new StateRandomFlee();
                case 1:
                    if (context.DEBUG)
                        Debug.Log("StateRandomDefault: Transitioning to StateCowardPatrol");
                    return new StateRandomPatrol();
                case 2:
                    if (context.DEBUG)
                        Debug.Log("StateRandomDefault: Staying in StateRandomDefault");
                    return this;
            }

            if (context.DEBUG)
                Debug.Log("StateRandomDefault: Defaulting to StateCowardPatrol");
            return new StateRandomPatrol();
        }

        public void OnSound(AIController controller, AIContext context)
        {
            if (context.DEBUG)
                Debug.Log("StateRandomDefault: Heard a sound, investigating...");
            // Implement sound handling logic if needed
        }

        public void OnTargetVisible(AIController controller, AIContext context) { }
    }
}
