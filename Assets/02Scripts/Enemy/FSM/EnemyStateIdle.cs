using UnityEngine;

public class EnemyStateIdle : EnemyStateBase
{
    public EnemyStateIdle(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override bool CanExecute() => stateMachine.currentStateType == EState.Init;

    public override EState MoveNextStep()
    {
        EState nextState = EState.Idle;

        switch (_currentStep)
        {
            case EStepInState.None:
                {
                    _currentStep++;
                }
                break;
            case EStepInState.Start:
                {
                    _currentStep++;
                    
                }
                break;
            case EStepInState.Playing:
                {
                    animator.Play("idle");
                }
                break;
            case EStepInState.End:
                {
                    
                }
                break;
            default:
                break;
        }

        return nextState;

    }
}
