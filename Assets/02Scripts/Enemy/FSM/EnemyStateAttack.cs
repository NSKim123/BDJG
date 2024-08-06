using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyStateAttack : EnemyStateBase
{
    private float animLength;

    System.Diagnostics.Stopwatch watch;

    public EnemyStateAttack(StateMachine stateMachine) : base(stateMachine)
    { 
        watch = new System.Diagnostics.Stopwatch();
    }

    public override bool CanExecute() => stateMachine.currentStateType == EState.Move;


    public void AttackTiming()
    {
        if (enemyController.AttackDetect.Length > 0)
        {
            // 플레이어에게 데미지 주는 함수 호출
            enemyController.AttackDetect[0].GetComponent<IHit>().OnDamaged(20.0f, enemyCharacter.transform.forward);
            //Debug.Log("attack");
        }
        _currentStep++;
    }


    public override EState MoveNextStep()
    {
        EState nextState = EState.Attack;

        //Debug.Log(_currentStep);

        switch (_currentStep)
        {
            case EStepInState.None:
                {
                    watch.Reset();
                    watch.Start();

                    _currentStep++;
                }
                break;
            case EStepInState.Start:
                {

                    animLength = animator.GetCurrentAnimatorStateInfo(0).length;
                    animator.speed = animLength / enemyCharacter.AttackTime;
                    //Debug.Log(animLength);
                    //Debug.Log(animator.speed);
                    enemyAgent.isStopped = true;
                    enemyAgent.velocity = Vector3.zero;
                    animator.Play("attack_general");
                    _currentStep++;

                }
                break;
            case EStepInState.Playing:
                {

                    // 애니메이션에 이벤트 걸기
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
                    watch.Stop();
                    //Debug.Log(watch.ElapsedMilliseconds + "ms");
                }
                break;
            default:
                break;
        }

        return nextState;
    }

    
}
