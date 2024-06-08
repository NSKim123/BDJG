using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// �÷��̾��� ĳ���Ϳ� ���� ������Ʈ�Դϴ�.
/// </summary>
public class PlayerCharacter : MonoBehaviour, IHit
{
    /// <summary>
    /// �� ĳ������ ����
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

    private PlayerModel _PlayerModel;

    /// <summary>
    /// �̵� ������Ʈ�� ���� �б� ���� ������Ƽ�Դϴ�.
    /// </summary>
    public PlayerMovement movementComponent => _PlayerMovement ?? (_PlayerMovement = GetComponent<PlayerMovement>());

    /// <summary>
    /// ���� ������Ʈ�� ���� �б� ���� ������Ƽ�Դϴ�.
    /// </summary>
    public PlayerAttack attackComponent => _PlayerAttack ?? (_PlayerAttack = GetComponent<PlayerAttack>());

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
        _LevelSystem.UpdateSurvivalTime();
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
        _LevelSystem.onLevelUp += attackComponent.OnlevelUp;

        // ���� �ý��� ���ο����� �ʱ�ȭ�� �����մϴ�.
        _LevelSystem.Initailize();
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
        // �����Ͽ� �����̴� ���� �����մϴ�. 
        // TO DO : �̵� ������ Ǯ���� �۾��� �ִϸ��̼� �̺�Ʈ�� �߰����Ѿ���!!
        //movementComponent.SetMovable(false);

        // �з������� movement ������Ʈ�� ����մϴ�.
        movementComponent.OnHit(distance, direction);
    }

    /// <summary>
    /// ���� �� ȣ��Ǵ� �޼����Դϴ�.
    /// </summary>
    /// <exception cref="System.NotImplementedException"></exception>
    public void OnDead()
    {
        throw new System.NotImplementedException();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_LevelSystem == null) return;        

        _LevelSystem.OnDrawGizmos(transform.position);        
    }
#endif

}
