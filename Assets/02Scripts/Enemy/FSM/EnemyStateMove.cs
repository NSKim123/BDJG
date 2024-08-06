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

    public override bool CanExecute() => stateMachine.currentStateType != EState.Die;


    public override EState MoveNextStep()
    {
        EState nextState = EState.Move;
        //Debug.Log(enemyCharacter.GetType());

        switch (_currentStep)
        {
            case EStepInState.None:
                {
                    _currentStep++;
                }
                break;
            case EStepInState.Start:
                {
                    animator.Play("move");
                    _currentStep++;
                }
                break;
            case EStepInState.Playing:
                {
                    enemyAgent.SetDestination(enemyController.Target.transform.position);
                }
                break;
            case EStepInState.End:
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
