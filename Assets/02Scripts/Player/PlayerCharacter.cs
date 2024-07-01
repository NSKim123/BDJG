using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// �÷��̾��� ĳ���Ϳ� ���� ������Ʈ�Դϴ�.
/// </summary>
public partial class PlayerCharacter : PlayerCharacterBase, IHit
{
    [Header("# �Ŵ�ȭ �������϶� ���� �� ����Ʈ")]
    public GameObject m_Effect_GiantSlimeLand;

    [Header("# �Ŵ�ȭ ���� �� ������ UI ����Ʈ")]
    public GameObject m_UIEffect_GiantStarted;

    [Header("# �ǰ� ����Ʈ")]
    public GameObject m_Effect_Hit;

    /// <summary>
    /// �� ĳ���Ͱ� �׾������� ��Ÿ���ϴ�.
    /// </summary>
    private bool _IsDead;

    /// <summary>
    /// �ൿ �Ұ� ���������� ��Ÿ���ϴ�.
    /// </summary>
    private bool _IsStunned;

    private bool _AbleToLevelUp = true;

    private List<int> _ItemSlots;

    /// <summary>
    /// �� ĳ������ ���� �ý��� ��ü
    /// </summary>
    private LevelSystem _LevelSystem;

    /// <summary>
    /// �� ĳ������ ���� �ý��� ��ü
    /// </summary>
    private BuffSystem _BuffSystem;

    /// <summary>
    /// �̵� ������Ʈ
    /// </summary>
    private PlayerMovement _PlayerMovement;

    /// <summary>
    /// ���� ������Ʈ
    /// </summary>
    private PlayerAttack _PlayerAttack;

    /// <summary>
    /// �� ������Ʈ
    /// </summary>
    private PlayerModel _PlayerModel;

    /// <summary>
    /// �ִϸ����͸� �����ϴ� ������Ʈ
    /// </summary>
    private PlayerAnimController _PlayerAnimController;

    /// <summary>
    /// �� ĳ���Ͱ� �׾������� ��Ÿ���� �б� ���� ������Ƽ�Դϴ�.
    /// </summary>
    public bool isDead => _IsDead;

    /// <summary>
    /// �ൿ �Ұ� ���������� ��Ÿ���� �б� ���� ������Ƽ�Դϴ�.
    /// </summary>
    public bool isStunned => _IsStunned;    

    /// <summary>
    /// ���� �ý��� ��ü�� ���� �б� ���� ������Ƽ�Դϴ�.
    /// </summary>
    public BuffSystem buffSystem => _BuffSystem;

    /// <summary>
    /// ���� �ý��� ��ü�� ���� �б� ���� ������Ƽ�Դϴ�.
    /// </summary>
    public LevelSystem levelSystem => _LevelSystem;

    /// <summary>
    /// �̵� ������Ʈ�� ���� �б� ���� ������Ƽ�Դϴ�.
    /// </summary>
    public PlayerMovement movementComponent => _PlayerMovement ?? (_PlayerMovement = GetComponent<PlayerMovement>());

    /// <summary>
    /// ���� ������Ʈ�� ���� �б� ���� ������Ƽ�Դϴ�.
    /// </summary>
    public PlayerAttack attackComponent => _PlayerAttack ?? (_PlayerAttack = GetComponent<PlayerAttack>());

    /// <summary>
    /// �� ������Ʈ�� ���� �б� ���� ������Ƽ�Դϴ�.
    /// </summary>
    public PlayerModel modelComponent => _PlayerModel ?? (_PlayerModel = GetComponent<PlayerModel>());

    /// <summary>
    /// �ִϸ����͸� �����ϴ� ������Ʈ�� ���� �б� ���� ������Ƽ�Դϴ�.
    /// </summary>
    public PlayerAnimController animController => _PlayerAnimController ?? (_PlayerAnimController = GetComponentInChildren<PlayerAnimController>());

    public event System.Action<List<int>> onItemSlotsChanged;

    /// <summary>
    /// �ൿ �Ұ� ���� ���� �� ȣ��Ǵ� �̺�Ʈ�Դϴ�.
    /// </summary>
    public event System.Action onStunnedEnter;

    /// <summary>
    /// ��� �� ȣ��Ǵ� �޼����Դϴ�.
    /// </summary>
    public event System.Action onDead;

    /// <summary>
    /// ������Ÿ���� ���� �ӽ� �̺�Ʈ�Դϴ�.
    /// �Ŵ�ȭ�� ���� �� ȣ��˴ϴ�.
    /// </summary>
    public event Action onGiantEnd;


    private void Awake()
    {
        // ���� �ý����� �����մϴ�.
        _LevelSystem = new LevelSystem();

        // �̺�Ʈ �Լ��� ���ε��մϴ�.
        BindEventFunction();

        // ���� �ý����� �����մϴ�.
        _BuffSystem = new BuffSystem(this.gameObject);

        _ItemSlots = new List<int>();
    }

    private void Start()
    {
        InitLevelSystem();

        // test
        //testCoroutine = StartCoroutine(Test_IncreaseKillCountPer5s());
    }

