using UnityEngine;

public class SpecialMushroomAIController : EnemyAIController
{
    // 특수공격 가능한지, 가능하면 우선
    // 쿨타임 걸리면 일반공격

    [SerializeField] private bool isAvailableSpecialAttack = true;
    [SerializeField] private bool coolTimeStart = false;
    private float coolTime = 0;

    public override Collider[] AttackDetect { get { return _attackDetect; } }

    private Collider[] _attackDetect;


    protected void Update()
    {
        // idle 건너뜀
        if (_stateMachine.currentStateType == State.Idle)
        {
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

