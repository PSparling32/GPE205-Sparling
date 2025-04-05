using System.Collections;
using System.Collections.Generic;
using CowardStates;
using DefensiveStates;
using UnityEngine;

public class AIControllerDefensive : AIController
{
    private void Start()
    {
        name = "AIControllerDefensive";
        SetUp();
        context.self = GetPawn();

        stateMachine = new StateMachine(new StateDefensiveDefault());
    }

    public void Update()
    {
        AIUpdate();
    }
}