    private void Update()
    {
        // źȯ ������ ������ �̿��ϰ� �ִ� ��ü�� źȯ ������ ������ �����մϴ�. 
        UpdateBulletGaugeInfo();

        // �ִϸ��̼� �Ķ���͸� �����մϴ�.
        UpdateAnimationParameter();

        // ���� �ý����� �����մϴ�.
        UpdateBuffList();
    }

    // test
    Coroutine testCoroutine;
    private void OnDestroy()
    {
        if(testCoroutine != null)
            StopCoroutine(testCoroutine);
    }

    //test
    private IEnumerator Test_IncreaseKillCountPer5s()
    {
        while(true)
        {
           yield return new WaitForSeconds(5.0f);
           //_BuffSystem.AddBuff(100001);
           // _LevelSystem.IncreaseKillCount();
        }
    }

    /// <summary>
    /// ���� �ý����� �����ϰ� ������ �̺�Ʈ�� ���ε��ϴ� �޼����Դϴ�.
    /// </summary>
    private void InitLevelSystem()
    {
        // �������� �� ȣ��Ǿ���ϴ� �Լ����� ���ε��մϴ�.
        _LevelSystem.onLevelUp += (int level) => StartCoroutine(OnLevelUpCoroutine(level));
        
        // ���� �ý��� ���ο����� �ʱ�ȭ�� �����մϴ�.
        _LevelSystem.Initailize();
    }

    private IEnumerator OnLevelUpCoroutine(int level)
    {
        yield return new WaitUntil(() => _AbleToLevelUp);

        OnLevelUp(level);
    }

    private void OnLevelUp(int level)
    {
        modelComponent.OnLevelUp(level);
        attackComponent.OnLevelUp(level);
        movementComponent.OnLevelUp(level);
    }

    /// <summary>
    /// �̺�Ʈ �Լ��� ���ε��ϴ� �޼����Դϴ�.
    /// </summary>
    private void BindEventFunction()
    {
        // ���� ����� �� ȣ��Ǿ���ϴ� �Լ����� ���ε��մϴ�.
        modelComponent.onModelChanged += ResetAnimController;
    }
    
    /// <summary>
    /// źȯ ������ ������ �����Ͽ� �� �������� �����ϰ� �ϴ� �޼����Դϴ�.
    /// </summary>
    private void SetModelScaleByBulletGauge()
    {
        modelComponent.UpdateTargetScale(attackComponent.bulletGauge.currentValue);
    }

    /// <summary>
    /// źȯ ������ ������ �����Ͽ� ������ �����ϰ� �ϴ� �޼����Դϴ�.
    /// </summary>
    private void SetDefenceByBulletGauge()
    {
        movementComponent.UpdateDefence(attackComponent.bulletGauge.ratio);
    }

    /// <summary>
    /// źȯ ������ ������ �̿��ϰ� �ִ� ��ü�鿡�� źȯ ������ ������ �����ϴ� �޼����Դϴ�.
    /// </summary>
    private void UpdateBulletGaugeInfo()
    {
        SetModelScaleByBulletGauge();
        SetDefenceByBulletGauge();
    }

    /// <summary>
    /// �ִϸ��̼� �Ķ���͸� �����ϴ� �޼����Դϴ�.
    /// </summary>
    private void UpdateAnimationParameter()
    {
        animController?.UpdateMoveParam(movementComponent.normalizedZXSpeed);
        animController?.UpdateGroundedParam(movementComponent.isGrounded);
    }

    /// <summary>
    /// ���� �ý����� �����ϴ� �޼����Դϴ�.
    /// </summary>
    private void UpdateBuffList()
    {
        _BuffSystem.UpdateBuffList();
    }

    /// <summary>
    /// PlayerAnimController ��ü�� �缳���ϴ� �޼����Դϴ�.
    /// </summary>
    private void ResetAnimController()
    {
        _PlayerAnimController = modelComponent.currentModel.GetComponentInChildren<PlayerAnimController>();
    }    

    /// <summary>
    /// ĳ���͸� �ʱ�ȭ�ϴ� �޼����Դϴ�.
    /// </summary>
    public void ResetPlayerCharacter()
    {
        // ���� �ý��� ���ο����� �ʱ�ȭ�� �����մϴ�.
        _LevelSystem.Initailize();

        _ItemSlots.Clear();
        onItemSlotsChanged?.Invoke(_ItemSlots);

        // �ൿ�Ұ� ����, ��� ���¸� �ʱ�ȭ�մϴ�.
        _IsDead = false;
        _IsStunned = false;
    }

    /// <summary>
    /// ���� �ð��� �����ϴ� �޼����Դϴ�.
    /// </summary>
    /// <param name="newTime"> ������ �ð�</param>
    public void UpdateSurvivalTime(float newTime)
    {
        _LevelSystem.UpdateSurvivalTime(newTime);
    }

    /// <summary>
    /// ������ �߰��ϴ� �޼����Դϴ�.
    /// </summary>
    /// <param name="buffCode"> �ο��� ������ �ڵ�</param>
    public void AddBuff(int buffCode)
    {
        _BuffSystem.AddBuff(buffCode);
    }

    

    

