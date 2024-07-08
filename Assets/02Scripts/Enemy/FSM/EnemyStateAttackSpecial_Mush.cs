using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyStateAttackSpecial_Mush : EnemyStateBase
{
    private float animLength;

    public EnemyStateAttackSpecial_Mush(StateMachine stateMachine) : base(stateMachine)
    { 

    }

    public override bool canExecute() => stateMachine.currentStateType == State.Move;


    public override State MoveNextStep()
    {
        State nextState = State.AttackSpecial;

        SpecialMushroom mush = enemyCharacter as SpecialMushroom;


        switch (_currentStep)
        {
            case StepInState.None:
                {
                    _currentStep++;
                }
                break;
            case StepInState.Start:
                {
                    animator.Play("attack_special");
                    
                    _currentStep++;
                }
                break;
            case StepInState.Playing:
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.7f)
                    {
                        Vector3 pos = mush.transform.position + new Vector3(0, 1, 1);
                        GameObject cloud = GameObject.Instantiate(mush.cloud, pos, Quaternion.identity);

                        cloud.transform.position += mush.transform.up * 0.5f;
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
