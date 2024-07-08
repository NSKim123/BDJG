using UnityEngine;

public class EnemyStateInit : EnemyStateBase
{

    public EnemyStateInit(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override bool canExecute() => false;

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
                    animator.Play("init");
                    _currentStep++;

                }
                break;
            case StepInState.Playing:
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f)
                    {
                        _currentStep++;
                    }

                }
                break;
            case StepInState.End:
                {
                    stateMachine.ChangeState(State.Idle);
                }
                break;
            default:
                break;
        }

        return nextState;

    }
}
