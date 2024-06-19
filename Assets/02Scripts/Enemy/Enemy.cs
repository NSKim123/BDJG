using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

    public SkinnedMeshRenderer dieRenderer;
    public Material changeMat;


    public BuffSystem BuffSystem => _buffSystem;
    private BuffSystem _buffSystem;

    public event System.Action onDead;

    protected virtual void Start()
    {
        stateMachine = GetComponent<StateMachine>();

        dieRenderer = transform.GetChild(0).GetChild(0).GetComponent<SkinnedMeshRenderer>();

        //_buffSystem = new BuffSystem(this.gameObject);
        //Debug.Log(this.gameObject);
    }

    public abstract void OnDamaged(float distance, Vector3 direction);

    public virtual void OnDead()
    {
        onDead?.Invoke();
    }

}

