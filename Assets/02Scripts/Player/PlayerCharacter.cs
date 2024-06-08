using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 플레이어의 캐릭터에 대한 컴포넌트입니다.
/// </summary>
public class PlayerCharacter : MonoBehaviour, IHit
{
    /// <summary>
    /// 이 캐릭터가 죽었는지를 나타냅니다.
    /// </summary>
    private bool _IsDead;

    /// <summary>
    /// 이 캐릭터의 레벨 시스템
    /// </summary>
    private LevelSystem _LevelSystem;

    /// <summary>
    /// 이동 컴포넌트
    /// </summary>
    private PlayerMovement _PlayerMovement;

    /// <summary>
    /// 공격 컴포넌트
    /// </summary>
    private PlayerAttack _PlayerAttack;

    /// <summary>
    /// 모델 컴포넌트
    /// </summary>
    private PlayerModel _PlayerModel;

    /// <summary>
    /// 이 캐릭터가 죽었는지를 나타내는 읽기 전용 프로퍼티입니다.
    /// </summary>
    public bool isDead => _IsDead;

    /// <summary>
    /// 이동 컴포넌트에 대한 읽기 전용 프로퍼티입니다.
    /// </summary>
    public PlayerMovement movementComponent => _PlayerMovement ?? (_PlayerMovement = GetComponent<PlayerMovement>());

    /// <summary>
    /// 공격 컴포넌트에 대한 읽기 전용 프로퍼티입니다.
    /// </summary>
    public PlayerAttack attackComponent => _PlayerAttack ?? (_PlayerAttack = GetComponent<PlayerAttack>());

    /// <summary>
    /// 모델 컴포넌트에 대한 읽기 전용 프로퍼티입니다.
    /// </summary>
    public PlayerModel modelComponent => _PlayerModel ?? (_PlayerModel = GetComponent<PlayerModel>());

    private void Start()
    {
        // 레벨 시스템을 생성합니다.
        InitLevelSystem();

        // test
        testCoroutine = StartCoroutine(Test_IncreaseKillCountPer5s());
    }
    private void Update()
    {
        // 레벨 시스템의 생존 시간을 갱신합니다.
        _LevelSystem.UpdateSurvivalTime();

        // 탄환 게이지 정보를 이용하고 있는 객체에 탄환 게이지 정보를 전달합니다. 
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
    /// 레벨 시스템을 생성하는 메서드입니다.
    /// </summary>
    private void InitLevelSystem()
    {
        // 레벨 시스템을 생성합니다.
        _LevelSystem = new LevelSystem();

        // 레벨업할 때 호출되어야하는 함수들을 바인딩합니다.
        _LevelSystem.onLevelUp += modelComponent.OnLevelUp;
        _LevelSystem.onLevelUp += attackComponent.OnLevelUp;
        _LevelSystem.onLevelUp += movementComponent.OnLevelUp;

        // 레벨 시스템 내부에서의 초기화를 진행합니다.
        _LevelSystem.Initailize();
    }
    
    /// <summary>
    /// 탄환 게이지 정보를 전달하여 모델 스케일을 조정하게 하는 메서드입니다.
    /// </summary>
    private void SetModelScaleByBulletGauge()
    {
        modelComponent.UpdateTargetScale(attackComponent.bulletGauge.currentValue);
    }

    /// <summary>
    /// 탄환 게이지 정보를 전달하여 방어력을 조정하게 하는 메서드입니다.
    /// </summary>
    private void SetDefenceByBulletGauge()
    {
        movementComponent.UpdateDefence(attackComponent.bulletGauge.ratio);
    }

    /// <summary>
    /// 탄환 게이지 정보를 이용하고 있는 객체들에게 탄환 게이지 정보를 전달하는 메서드입니다.
    /// </summary>
    private void UpdateBulletGaugeInfo()
    {
        SetModelScaleByBulletGauge();
        SetDefenceByBulletGauge();
    }

    /// <summary>
    /// 이동 입력을 받았을 때 호출되는 메서드입니다.
    /// </summary>
    /// <param name="inputDirection"> 입력받은 이동 방향입니다.</param>
    public void OnMoveInput(Vector2 inputDirection)
    {
        movementComponent.OnMoveInput(inputDirection);
    }

    /// <summary>
    /// 회전 입력을 받았을 때 호출되는 메서드입니다.
    /// </summary>
    /// <param name="inputDelta"> 입력받은 회전 값입니다.</param>
    public void OnTurnInput(Vector2 inputDelta)
    {
        movementComponent.OnTurnInput(inputDelta.x);
    }

    /// <summary>
    /// 점프 입력을 받았을 때 호출되는 메서드입니다.
    /// </summary>
    public void OnJumpInput()
    {
        movementComponent?.OnJumpInput();
    }

    /// <summary>
    /// 공격 입력을 받았을 때 호출되는 메서드입니다.
    /// </summary>
    public void OnAttackInput()
    {
        attackComponent?.OnAttackInput();
    }

    /// <summary>
    /// 공격을 받을 시 호출되는 메서드입니다.
    /// </summary>
    /// <param name="distance"> 밀려날 거리</param>
    /// <param name="direction"> 밀려날 방향</param>
    public void OnDamaged(float distance, Vector3 direction)
    {
        // 밀려나도록 movement 컴포넌트에 명령합니다.
        movementComponent.OnHit(distance, direction);

        // 피격 이펙트 표시
    }

    /// <summary>
    /// 죽을 시 호출되는 메서드입니다.
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
