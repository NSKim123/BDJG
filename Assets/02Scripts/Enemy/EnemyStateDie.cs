using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateDie : EnemyStateBase
{
    public EnemyStateDie(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override bool canExecute() => true;

    public override State MoveNextStep()
    {
        State nextState = State.Die;

        switch (_currentStep)
        {
            case StepInState.None:
                {
                    _currentStep++;
                }
                break;
            case StepInState.Start:
                {
                    _currentStep++;
                }
                break;
            case StepInState.Playing:
                {
                    Debug.Log("die");
                    //enemyCharacter.myMaterial.color = 
                    GameObject.Destroy(enemyCharacter.gameObject);
                }
                break;
            case StepInState.End:
                {

                }
                break;
            default:
                break;
        }

        return nextState;
    }

}
