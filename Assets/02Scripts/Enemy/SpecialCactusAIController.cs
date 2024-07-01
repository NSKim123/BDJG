﻿using UnityEngine;

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



    void Update()
    {
        dis = Vector3.Distance(transform.position, target.transform.position);

        if (dis <= maxdis && !isDetected)
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


        // 특수공격이 가능하다면
        if (isDetected)
        {
            if (isAvailableSpecialAttack)
            {

                _attackDetect = Physics.OverlapSphere(transform.position, _enemyCharacter.SpecialAttackRange, _targetLayer);

                if (_attackDetect.Length > 0 && !_attacked)
                {
                    Debug.Log("특수 공격 선인장");
                    _attacked = true;
                    isAvailableSpecialAttack = false;
                    coolTimeStart = true;
                    coolTime = 0;
                    _stateMachine.ChangeState(State.AttackSpecial);
                    

                }

                
            }
            else
            {
                _attackDetect = Physics.OverlapSphere(transform.position, _enemyCharacter.AttackRange, _targetLayer);

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

    public override void ChangeDestination()
    {
        if (_stateMachine.currentStateType != State.AvoidWater)
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
            if (isAvailableSpecialAttack)
            {
                Gizmos.DrawSphere(transform.position, _enemyCharacter.SpecialAttackRange);

            }
            else
            {
                Gizmos.DrawSphere(transform.position, _enemyCharacter.AttackRange);

            }
        }
    }
#endif
}
