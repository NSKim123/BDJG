using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyAIController : MonoBehaviour
{
    private StateMachine _stateMachine;
    private Enemy _enemyCharacter;

    // �÷��̾� ������Ʈ
    public GameObject target;

    [SerializeField] private LayerMask _targetLayer;
    [SerializeField] private bool _attacked = false;

    public Collider[] attackDetect;

    private MapManager _mapManager;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        _stateMachine = GetComponent<StateMachine>();
        _enemyCharacter = GetComponent<Enemy>();
        _mapManager = FindAnyObjectByType<MapManager>();
        _mapManager.OnChangeDestination += ChangeDestination;
    }


    private void Update()
    {
        // player�� �����ؼ� ���� ���¸� move �Ǵ� attack ���� �ٲߴϴ�.

        attackDetect = Physics.OverlapSphere(transform.position, _enemyCharacter.DetectPlayerDistance, _targetLayer);

        // Ÿ��(�÷��̾�) ����
        if (attackDetect.Length > 0 && !_attacked)
        {
            //Debug.Log("Ÿ�� ����");
            _attacked = true;
            _stateMachine.ChangeState(State.Attack);

        }
        //else
        //{
        //    Debug.Log("�ٽ� ����");
        //    _stateMachine.ChangeState(State.Move);
        //}

        if (_stateMachine.currentStateType == State.Move && _attacked)
        {
            _attacked = false;
        }

    }

    public void ChangeDestination()
    {

        if (_stateMachine.currentStateType == State.Move)
        {
            Debug.Log("�ٲ�");
            _stateMachine.ChangeState(State.AvoidWater);
        }
        else if (_stateMachine.currentStateType == State.AvoidWater)
        {
            Debug.Log("�ٽùٲ�");
            _stateMachine.ChangeState(State.Move);
        }
        
    }


    // (�ӽ�) ���� �ѷ��� �ݶ��̴��� ���������� �׾��ٰ� �Ǵ��մϴ�.
    // �ʸ���� �� �� ��ũ��Ʈ���� Ondead ȣ��
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Boundary")
        {
            _enemyCharacter.OnDead();
        }
    }


#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (_enemyCharacter != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(transform.position, _enemyCharacter.DetectPlayerDistance);
        }
    }
#endif
}
