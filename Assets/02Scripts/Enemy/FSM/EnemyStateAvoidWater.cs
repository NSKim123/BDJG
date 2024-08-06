
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateAvoidWater : EnemyStateBase
{ 

    public EnemyStateAvoidWater(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override bool CanExecute() => true;

    public override EState MoveNextStep()
    {
        EState nextState = EState.AvoidWater;

        switch (_currentStep)
        {
            case EStepInState.None:
                {
                    _currentStep++;
                }
                break;
            case EStepInState.Start:
                {
                    //enemyAgent.SetDestination(EnemyManager.Instance.centerPos.position);
                    _currentStep++;
                }
                break;
            case EStepInState.Playing:
                {
                    //Debug.Log("물 피함");
                }
                break;
            case EStepInState.End:
                break;
            default:
                break;
        }

        return nextState;
    }
}
