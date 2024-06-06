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
    // raycast�� player�����ؼ� move �Ǵ� attack
    // ���¸� �����ְ�

    public EnemyState currentState;
    public event Action<GameObject> onStartAttack;
    public event Action<GameObject> onStartMove;
    public GameObject target;
    

    [SerializeField] private float _attackDistance;
    [SerializeField] private LayerMask _targetLayer;
    [SerializeField] private bool _attacked = false;


    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    private void FixedUpdate()
    {

        // Ÿ��(�÷��̾�) ����
        if (Physics.OverlapSphere(transform.position, _attackDistance, _targetLayer).Length > 0 && !_attacked)
        {
            _attacked = true;
            currentState = EnemyState.attack;
            onStartAttack?.Invoke(target);
            Debug.Log(currentState);
            Debug.Log("����");

        }
        else
        {
            currentState = EnemyState.move;
            onStartMove?.Invoke(target);
        }

        if (currentState == EnemyState.move && _attacked)
        {
            _attacked = false;
        }


    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, _attackDistance);
    }
}
