using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyStateAttack : EnemyStateBase
{
    private float _animLength;

    public EnemyStateAttack(StateMachine stateMachine) : base(stateMachine)
    { 
    }

    public override bool CanExecute() => stateMachine.currentStateType == EState.Move;


    public override EState MoveNextStep()
    {
        EState nextState = EState.Attack;

        switch (_currentStep)
        {
            case EStepInState.None:
                {
                    _currentStep++;
                }
                break;
            case EStepInState.Start:
                {
                    _animLength = animator.GetCurrentAnimatorStateInfo(0).length;
                    animator.speed = _animLength / enemyCharacter.AttackTime;

                    enemyAgent.isStopped = true;
                    enemyAgent.velocity = Vector3.zero;
                    animator.Play("attack_general");
                    _currentStep++;
                }
                break;
            case EStepInState.Playing:
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f)
                    {
                        if (enemyController.AttackDetect.Length > 0)
                        {
                            
                            // 플레이어에게 데미지 주는 함수 호출
                            if (enemyController.AttackDetect[0].TryGetComponent(out IHit hit))
                            {
                                hit.OnDamaged(20.0f, (enemyCharacter.transform.forward + Vector3.up).normalized);
                            }
                        }

                        _currentStep++;
                    }
                }
                break;
            case EStepInState.End:
                {
                    enemyAgent.isStopped = false;
                    nextState = EState.Move;
                    animator.speed = 1;
                }
                break;
            default:
                break;
        }

        return nextState;
    }

    
}
