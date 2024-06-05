using System;
using UnityEngine;

public enum EnemyState
{
    idle,
    move,
    attack,
    damage,
    die,
}
public class EnemyAIController : MonoBehaviour
{
    // raycast로 player감지해서 move 또는 attack
    // 상태만 돌려주고

    public EnemyState currentState;
    public event Action<GameObject> onStartAttack;
    public event Action<GameObject> onStartMove;
    public GameObject target;
    

    [SerializeField] private float _attackDistance;
    [SerializeField] private LayerMask _targetLayer;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    private void FixedUpdate()
    {

        // 타켓(플레이어) 감지
        if (Physics.OverlapSphere(transform.position, _attackDistance, _targetLayer).Length > 0)
        {
            Debug.Log("감지");
            currentState = EnemyState.attack;
            onStartAttack?.Invoke(target);
        }
        else
        {
            currentState = EnemyState.move;
            onStartMove?.Invoke(target);
        }




    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, _attackDistance);
    }
}
