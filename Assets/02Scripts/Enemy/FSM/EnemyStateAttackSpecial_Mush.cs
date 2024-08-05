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

    public override bool CanExecute() => stateMachine.currentStateType == EState.Move;


    public override EState MoveNextStep()
    {
        EState nextState = EState.AttackSpecial;

        SpecialMushroom mushroom = enemyCharacter as SpecialMushroom;


        switch (_currentStep)
        {
            case EStepInState.None:
                {
                    _currentStep++;
                }
                break;
            case EStepInState.Start:
                {
                    animator.Play("attack_special");
                    
                    _currentStep++;
                }
                break;
            case EStepInState.Playing:
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                    {
                        enemyCharacter.RequestCloudAttack(mushroom);
                        _currentStep++;
                    }

                }
                break;
            case EStepInState.End:
                {
                    nextState = EState.Move;

                }
                break;
            default:
                break;
        }

        return nextState;
    }

    
}
