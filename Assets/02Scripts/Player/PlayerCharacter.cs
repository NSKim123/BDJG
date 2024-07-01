using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 플레이어의 캐릭터에 대한 컴포넌트입니다.
/// </summary>
public partial class PlayerCharacter : PlayerCharacterBase, IHit
{
    [Header("# 거대화 슬라임일때 착지 시 이펙트")]
    public GameObject m_Effect_GiantSlimeLand;

    [Header("# 거대화 시작 시 생성될 UI 이펙트")]
    public GameObject m_UIEffect_GiantStarted;

    [Header("# 피격 이펙트")]
    public GameObject m_Effect_Hit;

    /// <summary>
    /// 이 캐릭터가 죽었는지를 나타냅니다.
    /// </summary>
    private bool _IsDead;

    /// <summary>
    /// 행동 불가 상태인지를 나타냅니다.
    /// </summary>
    private bool _IsStunned;

    private bool _AbleToLevelUp = true;

    private List<int> _ItemSlots;

    /// <summary>
    /// 이 캐릭터의 레벨 시스템 객체
    /// </summary>
    private LevelSystem _LevelSystem;

    /// <summary>
    /// 이 캐릭터의 버프 시스템 객체
    /// </summary>
    private BuffSystem _BuffSystem;

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
    /// 애니메이터를 관리하는 컴포넌트
    /// </summary>
    private PlayerAnimController _PlayerAnimController;

    /// <summary>
    /// 이 캐릭터가 죽었는지를 나타내는 읽기 전용 프로퍼티입니다.
    /// </summary>
    public bool isDead => _IsDead;

    /// <summary>
    /// 행동 불가 상태인지를 나타내는 읽기 전용 프로퍼티입니다.
    /// </summary>
    public bool isStunned => _IsStunned;    

    /// <summary>
    /// 버프 시스템 객체에 대한 읽기 전용 프로퍼티입니다.
    /// </summary>
    public BuffSystem buffSystem => _BuffSystem;

    /// <summary>
    /// 레벨 시스템 객체에 대한 읽기 전용 프로퍼티입니다.
    /// </summary>
    public LevelSystem levelSystem => _LevelSystem;

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

    /// <summary>
    /// 애니메이터를 관리하는 컴포넌트에 대한 읽기 전용 프로퍼티입니다.
    /// </summary>
    public PlayerAnimController animController => _PlayerAnimController ?? (_PlayerAnimController = GetComponentInChildren<PlayerAnimController>());

    public event System.Action<List<int>> onItemSlotsChanged;

    /// <summary>
    /// 행동 불가 상태 돌입 시 호출되는 이벤트입니다.
    /// </summary>
    public event System.Action onStunnedEnter;

    /// <summary>
    /// 사망 시 호출되는 메서드입니다.
    /// </summary>
    public event System.Action onDead;

    /// <summary>
    /// 프로토타입을 위한 임시 이벤트입니다.
    /// 거대화가 끝난 뒤 호출됩니다.
    /// </summary>
    public event Action onGiantEnd;


