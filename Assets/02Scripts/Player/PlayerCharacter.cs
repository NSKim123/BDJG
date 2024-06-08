using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// �÷��̾��� ĳ���Ϳ� ���� ������Ʈ�Դϴ�.
/// </summary>
public class PlayerCharacter : MonoBehaviour, IHit
{
    /// <summary>
    /// �� ĳ���Ͱ� �׾������� ��Ÿ���ϴ�.
    /// </summary>
    private bool _IsDead;

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
    /// �� ĳ���Ͱ� �׾������� ��Ÿ���� �б� ���� ������Ƽ�Դϴ�.
    /// </summary>
    public bool isDead => _IsDead;

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

    private void Start()
    {
        // ���� �ý����� �����մϴ�.
        InitLevelSystem();

        // test
        testCoroutine = StartCoroutine(Test_IncreaseKillCountPer5s());
    }
    private void Update()
    {
        // ���� �ý����� ���� �ð��� �����մϴ�.
        _LevelSystem.UpdateSurvivalTime();

        // źȯ ������ ������ �̿��ϰ� �ִ� ��ü�� źȯ ������ ������ �����մϴ�. 
        UpdateBulletGaugeInfo();
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
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_LevelSystem == null) return;        

        _LevelSystem.OnDrawGizmos(transform.position);        
    }
#endif

}
