
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateAvoidWater : EnemyStateBase
{ 

    public EnemyStateAvoidWater(StateMachine stateMachine) : base(stateMachine)
    {


    }

    public override bool canExecute() => true;

    public override State MoveNextStep()
    {
        State nextState = State.AvoidWater;

        switch (_currentStep)
        {
            case StepInState.None:
                {
                    _currentStep++;
                }
                break;
            case StepInState.Start:
                {
                    enemyAgent.SetDestination(EnemyManager.Instance.centerPos.position);
                    _currentStep++;
                }
                break;
            case StepInState.Playing:
                {
                    Debug.Log("물 피함");
                }
                break;
            case StepInState.End:
                break;
            default:
                break;
        }

        return nextState;
    }
}