    private void Awake()
    {
        // 레벨 시스템을 생성합니다.
        _LevelSystem = new LevelSystem();

        // 이벤트 함수를 바인딩합니다.
        BindEventFunction();

        // 버프 시스템을 생성합니다.
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
        // 탄환 게이지 정보를 이용하고 있는 객체에 탄환 게이지 정보를 전달합니다. 
        UpdateBulletGaugeInfo();

        // 애니메이션 파라미터를 갱신합니다.
        UpdateAnimationParameter();

        // 버프 시스템을 갱신합니다.
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
    /// 레벨 시스템을 생성하고 레벨업 이벤트를 바인딩하는 메서드입니다.
    /// </summary>
    private void InitLevelSystem()
    {
        // 레벨업할 때 호출되어야하는 함수들을 바인딩합니다.
        _LevelSystem.onLevelUp += (int level) => StartCoroutine(OnLevelUpCoroutine(level));
        
        // 레벨 시스템 내부에서의 초기화를 진행합니다.
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
    /// 이벤트 함수를 바인딩하는 메서드입니다.
    /// </summary>
    private void BindEventFunction()
    {
        // 모델이 변경될 때 호출되어야하는 함수들을 바인딩합니다.
        modelComponent.onModelChanged += ResetAnimController;
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
    /// 애니메이션 파라미터를 갱신하는 메서드입니다.
    /// </summary>
    private void UpdateAnimationParameter()
    {
        animController?.UpdateMoveParam(movementComponent.normalizedZXSpeed);
        animController?.UpdateGroundedParam(movementComponent.isGrounded);
    }

    /// <summary>
    /// 버프 시스템을 갱신하는 메서드입니다.
    /// </summary>
    private void UpdateBuffList()
    {
        _BuffSystem.UpdateBuffList();
    }

    /// <summary>
    /// PlayerAnimController 객체를 재설정하는 메서드입니다.
    /// </summary>
    private void ResetAnimController()
    {
        _PlayerAnimController = modelComponent.currentModel.GetComponentInChildren<PlayerAnimController>();
    }    

    /// <summary>
    /// 캐릭터를 초기화하는 메서드입니다.
    /// </summary>
    public void ResetPlayerCharacter()
    {
        // 레벨 시스템 내부에서의 초기화를 진행합니다.
        _LevelSystem.Initailize();

        _ItemSlots.Clear();
        onItemSlotsChanged?.Invoke(_ItemSlots);

        // 행동불가 상태, 사망 상태를 초기화합니다.
        _IsDead = false;
        _IsStunned = false;
    }

    /// <summary>
    /// 생존 시간을 갱신하는 메서드입니다.
    /// </summary>
    /// <param name="newTime"> 설정할 시간</param>
    public void UpdateSurvivalTime(float newTime)
    {
        _LevelSystem.UpdateSurvivalTime(newTime);
    }

    /// <summary>
    /// 버프를 추가하는 메서드입니다.
    /// </summary>
    /// <param name="buffCode"> 부여할 버프의 코드</param>
    public void AddBuff(int buffCode)
    {
        _BuffSystem.AddBuff(buffCode);
    }

    

    

    /// <summary>
    /// 이동 입력을 받았을 때 호출되는 메서드입니다.
    /// </summary>
    /// <param name="inputDirection"> 입력받은 이동 방향입니다.</param>
    public void OnMoveInput(Vector2 inputDirection)
    {
        movementComponent.OnMoveInput(inputDirection);
    }

    public void OnUseItemInput()
    {
        UseItem();
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

        // 피격 이펙트 생성
        GameObject effect = Instantiate(m_Effect_Hit);
        effect.transform.position = transform.position;
        effect.transform.SetParent(transform);
    }

    /// <summary>
    /// 죽을 시 호출되는 메서드입니다.
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
        // 저장된 아이템이 2개 이상이라면 호출 종료
        if (_ItemSlots.Count >= 2) return;

        _ItemSlots.Add(itemBuffCode);

        onItemSlotsChanged?.Invoke(_ItemSlots);
    }

    public void UseItem()
    {
        // 아이템이 없다면 호출 종료
        if (_ItemSlots.Count == 0) return;

        // 아이템 버프 타입의 버프가 존재한다면 호출 종료
        if (_BuffSystem.IsOtherItemBuffActive()) return;

        _BuffSystem.AddBuff(_ItemSlots[0]);
        _ItemSlots.RemoveAt(0);

        onItemSlotsChanged?.Invoke(_ItemSlots);
    }

    public void OnStartGiant()
    {
        // 레벨업 이벤트를 일시적으로 호출을 막습니다.
        _AbleToLevelUp = false;

        // 면역상태 설정
        movementComponent.SetImmuneState(true);

        // 모델 변경
        //modelComponent.OnGiantStart();

        // 점프 애니메이션 이벤트 바인딩
        animController.onLand += attackComponent.AttackAround;
        animController.onLand += InstantiateLandEffect;
        animController.onLand += FollowCamera.ShakeCamera;

        // 탄창 게이지 초기화 및 설정
        attackComponent.OnGiantStart();

        movementComponent.characterController.excludeLayers += LayerMask.GetMask("Enemy");

        GameObject UIEffect = Instantiate(m_UIEffect_GiantStarted);
        UIEffect.transform.SetParent(FindAnyObjectByType<Canvas>().transform);
        (UIEffect.transform as RectTransform).anchoredPosition = Vector2.zero;
    }

    public void OnFinishGiant()
    {
        // 레벨업 이벤트 재개
        _AbleToLevelUp = true;

        // 점프 애니메이션 이벤트 언바인딩
        animController.onLand -= attackComponent.AttackAround;
        animController.onLand -= InstantiateLandEffect;
        animController.onLand -= FollowCamera.ShakeCamera;

        // 면역상태 원상복구
        movementComponent.SetImmuneState(false);

        // 모델 원상복구
        //modelComponent.OnLevelUp(_LevelSystem.level);

        // 탄창 원상복구
        attackComponent.OnLevelUp(_LevelSystem.level);
        attackComponent.OnGiantFinish();

        // **임시** 거대화 끝나고 호출
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