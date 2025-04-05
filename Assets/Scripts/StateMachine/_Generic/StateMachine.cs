using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class StateMachine
{
    protected IState currentState;

    protected IState previousState;

    //default state must always be set and be valid from any state
    public IState defaultState;

    /// Constructor
    public StateMachine(IState defaultState)
    {
        this.defaultState = defaultState;
        currentState = defaultState;
    }

    public void Update(AIController controller, AIContext context)
    {
        currentState?.Execute(controller, context);
        if (currentState.CanExit(controller, context))
        {
            ChangeState(controller, context);
        }
    }

    public void ChangeState(AIController controller, AIContext context)
    {
        if (currentState == null) //go to default state
        {
            currentState = defaultState;
            currentState.Enter(controller, context);
            return;
        }

        //lets make sure the next state is valid
        IState nextState = currentState.Exit(controller, context);

        if (!nextState.CanEnter(controller, context)) //next state isnt valid, go to default
        {
            previousState = currentState;
            currentState = defaultState;
            currentState.Enter(controller, context);
            return;
        }
        else
        {
            previousState = currentState;
            currentState = nextState;
            currentState.Enter(controller, context);
        }
    }
}
