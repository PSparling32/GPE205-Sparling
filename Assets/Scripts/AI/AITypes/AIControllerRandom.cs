using System.Collections;
using System.Collections.Generic;
using DefensiveStates;
using UnityEngine;

public class AIControllerRandom : AIController
{
    private void Start()
    {
        name = "AIControllerDefensive";
        SetUp();
        context.self = GetPawn();

        stateMachine = new StateMachine(new StateRandomChase());
    }

    public void Update()
    {
        AIUpdate();
    }
}
