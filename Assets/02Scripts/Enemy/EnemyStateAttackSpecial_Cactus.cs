using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyStateAttackSpecial_Cactus : EnemyStateBase
{
    private float animLength;

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
                    Debug.Log("공격 상태 들어옴");
                    animLength = animator.GetCurrentAnimatorStateInfo(0).length;
                    animator.speed = animLength / enemyCharacter.SpecialAttackTime;
                    //animator.Play("attack_special");

                    Transform thornTrans = cactus.transform.GetChild(1).transform;
                    GameObject.Instantiate(cactus.thornArea, thornTrans.position, Quaternion.identity);

                    _currentStep++;



                }
                break;
            case StepInState.Playing:
                {
                   
                    if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                    {
                        _currentStep++;

                    }
                    // temp
                    _currentStep++;



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
