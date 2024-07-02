using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scarecrow : MonoBehaviour, IHit
{
    private float _LastProvocationTime;

    private Rigidbody _Rigidbody;

    private PlayerCharacterBase _Owner;

    private void Awake()
    {        
        _Rigidbody = GetComponent<Rigidbody>();
        _Owner = FindAnyObjectByType<PlayerCharacterBase>();
    }

    private void Update()
    {
        if (_Rigidbody.velocity.y > 0.0f)
            _Rigidbody.velocity = Vector3.zero;

        if(_LastProvocationTime + 2.0f < Time.time)
        {
            Provocation();
        }
    }

    private void Provocation()
    {
        Collider[] enemys = Physics.OverlapSphere(transform.position, 50.0f, LayerMask.GetMask("Enemy"));

        foreach(Collider enemy in enemys)
        {
            enemy.GetComponent<EnemyAIController>().target = this.gameObject;
        }

        _LastProvocationTime = Time.time;
    }

    private void OnDestroy()
    {
        Collider[] enemys = Physics.OverlapSphere(transform.position, 50.0f, LayerMask.GetMask("Enemy"));

        foreach (Collider enemy in enemys)
        {
            enemy.GetComponent<EnemyAIController>().target = _Owner.gameObject;
        }
    }

    public void OnDamaged(float distance, Vector3 direction)
    {
        
    }

    public void OnDead()
    {
        
    }
}
