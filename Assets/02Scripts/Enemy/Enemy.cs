using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

    public Material myMaterial;

    public event System.Action onDead;

    protected virtual void Start()
    {
        stateMachine = GetComponent<StateMachine>();
    }

    public abstract void OnDamaged(float distance, Vector3 direction);

    public virtual void OnDead()
    {
        onDead?.Invoke();
    }
}

