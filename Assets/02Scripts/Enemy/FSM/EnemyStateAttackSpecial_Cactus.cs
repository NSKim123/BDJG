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

    public override bool canExecute() => stateMachine.currentStateType == State.Move;


    public override State MoveNextStep()
    {
        State nextState = State.AttackSpecial;
        SpecialCactus cactus = enemyCharacter as SpecialCactus;

        //Debug.Log(_currentStep);

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
                        enemyCharacter.RequestSpecialAttack(cactus);
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
