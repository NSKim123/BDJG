using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // 플레이어 방향으로 이동

    private Rigidbody _rigid;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private GameObject _target;
    private Vector3 _moveDirection;


    private void Start()
    {
        _rigid = GetComponent<Rigidbody>();
    }

    //private void FixedUpdate()
    //{
    //    MoveToPlayer();
    //}

    public void MoveToPlayer(GameObject target)
    {
        _moveDirection = target.transform.position - transform.position;
        _rigid.velocity = _moveDirection * _moveSpeed * Time.deltaTime;
    }


}
