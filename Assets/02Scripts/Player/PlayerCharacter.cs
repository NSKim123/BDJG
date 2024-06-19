using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// �÷��̾��� ĳ���Ϳ� ���� ������Ʈ�Դϴ�.
/// </summary>
public class PlayerCharacter : PlayerCharacterBase, IHit
{
    /// <summary>
    /// �� ĳ���Ͱ� �׾������� ��Ÿ���ϴ�.
    /// </summary>
    private bool _IsDead;

    /// <summary>
    /// �ൿ �Ұ� ���������� ��Ÿ���ϴ�.
    /// </summary>
    private bool _IsStunned;

    private bool _AbleToLevelUp = true;

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

    /// <summary>
    /// �ൿ �Ұ� ���� ���� �� ȣ��Ǵ� �̺�Ʈ�Դϴ�.
    /// </summary>
    public event System.Action onStunnedEnter;

    /// <summary>
    /// ��� �� ȣ��Ǵ� �޼����Դϴ�.
    /// </summary>
    public event System.Action onDead;


    private void Awake()
    {
        // �̺�Ʈ �Լ��� ���ε��մϴ�.
        BindEventFunction();

        // ���� �ý����� �����մϴ�.
        _BuffSystem = new BuffSystem(this.gameObject);
    }

    private void Start()
    {
        // ���� �ý����� �����մϴ�.
        InitLevelSystem();

        // test
        _BuffSystem.AddBuff(100000);        
        _BuffSystem.AddBuff(100002);
        _BuffSystem.AddBuff(100003);

        // test
        testCoroutine = StartCoroutine(Test_IncreaseKillCountPer5s());
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
        //while(true)
        {
            yield return new WaitForSeconds(5.0f);
            _BuffSystem.AddBuff(100001);
           // _LevelSystem.IncreaseKillCount();
        }
    }

    /// <summary>
    /// ���� �ý����� �����ϰ� ������ �̺�Ʈ�� ���ε��ϴ� �޼����Դϴ�.
    /// </summary>
    private void InitLevelSystem()
    {
        // ���� �ý����� �����մϴ�.
        _LevelSystem = new LevelSystem();

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

    public void OnStartGiant()
    {
        // ������ �̺�Ʈ�� �Ͻ������� ȣ���� �����ϴ�.
        _AbleToLevelUp = false;

        // ���� �ִϸ��̼� �̺�Ʈ ���ε�
        animController.onLand += attackComponent.AttackAround;

        // �鿪���� ����
        movementComponent.SetImmuneState(true);

        // �� ����,
        modelComponent.OnGiantStart();

        // źâ ������ �ʱ�ȭ �� ����
        attackComponent.OnGiantStart();        
    }

    public void OnFinishGiant()
    {
        // ������ �̺�Ʈ �簳
        _AbleToLevelUp = true;

        // ���� �ִϸ��̼� �̺�Ʈ ����ε�
        animController.onLand -= attackComponent.AttackAround;

        // �鿪���� ���󺹱�
        movementComponent.SetImmuneState(false);

        // �� ���󺹱�
        modelComponent.OnLevelUp(_LevelSystem.level);

        // źâ ���󺹱�
        attackComponent.OnLevelUp(_LevelSystem.level);
        attackComponent.OnGiantFinish();
    }

    /// <summary>
    /// �̵� �Է��� �޾��� �� ȣ��Ǵ� �޼����Դϴ�.
    /// </summary>
    /// <param name="inputDirection"> �Է¹��� �̵� �����Դϴ�.</param>
    public void OnMoveInput(Vector2 inputDirection)
    {
        movementComponent.OnMoveInput(inputDirection);
    }

    /// <summary>
    /// ȸ�� �Է��� �޾��� �� ȣ��Ǵ� �޼����Դϴ�.
    /// </summary>
    /// <param name="inputDelta"> �Է¹��� ȸ�� ���Դϴ�.</param>
    public void OnTurnInput(Vector2 inputDelta)
    {
        movementComponent.OnTurnInput(inputDelta.x);
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

        // �ǰ� ����Ʈ ǥ��
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
