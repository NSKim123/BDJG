using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMove : EnemyStateBase
{
    // 플레이어 방향으로 이동
    private Vector3 _moveDirection;

    public EnemyStateMove(StateMachine stateMachine) : base(stateMachine)
    {
        enemyAgent.speed = enemyCharacter.MoveSpeed;
    }

    public override bool canExecute() => stateMachine.currentStateType != State.Die;



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
                    enemyAgent.SetDestination(enemyController.target.transform.position);

                    //Debug.Log(enemyAgent.speed);
                    //MoveToPlayer(enemyController.target);
                }
                break;
            case StepInState.End:
                {
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
