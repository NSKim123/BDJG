using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyStateHurt : EnemyStateBase
{
    private float dis;
    private Vector3 dir;

    private float _StunedTime = 1.0f;

    private Vector3 knockBackVelocity;
    public EnemyStateHurt(StateMachine stateMachine) : base(stateMachine)
    {
        
    }

    public override bool canExecute() => true;


    public override State MoveNextStep()
    {
        State nextState = State.Hurt;
        //Debug.Log(_currentStep);
        switch (_currentStep)
        {
            case StepInState.None:
            case StepInState.Start:
                {
                    //enemyAgent.isStopped = true;
                    //enemyAgent.velocity = Vector3.zero;
                    enemyAgent.enabled = false;
                    rigid.isKinematic = false;
                    rigid.mass *= 2;
                    //enemyAgent.ResetPath();
                    //Debug.Log("¸ÂÀ½");
                    animator.Play("hurt");

                    dis = enemyCharacter.Damage_Distance;
                    dir = enemyCharacter.Damage_Direction;                    
                    knockBackVelocity = dis * dir;

                    _StunedTime = 1.0f;
                    
                    _currentStep++;
                }
                break;
            case StepInState.Playing:
                {
                    if(knockBackVelocity.sqrMagnitude >= 0.1f)
                    {
                        enemyCharacter.transform.position += knockBackVelocity * Time.deltaTime;
                        knockBackVelocity = Vector3.MoveTowards(knockBackVelocity, Vector3.zero, 100.0f * Time.deltaTime);
                    }
                    else if(_StunedTime > 0.0f)
                    {
                        _StunedTime -= Time.deltaTime;
                    }
                    else
                    {
                        _currentStep++;
                        enemyAgent.enabled = true;
                    }
                }
                break;
            case StepInState.End:
                {
                    nextState = State.Move;
                    if (!enemyAgent.isOnNavMesh)
                    {
                        enemyCharacter.OnDead();
                    }
                    
                    //enemyAgent.SetDestination(enemyController.target.transform.position);
                    rigid.mass /= 2;
                    rigid.isKinematic = true;
                    
                    //enemyAgent.isStopped = false;
                }
                break;
            default:
                break;
        }

        return nextState;

    }
}
