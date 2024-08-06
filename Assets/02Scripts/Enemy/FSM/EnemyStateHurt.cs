using UnityEngine;

public class EnemyStateHurt : EnemyStateBase
{
    private float dis;
    private Vector3 dir;

    private float _StunedTime = 1.0f;

    private Vector3 knockBackVelocity;
    public EnemyStateHurt(StateMachine stateMachine) : base(stateMachine)
    {
        
    }

    public override bool CanExecute() => stateMachine.currentStateType != EState.Die;


    public override EState MoveNextStep()
    {
        EState nextState = EState.Hurt;
        //Debug.Log(_currentStep);
        switch (_currentStep)
        {
            case EStepInState.None:
            case EStepInState.Start:
                {
                    //enemyAgent.isStopped = true;
                    //enemyAgent.velocity = Vector3.zero;
                    enemyAgent.enabled = false;
                    rigid.isKinematic = false;
                    rigid.mass *= 2;
                    //enemyAgent.ResetPath();
                    //Debug.Log("¸ÂÀ½");
                    animator.Play("hurt");

                    dis = enemyCharacter.Damage_Distance;
                    dir = enemyCharacter.Damage_Direction;                    
                    knockBackVelocity = dis * dir;

                    _StunedTime = 1.0f;
                    
                    _currentStep++;
                }
                break;
            case EStepInState.Playing:
                {
                    if(knockBackVelocity.sqrMagnitude >= 0.1f)
                    {
                        enemyCharacter.transform.position += knockBackVelocity * Time.deltaTime;
                        knockBackVelocity = Vector3.MoveTowards(knockBackVelocity, Vector3.zero, 100.0f * Time.deltaTime);
                    }
                    else if(_StunedTime > 0.0f)
                    {
                        _StunedTime -= Time.deltaTime;
                    }
                    else
                    {
                        _currentStep++;
                        enemyAgent.enabled = true;
                    }
                }
                break;
            case EStepInState.End:
                {
                    nextState = EState.Move;
                    if (!enemyAgent.isOnNavMesh)
                    {
                        enemyCharacter.OnDead();
                    }
                    
                    //enemyAgent.SetDestination(enemyController.target.transform.position);
                    rigid.mass /= 2;
                    rigid.isKinematic = true;
                    
                    //enemyAgent.isStopped = false;
                }
                break;
            default:
                break;
        }

        return nextState;

    }
}
