using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
// Enemy�� ���� ����. ���� �����ͷ� �޾ƿ��� ������ ���� ��ӹ޽��ϴ�.
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
        // �����ϰ� �������
        // ������ �ٸ��ٸ� ���� ����
        // test
        Debug.Log("die");
        Destroy(gameObject);
    }
}

