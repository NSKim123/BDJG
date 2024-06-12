using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public List<EnemyStateBase> states;

    public State currentStateType;
    public EnemyStateBase currentState;

    public bool ChangeState(State newStateType)
    {
        if (currentStateType == newStateType)
        {
            return false;
        }

        if (states[(int)newStateType].canExecute() == false)
        {
            return false;
        }

        states[(int)currentStateType].Reset();
        currentState = states[(int)newStateType];
        currentStateType = newStateType;
        currentState.MoveNextStep();

        return true;
    }

    public void StateInit(List<EnemyStateBase> states)
    {
        this.states = states;
        currentState = states[(int)currentStateType];
    }

    private void Update()
    {
        ChangeState(currentState.MoveNextStep());
        
    }
}
