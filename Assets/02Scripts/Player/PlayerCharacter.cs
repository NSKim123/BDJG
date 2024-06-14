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

    /// <summary>
    /// �� ĳ������ ���� �ý���
    /// </summary>
    private LevelSystem _LevelSystem;

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
    }

    private void Start()
    {
        // ���� �ý����� �����մϴ�.
        InitLevelSystem();

        // test
        testCoroutine = StartCoroutine(Test_IncreaseKillCountPer5s());
    }

    private void Update()
    {
        // źȯ ������ ������ �̿��ϰ� �ִ� ��ü�� źȯ ������ ������ �����մϴ�. 
        UpdateBulletGaugeInfo();

        // �ֹ̳��̼� �Ķ���͸� �����մϴ�.
        UpdateAnimationParameter();
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
            _LevelSystem.IncreaseKillCount();
        }
    }

    /// <summary>
    /// ���� �ý����� �����ϴ� �޼����Դϴ�.
    /// </summary>
    private void InitLevelSystem()
    {
        // ���� �ý����� �����մϴ�.
        _LevelSystem = new LevelSystem();

        // �������� �� ȣ��Ǿ���ϴ� �Լ����� ���ε��մϴ�.
        _LevelSystem.onLevelUp += modelComponent.OnLevelUp;
        _LevelSystem.onLevelUp += attackComponent.OnLevelUp;
        _LevelSystem.onLevelUp += movementComponent.OnLevelUp;        

        // ���� �ý��� ���ο����� �ʱ�ȭ�� �����մϴ�.
        _LevelSystem.Initailize();
    }

    /// <summary>
    /// �̺�Ʈ �Լ��� ���ε��մϴ�.
    /// </summary>
    private void BindEventFunction()
    {
        // ���� ����� �� ȣ��Ǿ���ϴ� �Լ����� ���ε��մϴ�.
        modelComponent.OnModelChanged += ResetAnimController;
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
    /// �ִϸ��̼� �Ķ���͸� �����մϴ�.
    /// </summary>
    private void UpdateAnimationParameter()
    {
        animController.UpdateMoveParam(movementComponent.normalizedZXSpeed);
        animController.UpdateGroundedParam(movementComponent.isGrounded);
    }

    /// <summary>
    /// PlayerAnimController ��ü�� �缳���մϴ�.
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

    }

    public void UpdateSurvivalTime(float newTime)
    {
        _LevelSystem.UpdateSurvivalTime(newTime);
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
