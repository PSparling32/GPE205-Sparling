using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    bool CanEnter(AIController controller, AIContext context);
    void Enter(AIController controller, AIContext context);
    bool CanExit(AIController controller, AIContext context);
    void Execute(AIController controller, AIContext context);
    IState Exit(AIController controller, AIContext context);

    void OnTargetVisible(AIController controller, AIContext context);

    void OnSound(AIController controller, AIContext context);
}
