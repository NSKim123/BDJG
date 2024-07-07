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

