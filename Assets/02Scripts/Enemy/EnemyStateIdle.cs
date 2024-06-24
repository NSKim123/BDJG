using UnityEngine;

public class EnemyStateIdle : EnemyStateBase
{
    public EnemyStateIdle(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override bool canExecute() => stateMachine.currentStateType != State.Die;

    public override State MoveNextStep()
    {
        State nextState = State.Idle;

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
                    //Debug.Log("idle");
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
