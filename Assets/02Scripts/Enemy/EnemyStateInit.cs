using UnityEngine;

public class EnemyStateInit : EnemyStateBase
{
    float desiredPosOfY = 7.3f;
    float nextTime = 1.0f;

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
                    if (enemyAgent != null)
                    {
                        enemyAgent.enabled = false;
                        col.isTrigger = true;
                        animator.Play("init");
                        _currentStep++;
                    }

                }
                break;
            case StepInState.Playing:
                {


                    if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                    {
                        _currentStep++;
                    }

                    //if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.6f)
                    //{
                    //    rigid.AddForce(Vector3.up * 0.5f, ForceMode.Impulse);

                    //}
                    //nextTime -= Time.deltaTime;


                    //else
                    //{
                    //    _currentStep++;
                    //}
                }
                break;
            case StepInState.End:
                {
                    enemyAgent.enabled = true;
                    col.isTrigger = false;
                    stateMachine.ChangeState(State.Idle);
                }
                break;
            default:
                break;
        }

        return nextState;

    }
}
