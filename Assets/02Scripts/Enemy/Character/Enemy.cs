using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[Serializable]
// Enemy에 대한 정보. 값을 데이터로 받아오고 각각의 적이 상속받습니다.
public abstract class Enemy : MonoBehaviour, IHit
{
    public abstract EnemyType Type { get; set; }
    public abstract float MoveSpeed { get; set; }
    public abstract float AttackRange { get; set; }
    public abstract float AttackForce { get; set; }
    public abstract float AttackTime { get; set; }
    public abstract float AttackSpeed { get; set; }
    public abstract float Damage_Distance { get; set; }
    public abstract Vector3 Damage_Direction { get; set; }

    #region 특수개체의 특수공격용
    public abstract float SpecialAttackCoolTime { get; set; }
    public abstract float SpecialAttackRange {  get; set; }
    public abstract float SpecialAttackTime { get; set; }
    #endregion


    protected StateMachine stateMachine;

    [HideInInspector] public SkinnedMeshRenderer dieRenderer;

    [Header("# 피격 이펙트")]
    public GameObject Effect_Hit;

    [Header("# 사망 시 변경될 머터리얼")]
    public Material changeMat;

    public event System.Action onDead;

    public event Action<EnemyType> OnRequestSpawnItem;

    protected virtual void Start()
    {
        stateMachine = GetComponent<StateMachine>();

        dieRenderer = transform.GetChild(0).GetChild(0).GetComponent<SkinnedMeshRenderer>();
    }

    public virtual void OnDamaged(float distance, Vector3 direction)
    {
        // 피격 이펙트 생성
        //GameObject effect = Instantiate(Effect_Hit);
        GameObject effect = ObjectPoolManager.Instance.GetFromPool(PoolType.Effect_Hit);
        effect.transform.position = transform.position + Vector3.up * GetComponent<CapsuleCollider>().height * 0.5f;
        effect.transform.SetParent(transform);        
    }

    public virtual void OnDead()
    {
        onDead?.Invoke();
        OnRequestSpawnItem?.Invoke(Type);

    }

}

