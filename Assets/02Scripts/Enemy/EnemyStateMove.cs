using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMove : EnemyStateBase
{

    // 플레이어 방향으로 이동
    private Vector3 _moveDirection;
    private Vector3 _lookDirection;

    public EnemyStateMove(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override bool canExecute() => stateMachine.currentStateType == State.Attack
                                    || stateMachine.currentStateType == State.Hurt;



    public override State MoveNextStep()
    {
        State nextState = State.Move;

        switch (_currentStep)
        {
            case StepInState.None:
                {
                    _currentStep++;
                }
                break;
            case StepInState.Start:
                {
                    animator.Play("move");
                    _currentStep++;
                }
                break;
            case StepInState.Playing:
                {
                    MoveToPlayer(enemyController.target);
                    //Debug.Log("moving");
                }
                break;
            case StepInState.End:
                {
                    _currentStep++;
                }
                break;
            default:
                break;
        }

        return nextState;
    }

    public void MoveToPlayer(GameObject target)
    {
        _moveDirection = target.transform.position - enemyCharacter.transform.position;
        _moveDirection.y = 0;
        enemyCharacter.transform.rotation = Quaternion.LookRotation(_moveDirection);
        rigid.velocity = _moveDirection.normalized * enemyCharacter.MoveSpeed;
    }
}
