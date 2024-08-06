using UnityEngine;

public class EnemyStateInit : EnemyStateBase
{

    public EnemyStateInit(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override bool CanExecute() => false;

    public override EState MoveNextStep()
    {
        EState nextState = EState.Init;

        switch (_currentStep)
        {
            case EStepInState.None:
                {
                    if (!enemyAgent.enabled)
                    {
                        enemyAgent.enabled = true;

                    }
                    if (!rigid.useGravity)
                    {
                        rigid.useGravity = true;
                    }

                    if (!rigid.isKinematic)
                    {
                        rigid.isKinematic = true;
                    }

                    _currentStep++;


                }
                break;
            case EStepInState.Start:
                {
                    animator.Play("init");
                    _currentStep++;

                }
                break;
            case EStepInState.Playing:
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f)
                    {
                        _currentStep++;
                    }

                }
                break;
            case EStepInState.End:
                {
                    stateMachine.ChangeState(EState.Idle);
                }
                break;
            default:
                break;
        }

        return nextState;

    }
}
