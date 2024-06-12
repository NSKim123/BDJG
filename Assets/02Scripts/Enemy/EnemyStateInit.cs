
using System;
using UnityEngine;

public class EnemyStateInit : EnemyStateBase
{
    private Vector3 destination;

    public EnemyStateInit(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override bool canExecute() => true;

    public override State MoveNextStep()
    {
        throw new NotImplementedException();
    }

    /*
    public override State MoveNextStep()
    {
        State nextState = State.Init;

        switch (_currentStep)
        {
            case StepInState.None:
                {
                    _currentStep++;
                }
                break;
            case StepInState.Start:
                {
                    destination = enemyCharacter.transform.position + Vector3.up * 3;
                    enemyCollider.isTrigger = true;
                    rigid.isKinematic = true;
                    _currentStep++;
                }
                break;
            case StepInState.Playing:
                {
                    enemyCharacter.transform.position += Vector3.up * Time.deltaTime * 0.5f;
                    if (enemyCharacter.transform.position.y >= destination.y)
                    {
                        _currentStep++;
                    }
                }
                break;
            case StepInState.End:
                {
                    enemyCollider.isTrigger = false;
                    rigid.isKinematic = false;
                    nextState = State.Move;
                }
                break;
            default:
                break;
        }

        return nextState;
    } */
}
