using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyStateAttackSpecial_Mush : EnemyStateBase
{

    public EnemyStateAttackSpecial_Mush(StateMachine stateMachine) : base(stateMachine)
    { 

    }

    public override bool canExecute() => stateMachine.currentStateType == State.Move;


    public override State MoveNextStep()
    {
        State nextState = State.AttackSpecial;

        SpecialMushroom mushroom = enemyCharacter as SpecialMushroom;


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
                    if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                    {
                        enemyCharacter.RequestCloudAttack(mushroom);
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
