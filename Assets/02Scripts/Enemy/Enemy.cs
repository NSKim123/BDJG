using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
// Enemy에 대한 정보. 값을 데이터로 받아오고 각각의 적이 상속받습니다.
public abstract class Enemy : MonoBehaviour, IHit
{
    public abstract float MoveSpeed { get; set; }
    public abstract float DetectPlayerDistance { get; set; }
    public abstract float AttackForce { get; set; }
    public abstract float AttackTime { get; set; }
    public abstract float Damage_Distance { get; set; }
    public abstract Vector3 Damage_Direction { get; set; }
    
    protected StateMachine stateMachine;
    protected Rigidbody rigid;


    protected virtual void Start()
    {
        stateMachine = GetComponent<StateMachine>();
        rigid = GetComponent<Rigidbody>();
    }

    public abstract void OnDamaged(float distance, Vector3 direction);

    public void OnDead()
    {
        // 투명하게 사라지기
        // 적마다 다르다면 각자 구현
        // test
        Debug.Log("die");
        Destroy(gameObject);
    }
}

