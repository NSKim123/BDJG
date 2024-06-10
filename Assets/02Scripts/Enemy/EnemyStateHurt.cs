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
                    animator.Play("hurt");
                    Debug.Log("¸ÂÀ½");

                    dis = enemyCharacter.Damage_Distance;
                    dir = enemyCharacter.Damage_Direction;
                    enemyCharacter.transform.position += dis * dir;
                    //enemyCharacter.transform.position = Vector3.Lerp(enemyCharacter.transform.position, dir * dis, 5 * Time.deltaTime);
                    Debug.Log($"{dis}¿Í {dir}");
                    _currentStep++;
                }
                break;
            case StepInState.Playing:
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f)
                    {
                        Debug.Log("hurt");
                        _currentStep++;
                    }
                    
                }
                break;
            case StepInState.End:
                {
                    nextState = State.Move;
                }
                break;
            default:
                break;
        }

        return nextState;

    }
}
