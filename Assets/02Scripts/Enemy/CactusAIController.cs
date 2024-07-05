using UnityEngine;

public class CactusAIController : EnemyAIController
{
    float dis;
    float maxdis = 7.0f;

    bool isDetected = false;

    public override Collider[] AttackDetect { get { return _attackDetect; } }
    private Collider[] _attackDetect;



    void Update()
    {
        dis = Vector3.Distance(transform.position, target.transform.position);

        // 수정하기
        _attackDetect = Physics.OverlapSphere(transform.position + Vector3.up * 1.8f, _enemyCharacter.AttackRange, _targetLayer);

        //AttackDetected = Physics.OverlapSphereNonAlloc(transform.position, _enemyCharacter.AttackRange, AttackDetect);

        if (dis <= maxdis && !isDetected)
        {
            isDetected = true;
            _stateMachine.ChangeState(State.Move);
        }


        if (_attackDetect.Length > 0 && !_attacked && isDetected)
        {
            _attacked = true;
            _stateMachine.ChangeState(State.Attack);
        }

        if ((_stateMachine.currentStateType != State.Attack || _stateMachine.currentStateType != State.AttackSpecial) && _attacked)
        {
            _attacked = false;
        }

    }

    public override void ChangeDestination()
    {
        if (_stateMachine.currentStateType == State.Move)
        {
            _stateMachine.ChangeState(State.AvoidWater);
        }
        else if (_stateMachine.currentStateType == State.AvoidWater)
        {
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