    /// <summary>
    /// �̵� �Է��� �޾��� �� ȣ��Ǵ� �޼����Դϴ�.
    /// </summary>
    /// <param name="inputDirection"> �Է¹��� �̵� �����Դϴ�.</param>
    public void OnMoveInput(Vector2 inputDirection)
    {
        movementComponent.OnMoveInput(inputDirection);
    }

    public void OnUseItemInput()
    {
        UseItem();
    }

    /// <summary>
    /// ���� �Է��� �޾��� �� ȣ��Ǵ� �޼����Դϴ�.
    /// </summary>
    public void OnJumpInput()
    {
        movementComponent?.OnJumpInput();
    }

    /// <summary>
    /// ���� �Է��� �޾��� �� ȣ��Ǵ� �޼����Դϴ�.
    /// </summary>
    public void OnAttackInput()
    {
        attackComponent?.OnAttackInput();
    }

    /// <summary>
    /// ������ ���� �� ȣ��Ǵ� �޼����Դϴ�.
    /// </summary>
    /// <param name="distance"> �з��� �Ÿ�</param>
    /// <param name="direction"> �з��� ����</param>
    public void OnDamaged(float distance, Vector3 direction)
    {
        // �з������� movement ������Ʈ�� ����մϴ�.
        movementComponent.OnHit(distance, direction);

        // �ǰ� ����Ʈ ����
        GameObject effect = Instantiate(m_Effect_Hit);
        effect.transform.position = transform.position;
        effect.transform.SetParent(transform);
    }

    /// <summary>
    /// ���� �� ȣ��Ǵ� �޼����Դϴ�.
    /// </summary>
    public void OnDead()
    {
        _IsDead = true;

        onDead?.Invoke();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_LevelSystem == null) return;        

        _LevelSystem.OnDrawGizmos(transform.position);        
    }
#endif

}

public partial class PlayerCharacter
{
    public void AddItem(int itemBuffCode)
    {
        // ����� �������� 2�� �̻��̶�� ȣ�� ����
        if (_ItemSlots.Count >= 2) return;

        _ItemSlots.Add(itemBuffCode);

        onItemSlotsChanged?.Invoke(_ItemSlots);
    }

    public void UseItem()
    {
        // �������� ���ٸ� ȣ�� ����
        if (_ItemSlots.Count == 0) return;

        // ������ ���� Ÿ���� ������ �����Ѵٸ� ȣ�� ����
        if (_BuffSystem.IsOtherItemBuffActive()) return;

        _BuffSystem.AddBuff(_ItemSlots[0]);
        _ItemSlots.RemoveAt(0);

        onItemSlotsChanged?.Invoke(_ItemSlots);
    }

    public void OnStartGiant()
    {
        // ������ �̺�Ʈ�� �Ͻ������� ȣ���� �����ϴ�.
        _AbleToLevelUp = false;

        // �鿪���� ����
        movementComponent.SetImmuneState(true);

        // �� ����
        //modelComponent.OnGiantStart();

        // ���� �ִϸ��̼� �̺�Ʈ ���ε�
        animController.onLand += attackComponent.AttackAround;
        animController.onLand += InstantiateLandEffect;
        animController.onLand += FollowCamera.ShakeCamera;

        // źâ ������ �ʱ�ȭ �� ����
        attackComponent.OnGiantStart();

        movementComponent.characterController.excludeLayers += LayerMask.GetMask("Enemy");

        GameObject UIEffect = Instantiate(m_UIEffect_GiantStarted);
        UIEffect.transform.SetParent(FindAnyObjectByType<Canvas>().transform);
        (UIEffect.transform as RectTransform).anchoredPosition = Vector2.zero;
    }

    public void OnFinishGiant()
    {
        // ������ �̺�Ʈ �簳
        _AbleToLevelUp = true;

        // ���� �ִϸ��̼� �̺�Ʈ ����ε�
        animController.onLand -= attackComponent.AttackAround;
        animController.onLand -= InstantiateLandEffect;
        animController.onLand -= FollowCamera.ShakeCamera;

        // �鿪���� ���󺹱�
        movementComponent.SetImmuneState(false);

        // �� ���󺹱�
        //modelComponent.OnLevelUp(_LevelSystem.level);

        // źâ ���󺹱�
        attackComponent.OnLevelUp(_LevelSystem.level);
        attackComponent.OnGiantFinish();

        // **�ӽ�** �Ŵ�ȭ ������ ȣ��
        onGiantEnd?.Invoke();

        movementComponent.characterController.excludeLayers -= LayerMask.GetMask("Enemy");
    }

    private void InstantiateLandEffect()
    {
        GameObject effect = Instantiate(m_Effect_GiantSlimeLand);

        effect.transform.position = transform.position + Vector3.down * movementComponent.characterController.height / 2.0f * transform.lossyScale.y;
    }

    private void OnStartMachineGun()
    {

    }

    private void OnFinishMachinGun()
    {

    }
}