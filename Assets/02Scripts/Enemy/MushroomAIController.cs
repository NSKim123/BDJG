using UnityEngine;

public class MushroomAIController : EnemyAIController
{
    public override Collider[] AttackDetect { get { return _attackDetect; } }

    private Collider[] _attackDetect;


    protected void Update()
    {
        if (_stateMachine.currentStateType == State.Idle)
        {
            _stateMachine.ChangeState(State.Move);
        }

        _attackDetect = Physics.OverlapSphere(transform.position + Vector3.up * 1.8f, _enemyCharacter.AttackRange, _targetLayer);

        // 타켓(플레이어) 감지
        if (_attackDetect.Length > 0 && !_attacked)
        {
            //Debug.Log("타겟 감지");
            _attacked = true;
            _stateMachine.ChangeState(State.Attack);

        }

        if (_stateMachine.currentStateType == State.Move && _attacked)
        {
            _attacked = false;
        }

    }
    public override void ChangeDestination()
    {
        if (_stateMachine.currentStateType == State.Move)
        {
            //Debug.Log("바꿈");
            _stateMachine.ChangeState(State.AvoidWater);
        }
        else if (_stateMachine.currentStateType == State.AvoidWater)
        {
            //Debug.Log("다시바꿈");
            _stateMachine.ChangeState(State.Move);
        }

    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (_enemyCharacter != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(transform.position + Vector3.up * 1.8f, _enemyCharacter.AttackRange);
        }
    }
#endif


}

