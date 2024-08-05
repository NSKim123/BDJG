using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public Dictionary<EState, EnemyStateBase> states;

    public EState currentStateType;
    public EnemyStateBase currentState;

    /// <summary>
    /// 상태를 전환합니다. 현재 상태와 새로운 상태가 동일하다면 바꾸지 않습니다.
    /// </summary>
    /// <param name="newStateType">새로운 상태</param>
    /// <returns>전환 성공이면 true 아니면 false</returns>
    public bool ChangeState(EState newStateType)
    {
        if (currentStateType == newStateType)
        {
            return false;
        }

        // 새로운 상태로 진입가능한지 판단
        if (states[newStateType].CanExecute() == false)
        {
            return false;
        }

        states[currentStateType].Reset();
        currentState = states[newStateType];
        currentStateType = newStateType;
        currentState.MoveNextStep();

        return true;
    }

    /// <summary>
    /// 상태를 전환합니다. 현재 상태와 새로운 상태가 동일해도 전환됩니다.
    /// </summary>
    /// <param name="newStateType">새로운 상태</param>
    /// <returns>전환 성공이면 true 아니면 false</returns>
    public bool ChangeState_AllowSameState(EState newStateType)
    {
        if (states[newStateType].CanExecute() == false)
        {
            return false;
        }

        states[currentStateType].Reset();
        currentState = states[newStateType];
        currentStateType = newStateType;
        currentState.MoveNextStep();

        return true;
    }

    /// <summary>
    /// 상태 정보를 초기화합니다.
    /// </summary>
    /// <param name="states">상태타입과 상태를 저장할 딕셔너리</param>
    public void InitState(Dictionary<EState, EnemyStateBase> states)
    {
        this.states = states;
        currentState = this.states[currentStateType];
    }

    private void Update()
    {
        ChangeState(currentState.MoveNextStep());
    }
}
