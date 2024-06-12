using System;
using System.Collections;
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

    // 플레이어 오브젝트
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

    //IEnumerator C_WaitAttackTime()
    //{
    //    yield return new WaitForSeconds(_enemyCharacter.AttackTime);

    //    _attacked = false;

    //}

    private void Update()
    {
        // player를 감지해서 적의 상태를 move 또는 attack 으로 바꿉니다.

        attackDetect = Physics.OverlapSphere(transform.position, _enemyCharacter.DetectPlayerDistance, _targetLayer);

        // 타켓(플레이어) 감지
        if (attackDetect.Length > 0 && !_attacked)
        {
            //Debug.Log("타겟 감지");
            _attacked = true;
            _stateMachine.ChangeState(State.Attack);

        }
        //else
        //{
        //    Debug.Log("다시 무브");
        //    _stateMachine.ChangeState(State.Move);
        //}

        if (_stateMachine.currentStateType == State.Move && _attacked)
        {
            _attacked = false;
        }

   

    }

    // (임시) 맵을 둘러싼 콜라이더를 빠져나가면 죽었다고 판단합니다.
    // 맵만들면 맵 쪽 스크립트에서 Ondead 호출
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
