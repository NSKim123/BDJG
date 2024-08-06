using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyStateAttackSpecial_Cactus : EnemyStateBase
{
    public EnemyStateAttackSpecial_Cactus(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override bool CanExecute() => stateMachine.currentStateType == EState.Move;


    public override EState MoveNextStep()
    {
        EState nextState = EState.AttackSpecial;
        SpecialCactus cactus = enemyCharacter as SpecialCactus;

        //Debug.Log(_currentStep);

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
                        enemyCharacter.RequestSpecialAttack(cactus);
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
