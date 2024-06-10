using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyStateAttack : EnemyStateBase
{
    public EnemyStateAttack(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override bool canExecute() => stateMachine.currentStateType == State.Move;


  /*
    public void AttackPlayer(GameObject target)
    {
        target.transform.position += Vector3.back * 3;
        //_rigid.AddForce((target.transform.position - transform.position).normalized * _attackForce, ForceMode.Impulse);
    } */

    public override State MoveNextStep()
    {
        State nextState = State.Attack;

        switch (_currentStep)
        {
            case StepInState.None:
                {
                    _currentStep++;
                }
                break;
            case StepInState.Start:
                {
                    // 애니메이션 클립의 길이를 가져옵니다.
                    //float animationClipLength = animator.GetCurrentAnimatorStateInfo(0).length;

                    // 애니메이션 속도를 조절하여 재생 시간이 1초가 되도록 설정합니다.
                    //animator.speed = animationClipLength / enemyController.attackTime;
                    animator.Play("attack_general");
                    _currentStep++;
                    
                }
                break;
            case StepInState.Playing:
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f)
                    {
                        if (enemyController.attackDetect.Length > 0)
                        {
                            // 플레이어에게 데미지 주는 함수 호출
                            enemyController.attackDetect[0].GetComponent<IHit>().OnDamaged(20.0f, enemyCharacter.transform.forward);
                            Debug.Log("attack");

                        }
                        
                        _currentStep++;

                    }

                }
                break;
            case StepInState.End:
                {
                    nextState = State.Move;
                    //animator.speed = 1;

                }
                break;
            default:
                break;
        }

        return nextState;
    }

    
}
