using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[Serializable]
// Enemy�� ���� ����. ���� �����ͷ� �޾ƿ��� ������ ���� ��ӹ޽��ϴ�.
public abstract class Enemy : MonoBehaviour, IHit
{
    public EEnemyType Type { get; set; }
    public float MoveSpeed { get; set; }
    public float AttackRange { get; set; }
    public float AttackForce { get; set; }
    public float AttackTime { get; set; }
    public float AttackSpeed { get; set; }
    public float Damage_Distance { get; set; }
    public Vector3 Damage_Direction { get; set; }

    #region Ư����ü�� Ư�����ݿ�
    public float SpecialAttackCoolTime { get; set; }
    public float SpecialAttackRange { get; set; }
    public float SpecialAttackTime { get; set; }
    #endregion


    protected StateMachine stateMachine;

    [HideInInspector] public SkinnedMeshRenderer dieRenderer;

    [Header("# �ǰ� ����Ʈ")]
    public GameObject Effect_Hit;

    [Header("# ���� ���͸���")]
    [SerializeField] private Material defaultMat;

    [Header("# ��� �� ����� ���͸���")]
    public Material changeMat;

    public event System.Action onDead;

    public event Action<EEnemyType> OnRequestSpawnItem;
    public event Action<SpecialCactus> OnRequestThornAttack;
    public event Action<SpecialMushroom> OnRequestCloudAttack;

    public bool isReused = false;

    protected bool isInitialized = false;

    protected virtual void Start()
    {
        stateMachine = GetComponent<StateMachine>();
        dieRenderer = transform.GetChild(0).GetChild(0).GetComponent<SkinnedMeshRenderer>();
        isInitialized = true;
    }


    public virtual void OnDamaged(float distance, Vector3 direction)
    {
        // �ǰ� ����Ʈ ����
        GameObject effect = ObjectPoolManager.Instance.GetFromPool(EPoolType.Effect_Hit);
        effect.SetActive(true);
        effect.transform.position = transform.position + Vector3.up * GetComponent<CapsuleCollider>().height * 0.5f;
        effect.transform.SetParent(transform);

        Damage_Distance = distance;
        Damage_Direction = direction;
        stateMachine.ChangeState_AllowSameState(EState.Hurt);
    }

    public virtual void OnDead()
    {
        onDead?.Invoke();
        OnRequestSpawnItem?.Invoke(Type);
        stateMachine.ChangeState(EState.Die);
    }

    public virtual void RequestCloudAttack(SpecialMushroom mushroom)
    {
        OnRequestCloudAttack?.Invoke(mushroom);
    }

    public void RequestSpecialAttack(SpecialCactus cactus)
    {
        OnRequestThornAttack?.Invoke(cactus);
    }


    private void OnDisable()
    {   
        if (isInitialized)
        {
            stateMachine.currentStateType = EState.Init;
            dieRenderer.material = defaultMat;
        }
    }

}

