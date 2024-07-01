using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindArea : MonoBehaviour
{
    private Vector3 _WindDirection;

    private float _LastAttackTime;

    private float _AttackPeriod = 0.1f;

    private void Awake()
    {
        float randomTheta = Random.Range(0.0f, 2.0f * Mathf.PI);

        _WindDirection.z = Mathf.Cos(randomTheta);
        _WindDirection.x = Mathf.Sin(randomTheta);
        _WindDirection.y = 0.0f;
    }

    private void Update()
    {
        if(_LastAttackTime + _AttackPeriod < Time.time)
        {
            Push();
        }
    }

    private void Push()
    {
        Collider[] enemys = Physics.OverlapSphere(transform.position, 30.0f, LayerMask.GetMask("Enemy"));

        foreach (Collider enemy in enemys)
        {
            enemy.GetComponent<IHit>().OnDamaged(10.0f, _WindDirection);
        }

        _LastAttackTime = Time.time;
    }
}
