using UnityEngine;

public class SpecialCactusAIController : CactusAIController
{
    [SerializeField] private bool _isAvailableSpecialAttack = true;
    private float _coolTime;
    [SerializeField] private bool _coolTimeStart = false;

    protected override void OnDisable()
    {
        base.OnDisable();
        _isAvailableSpecialAttack = true;
        _coolTimeStart = false;
        _coolTime = 0;
    }

    protected override void Update()
    {
        base.Update();
 
        // 특수공격 쿨타임 계산
        if (_coolTimeStart)
        {
            _coolTime += Time.deltaTime;

            if (_coolTime > enemyCharacter.SpecialAttackCoolTime)
            {
                _isAvailableSpecialAttack = true;
                _coolTimeStart = false;
            }
        }


        if (isDetected)
        {
            // 특수공격이 가능하다면
            if (_isAvailableSpecialAttack)
            {
                AttackDetect = Physics.OverlapSphere(transform.position + Vector3.up * 1.8f, enemyCharacter.SpecialAttackRange, targetLayer);

                if (AttackDetect.Length > 0 && !attacked)
                {
                    attacked = true;
                    _isAvailableSpecialAttack = false;
                    _coolTimeStart = true;
                    _coolTime = 0;
                    stateMachine.ChangeState(EState.AttackSpecial);
        
                }
            }
            else
            {
                AttackDetect = Physics.OverlapSphere(transform.position + Vector3.up * 1.8f, enemyCharacter.AttackRange, targetLayer);

                if (AttackDetect.Length > 0 && !attacked)
                {
                    attacked = true;
                    stateMachine.ChangeState(EState.Attack);
                }
            }

        }


        if ((stateMachine.currentStateType != EState.Attack || stateMachine.currentStateType != EState.AttackSpecial)
            && attacked)
        {
            attacked = false;
        }
    }


#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (enemyCharacter != null)
        {
            Gizmos.color = Color.green;
            if (_isAvailableSpecialAttack)
            {
                Gizmos.DrawSphere(transform.position + Vector3.up * 1.8f, enemyCharacter.SpecialAttackRange);

            }
            else
            {
                Gizmos.DrawSphere(transform.position + Vector3.up * 1.8f, enemyCharacter.AttackRange);

            }
        }
    }
#endif
}
