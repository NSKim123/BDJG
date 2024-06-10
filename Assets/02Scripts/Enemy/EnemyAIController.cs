using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;


public class EnemyAIController : MonoBehaviour
{

    public enum AIStep
    {
        None,
        StartFollow,
        Follow,
        StartAttack,
        Attack,
    }

    private StateMachine _stateMachine;
    private Enemy _enemyCharacter;

    // �÷��̾� ������Ʈ
    public GameObject target;

    [SerializeField] private LayerMask _targetLayer;
    [SerializeField] private bool _attacked = false;

    public Collider[] attackDetect;


    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        _stateMachine = GetComponent<StateMachine>();
        _enemyCharacter = GetComponent<Enemy>();
    }

    IEnumerator C_WaitAttackTime()
    {
        yield return new WaitForSeconds(_enemyCharacter.AttackTime);

        _attacked = false;

    }

    private void Update()
    {
        // player�� �����ؼ� ���� ���¸� move �Ǵ� attack ���� �ٲߴϴ�.

        attackDetect = Physics.OverlapSphere(transform.position, _enemyCharacter.DetectPlayerDistance, _targetLayer);

        // Ÿ��(�÷��̾�) ����
        if (attackDetect.Length > 0)
        {
            if (!_attacked)
            {
                _attacked = true;
                _stateMachine.ChangeState(State.Attack);
                StartCoroutine(C_WaitAttackTime());
            }

        }
        else
        {
            _stateMachine.ChangeState(State.Move);
        }

            
        //if (stateMachine.currentStateType == State.move && _attacked)
        //{
        //    if (currentTime >= attackTime)
        //    {
        //        _attacked = false;
        //        currentTime = 0;
        //    }
            
        //}

    }

    // (�ӽ�) ���� �ѷ��� �ݶ��̴��� ���������� �׾��ٰ� �Ǵ��մϴ�.
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Boundary")
        {
            _stateMachine.ChangeState(State.Die);
        }
    }


    private void OnDrawGizmosSelected()
    {
        if (_enemyCharacter != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(transform.position, _enemyCharacter.DetectPlayerDistance);
        }

    }
}
