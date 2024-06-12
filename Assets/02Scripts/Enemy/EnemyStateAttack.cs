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

        //Debug.Log(_currentStep);

        switch (_currentStep)
        {
            case StepInState.None:
                {
                    watch.Reset();
                    watch.Start();

                    _currentStep++;
                }
                break;
            case StepInState.Start:
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
            case StepInState.Playing:
                {

                    // 애니메이션에 이벤트 걸기
                    if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f)
                    {
                        if (enemyController.attackDetect.Length > 0)
                        {
                            // 플레이어에게 데미지 주는 함수 호출
                            enemyController.attackDetect[0].GetComponent<IHit>().OnDamaged(20.0f, enemyCharacter.transform.forward);
                            //Debug.Log("attack");

                        }

                        _currentStep++;

                    }

                }
                break;
            case StepInState.End:
                {
                    enemyAgent.isStopped = false;
                    nextState = State.Move;
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
