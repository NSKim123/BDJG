using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateHurt : EnemyStateBase
{
    private float dis;
    private Vector3 dir;
    public EnemyStateHurt(StateMachine stateMachine) : base(stateMachine)
    {
        
    }

    public override bool canExecute() => true;


    public override State MoveNextStep()
    {
        State nextState = State.Hurt;

        switch (_currentStep)
        {
            case StepInState.None:
            case StepInState.Start:
                {
                    //enemyAgent.ResetPath();
                    enemyAgent.isStopped = true;
                    enemyAgent.velocity = Vector3.zero;
                    //Debug.Log("¸ÂÀ½");
                    animator.Play("hurt");

                    dis = enemyCharacter.Damage_Distance;
                    dir = enemyCharacter.Damage_Direction;
                    enemyCharacter.transform.position += dis * dir;
                    
                    _currentStep++;
                }
                break;
            case StepInState.Playing:
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f)
                    {
                        //Debug.Log("hurt");
                        _currentStep++;
                    }
                    
                }
                break;
            case StepInState.End:
                {
                    nextState = State.Move;
                    //enemyAgent.SetDestination(enemyController.target.transform.position);
                    enemyAgent.isStopped = false;
                }
                break;
            default:
                break;
        }

        return nextState;

    }
}
