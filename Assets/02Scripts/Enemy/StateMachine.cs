using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public Dictionary<State, EnemyStateBase> states;

    public State currentStateType;
    public EnemyStateBase currentState;

    public bool ChangeState(State newStateType)
    {
        if (currentStateType == newStateType)
        {
            return false;
        }

        if (states[newStateType].canExecute() == false)
        {
            return false;
        }

        states[currentStateType].Reset();
        currentState = states[newStateType];
        currentStateType = newStateType;
        currentState.MoveNextStep();

        return true;
    }

    public bool ChangeState_AllowSameState(State newStateType)
    {
        if (states[newStateType].canExecute() == false)
        {
            return false;
        }

        states[currentStateType].Reset();
        currentState = states[newStateType];
        currentStateType = newStateType;
        currentState.MoveNextStep();

        return true;
    }

    public void StateInit(Dictionary<State, EnemyStateBase> states)
    {
        this.states = states;
        currentState = this.states[currentStateType];
    }

    private void Update()
    {
        ChangeState(currentState.MoveNextStep());   
    }
}
