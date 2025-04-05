using System;
using CowardStates;
using UnityEngine;

public class AIControllerCoward : AIController
{
    private void Start()
    {
        name = "AIControllerCoward";
        SetUp();
        context.self = GetPawn();

        stateMachine = new StateMachine(new StateCowardDefault());
    }

    public void Update()
    {
        AIUpdate();
    }
}
