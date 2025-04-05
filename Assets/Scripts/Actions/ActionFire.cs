using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionFire : IAction
{
    bool hasFired = false;
    bool IAction.IsDone => hasFired;

    void IAction.Execute(AIController controller, AIContext context)
    {
        if (context.target == null)
        {
            hasFired = true; //is done due to no target
            return;
        }
        //rotate the character to face the target
        var direction = controller.transform.position - context.target.transform.position;
        //figure out if it needs to rotate left or right
        var angle = Vector3.SignedAngle(controller.transform.forward, direction, Vector3.forward);
        if (angle > 0) //
        {
            controller.GetPawn().Rotate(false);
        }
        else
        {
            controller.GetPawn().Rotate(true);
        }

        //check if the barrel is facing the target
        if (controller.IsBarrelIsFacingTarget(context.target))
        {
            //fire the weapon
            controller.GetPawn().Fire();
            hasFired = true;
        }
    }

    void IAction.Reset()
    {
        return;
    }
}
