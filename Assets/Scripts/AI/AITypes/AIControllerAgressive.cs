using System.Collections;
using System.Collections.Generic;
using AggressiveStates;
using UnityEngine;

public class AIControllerAgressive : AIController
{
    // Start is called before the first frame update
    void Start()
    {
        name = "AIControllerAgressive";
        SetUp();
        context.self = GetPawn();

        stateMachine = new StateMachine(new StateAggressiveDefault());
    }

    // Update is called once per frame
    void Update()
    {
        AIUpdate();
    }
}
