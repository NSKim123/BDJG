using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 플레이어의 캐릭터에 대한 컴포넌트입니다.
/// </summary>
public class PlayerCharacter : MonoBehaviour, IHit
{
    /// <summary>
    /// 이 캐릭터의 레벨
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

    private PlayerModel _PlayerModel;

    /// <summary>
    /// 이동 컴포넌트에 대한 읽기 전용 프로퍼티입니다.
    /// </summary>
    public PlayerMovement movementComponent => _PlayerMovement ?? (_PlayerMovement = GetComponent<PlayerMovement>());

    /// <summary>
    /// 공격 컴포넌트에 대한 읽기 전용 프로퍼티입니다.
    /// </summary>
    public PlayerAttack attackComponent => _PlayerAttack ?? (_PlayerAttack = GetComponent<PlayerAttack>());

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
    /// 레벨 시스템을 생성하는 메서드입니다.
    /// </summary>
    private void InitLevelSystem()
    {
        // 레벨 시스템을 생성합니다.
        _LevelSystem = new LevelSystem();

        // 레벨업할 때 호출되어야하는 함수들을 바인딩합니다.
        _LevelSystem.onLevelUp += modelComponent.OnLevelUp;
        _LevelSystem.onLevelUp += attackComponent.OnlevelUp;

        // 레벨 시스템 내부에서의 초기화를 진행합니다.
        _LevelSystem.Initailize();
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
        // 조작하여 움직이는 것을 제한합니다. 
        // TO DO : 이동 제한이 풀리는 작업을 애니메이션 이벤트에 추가시켜야함!!
        //movementComponent.SetMovable(false);

        // 밀려나도록 movement 컴포넌트에 명령합니다.
        movementComponent.OnHit(distance, direction);
    }

    /// <summary>
    /// 죽을 시 호출되는 메서드입니다.
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
