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
                    // �ִϸ��̼� Ŭ���� ���̸� �����ɴϴ�.
                    //float animationClipLength = animator.GetCurrentAnimatorStateInfo(0).length;

                    // �ִϸ��̼� �ӵ��� �����Ͽ� ��� �ð��� 1�ʰ� �ǵ��� �����մϴ�.
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
                            // �÷��̾�� ������ �ִ� �Լ� ȣ��
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
