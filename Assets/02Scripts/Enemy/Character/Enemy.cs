using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[Serializable]
// Enemy�� ���� ����. ���� �����ͷ� �޾ƿ��� ������ ���� ��ӹ޽��ϴ�.
public abstract class Enemy : MonoBehaviour, IHit
{
    public abstract EEnemyType Type { get; set; }
    public abstract float MoveSpeed { get; set; }
    public abstract float AttackRange { get; set; }
    public abstract float AttackForce { get; set; }
    public abstract float AttackTime { get; set; }
    public abstract float AttackSpeed { get; set; }
    public abstract float Damage_Distance { get; set; }
    public abstract Vector3 Damage_Direction { get; set; }

    #region Ư����ü�� Ư�����ݿ�
    public abstract float SpecialAttackCoolTime { get; set; }
    public abstract float SpecialAttackRange {  get; set; }
    public abstract float SpecialAttackTime { get; set; }
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

    protected virtual void Start()
    {
        stateMachine = GetComponent<StateMachine>();
        dieRenderer = transform.GetChild(0).GetChild(0).GetComponent<SkinnedMeshRenderer>();
    }

    protected virtual void OnEnable()
    {
        stateMachine = GetComponent<StateMachine>();
        dieRenderer = transform.GetChild(0).GetChild(0).GetComponent<SkinnedMeshRenderer>();
    }

    public virtual void OnDamaged(float distance, Vector3 direction)
    {
        // �ǰ� ����Ʈ ����
        //GameObject effect = Instantiate(Effect_Hit);
        GameObject effect = ObjectPoolManager.Instance.GetFromPool(EPoolType.Effect_Hit);
      
        effect.SetActive(true);

        effect.transform.position = transform.position + Vector3.up * GetComponent<CapsuleCollider>().height * 0.5f;
        effect.transform.SetParent(transform);

    }

    public void RequestCloudAttack(SpecialMushroom mushroom)
    {
        OnRequestCloudAttack?.Invoke(mushroom);
    }

    public void RequestSpecialAttack(SpecialCactus cactus)
    {
        OnRequestThornAttack?.Invoke(cactus);
    }

    public virtual void OnDead()
    {
        onDead?.Invoke();
        OnRequestSpawnItem?.Invoke(Type);
    }

    private void OnDisable()
    {
        stateMachine.currentStateType = EState.Init;
        dieRenderer.material = defaultMat;
    }

}

