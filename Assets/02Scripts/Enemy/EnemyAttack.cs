using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private Rigidbody _rigid;
    [SerializeField] private GameObject _target;
    [SerializeField] private float _attackForce;

    private void Start()
    {
        _rigid = GetComponent<Rigidbody>();
    }

    //private void FixedUpdate()
    //{
    //    if (Vector3.Distance(_target.transform.position, transform.position) < 2)
    //    {
    //        AttackPlayer();
    //    }
    //}

    // 플레이어를 밀어서 공격
    public void AttackPlayer(GameObject target)
    {
      
        _rigid.AddForce((target.transform.position - transform.position).normalized * _attackForce, ForceMode.Impulse);

    }
}
