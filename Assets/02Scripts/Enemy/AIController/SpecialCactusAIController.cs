using UnityEngine;

public class SpecialCactusAIController : EnemyAIController
{
    float dis;
    float maxdis = 7.0f;

    bool isDetected = false;

    [SerializeField] private bool isAvailableSpecialAttack = true;
    private float coolTime;
    [SerializeField] private bool coolTimeStart = false; 

    public override Collider[] AttackDetect { get { return _attackDetect; } }
    private Collider[] _attackDetect;

    private void OnDisable()
    {
        isDetected = false;
    }


    void Update()
    {
        dis = Vector3.Distance(transform.position, target.transform.position);

        if (_stateMachine.currentStateType == State.Idle && target.TryGetComponent(out Scarecrow scarecrow))
        {
            _stateMachine.ChangeState(State.Move);
            isDetected = true;
        }

        if (dis <= maxdis && !isDetected && _stateMachine.currentStateType == State.Idle)
        {
            isDetected = true;
            _stateMachine.ChangeState(State.Move);
        }

        // 특수공격 쿨타임 계산
        if (coolTimeStart)
        {
            coolTime += Time.deltaTime;

            if (coolTime > _enemyCharacter.SpecialAttackCoolTime)
            {
                isAvailableSpecialAttack = true;
                coolTimeStart = false;
            }
        }


        if (isDetected)
        {
            // 특수공격이 가능하다면
            if (isAvailableSpecialAttack)
            {
                _attackDetect = Physics.OverlapSphere(transform.position + Vector3.up * 1.8f, _enemyCharacter.SpecialAttackRange, _targetLayer);

                if (_attackDetect.Length > 0 && !_attacked)
                {
                    _attacked = true;
                    isAvailableSpecialAttack = false;
                    coolTimeStart = true;
                    coolTime = 0;
                    _stateMachine.ChangeState(State.AttackSpecial);
        
                }

                
            }
            else
            {
                _attackDetect = Physics.OverlapSphere(transform.position + Vector3.up * 1.8f, _enemyCharacter.AttackRange, _targetLayer);

                if (_attackDetect.Length > 0 && !_attacked)
                {
                    _attacked = true;
                    _stateMachine.ChangeState(State.Attack);
                }
            }

        }


        if ((_stateMachine.currentStateType != State.Attack || _stateMachine.currentStateType != State.AttackSpecial)
            && _attacked)
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
            if (isAvailableSpecialAttack)
            {
                Gizmos.DrawSphere(transform.position + Vector3.up * 1.8f, _enemyCharacter.SpecialAttackRange);

            }
            else
            {
                Gizmos.DrawSphere(transform.position + Vector3.up * 1.8f, _enemyCharacter.AttackRange);

            }
        }
    }
#endif
}
