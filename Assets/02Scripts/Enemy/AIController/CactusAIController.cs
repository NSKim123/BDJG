using UnityEngine;

public class CactusAIController : EnemyAIController
{
    float dis;
    float maxdis = 7.0f;

    bool isDetected = false;

    public override Collider[] AttackDetect { get { return _attackDetect; } }
    private Collider[] _attackDetect;

    private void OnDisable()
    {
        isDetected = false;
    }

    void Update()
    {
        if (Time.timeScale == 0.0f)
        {
            return;
        }

        dis = Vector3.Distance(transform.position, Target.transform.position);

        _attackDetect = Physics.OverlapSphere(transform.position + Vector3.up * 1.8f, _enemyCharacter.AttackRange, _targetLayer);

        // idle 상태이며 허수아비 아이템 사용 중일 때
        if (_stateMachine.currentStateType == EState.Idle && Target.TryGetComponent(out Scarecrow _))
        {
            _stateMachine.ChangeState(EState.Move);
            isDetected = true;
        }

        
        if (dis <= maxdis && !isDetected && _stateMachine.currentStateType == EState.Idle)
        {
            isDetected = true;
            _stateMachine.ChangeState(EState.Move);
        }


        if (_attackDetect.Length > 0 && !_attacked && isDetected)
        {
            _attacked = true;
            _stateMachine.ChangeState(EState.Attack);
        }

        if ((_stateMachine.currentStateType != EState.Attack || _stateMachine.currentStateType != EState.AttackSpecial) && _attacked)
        {
            _attacked = false;
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
