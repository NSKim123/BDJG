using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;

/// <summary>
/// 게임 알고리즘 실행하고, 게임 씬에 존재하는 객체들을 연결하는 오브젝트의 컴포넌트입니다.
/// </summary>
public class GameSceneInstance : SceneInstanceBase
{
    [Header("# GameScene UI")]
    public GameSceneUI m_GameSceneUI;

    [Header("# 플레이어 스폰 위치")]
    public Transform m_PlayerSpawnPosition;

    /// <summary>
    /// 생존 시간
    /// </summary>
    private float _SurviveTime;

    /// <summary>
    /// 점수
    /// </summary>
    private int _Score;

    /// <summary>
    /// 일시 중지되었는지
    /// </summary>
    private bool _IsPaused = true;

    /// <summary>
    /// 몬스터 스포너 객체
    /// </summary>
    private EnemySpawner _EnemySpawner;

    /// <summary>
    /// 맵 관리 객체
    /// </summary>
    private MapController _MapController;

    /// <summary>
    /// 아이템 스포너 객체
    /// </summary>
    private ItemSpawner _ItemSpawner;

    /// <summary>
    /// 이 씬에 생성된 플레이어 컨트롤러 객체
    /// </summary>
    public new GameScenePlayerController playerController => base.playerController as GameScenePlayerController;

    protected override void Awake()
    {
        base.Awake();

        _EnemySpawner = FindAnyObjectByType<EnemySpawner>();

        _MapController = FindAnyObjectByType<MapController>();
        _MapController.Warning = m_GameSceneUI.m_WarningUI;

        _ItemSpawner = FindAnyObjectByType<ItemSpawner>();
    }

    private void Start()
    {
        // 이벤트 함수를 바인딩합니다.
        BindEvents();

        // 게임을 시작합니다.
        StartGame();
    }


    private void Update()
    {
        // 생존 시간을 갱신합니다.
        UpdateSurvivalTime();

        // PUI를 갱신합니다.
        UpdatePlayerUI();
    }

    /// <summary>
    /// 게임 시작 알고리즘 코루틴입니다.
    /// </summary>
    private IEnumerator GameStartProcess()
    {
        // 게임을 일시정지합니다.
        PauseGame();

        // 게임 정보 초기화
        InitGameInfo();

        // 플레이어 캐릭터 초기화 및 위치 조정
        ResetPlayerCharacter();

        // 오브젝트 풀 초기화
        ObjectPoolManager.Instance.ResetObjectPools();

        // 아이템 남아있다면 없애서 초기화
        _ItemSpawner.ResetItems();

        // 특수공격 남아있다면 없애서 초기화
        EnemyManager.Instance.ResetSpecialAttack();

        // 적 스폰 초기화
        _EnemySpawner.InitEnemySpawnSetting();

        // 맵 초기화 (위치 원래대로)
        _MapController.ResetMapSetting();
        
        // 3초 카운트 ( 코루틴 )        
        yield return Count3sBeforeGameStart();

        // 조작 권한 부여
        SetUpControl(true);

        // 배경음악 플레이
        SoundManager.Instance.PlaySound("bgm", SoundType.Bgm, 1.0f, 0.3f);

        // 게임을 재개합니다.
        ContinueGame();
    }

    /// <summary>
    /// 게임을 시작하는 메서드입니다.
    /// </summary>
    private void StartGame()
    {
        StartCoroutine(GameStartProcess());
    }

    /// <summary>
    /// 플레이어 캐릭터를 초기화하는 메서드입니다.
    /// </summary>
    private void ResetPlayerCharacter()
    {
        playerController.controlledCharacter.ResetPlayerCharacter();
        playerController.controlledCharacter.movementComponent.ResetMovementComponent(m_PlayerSpawnPosition.transform.position);
    }

    /// <summary>
    /// 게임 재개 전 3초를 카운트하는 코루틴입니다.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Count3sBeforeGameStart()
    {
        // 게임 오버 UI를 제거합니다.
        m_GameSceneUI.m_GameOverUI.gameObject.SetActive(false);

        // 경고UI를 활성화합니다.
        ToggleActiveWarningUI();

        // 게임 시작 전 띄우는 UI를 활성화합니다.
        m_GameSceneUI.m_PanelBeforeGame.SetActive(true);

        // 준비를 알리는 text를 띄웁니다.
        m_GameSceneUI.m_PanelBeforeGame.GetComponentInChildren<TMP_Text>().text = "준비";

        // 3초 대기 코루틴
        yield return new WaitForSecondsRealtime(3.0f);

        // 시작을 알리는 text를 띄웁니다.
        m_GameSceneUI.m_PanelBeforeGame.GetComponentInChildren<TMP_Text>().text = "시작!";

        // 1초 대기 코루틴
        yield return new WaitForSecondsRealtime(1.0f);

        // 게임 시작 전 띄우는 UI를 비활성화합니다.
        m_GameSceneUI.m_PanelBeforeGame.SetActive(false);
    }

    /// <summary>
    /// 플레이어에게 조작 권한을 부여할지를 설정하는 메서드입니다.
    /// </summary>
    /// <param name="ableToControl"> 입력 권한 부여</param>
    private void SetUpControl(bool ableToControl)
    {
        if(ableToControl)
        {
            // 공격 버튼 이벤트 바인드
            m_GameSceneUI.m_AttackButtonUI.BindClickEvent(playerController.OnAttack);
            // 점프 버튼 이벤트 바인드
            m_GameSceneUI.m_JumpButtonUI.BindClickEvent(playerController.OnJump);            
            // 이동 버튼 이벤트 바인드
            m_GameSceneUI.m_Joystick_Move.BindDragEvent(playerController.OnMove);
        }
        else
        {
            // 공격 버튼 이벤트 언바인드
            m_GameSceneUI.m_AttackButtonUI.UnbindClickEvent(playerController.OnAttack);
            // 점프 버튼 이벤트 언바인드
            m_GameSceneUI.m_JumpButtonUI.UnbindClickEvent(playerController.OnJump);
            // 이동 버튼 이벤트 언바인드
            m_GameSceneUI.m_Joystick_Move.UnbindDragEvent(playerController.OnMove);
        }
        
    }

    /// <summary>
    /// 게임 정보를 초기화하는 메서드입니다.
    /// </summary>
    private void InitGameInfo()
    {
        // 생존 시간 초기화
        AddSurvivalTime(-_SurviveTime);

        // 점수 초기화
        AddScore(-_Score);
    }

    /// <summary>
    /// 필요한 바인딩을 실행하는 메서드입니다.
    /// </summary>
    private void BindEvents()
    {
        // UI 객체들의 대리자에 함수들을 바인드시킵니다.
        BindUIEvents();

        // 플레이어 객체의 대리자에 함수들을 바인드시킵니다.
        BindPlayerEvents();

        // 적 객체의 대리자에 함수들을 바인드시킵니다.
        BindEnemyEvents();
    }

    /// <summary>
    /// UI 객체들의 대리자에 함수들을 바인드시키는 메서드입니다.
    /// 게임 씬에 존재하는 객체들과 UI를 연결시킵니다.
    /// </summary>
    private void BindUIEvents()
    {
        // 환경설정 버튼 클릭 이벤트 <-- 바인드 -- 게임 일시 정지 함수
        m_GameSceneUI.m_Button_Configuration.onClick.AddListener(PauseGame);

        // 환경설정 UI 취소버튼 클릭 이벤트 <-- 바인드 -- 게임 재개 함수
        m_GameSceneUI.m_ConfigurationUI.m_Button_Cancel.onClick.AddListener(ContinueGame);

        // 게임오버 UI 다시하기 버튼 클릭 이벤트 <-- 바인드 -- 게임 실행 함수
        m_GameSceneUI.m_GameOverUI.BindButton1Events(StartGame);

        // 아이템 슬롯 UI 클릭 이벤트 <-- 바인드 -- 아이템 사용 함수
        m_GameSceneUI.m_ItemSlotsUI.BindClickEvent(playerController.OnUseItem);
    }

    /// <summary>
    /// 플레이어 객체의 대리자에 함수들을 바인드시키는 메서드입니다.
    /// </summary>
    private void BindPlayerEvents()
    {
        // 버프 시스템 UI가 표시할 버프 시스템을 플레이어의 버프 시스템으로 설정하고, 
        // 버프 시스템 이벤트들에 버프 시스템 UI의 함수들을 바인드합니다.
        m_GameSceneUI.m_BuffSystemUI.SetTargetBuffSystem(playerController.controlledCharacter.buffSystem);

        // 죽었을 때 실행되는 이벤트에 바인드합니다.
        playerController.controlledCharacter.onDead += OnGameOver;

        // 과부하 돌입 및 해제 시 실행되는 이벤트에 함수를 바인드합니다.
        playerController.controlledCharacter.attackComponent.bulletGauge.onOverburdenEnter += () => m_GameSceneUI.m_BulletGaugeUI.OnToggleChanged(false);
        playerController.controlledCharacter.attackComponent.bulletGauge.onOverburdenFinished += () => m_GameSceneUI.m_BulletGaugeUI.OnToggleChanged(true);

        // 아이템 슬롯에 변화가 생길 시 실행되는 이벤트에 함수를 바인드합니다.
        playerController.controlledCharacter.onItemSlotsChanged += m_GameSceneUI.m_ItemSlotsUI.OnItemSlotChanged;
    }

    /// <summary>
    /// 적 객체의 대리자에 함수를 바인드합니다.
    /// </summary>
    private void BindEnemyEvents()
    {
        // 적 객체 사망 이벤트 <-- 바인드 -- 점수 증가 함수
        _EnemySpawner.onEnemyDead += AddScore;

        // 적 객체 사망 이벤트 <-- 바인드 -- 플레이어의 킬 수 증가 함수
        _EnemySpawner.onEnemyDead += playerController.controlledCharacter.levelSystem.IncreaseKillCount;
    }

    /// <summary>
    /// 생존 시간을 갱신하는 메서드입니다.
    /// </summary>
    private void UpdateSurvivalTime()
    {
        // 일시정지된 상태가 아니라면
        if (!_IsPaused)
        {
            // 생존 시간을 더합니다.
            AddSurvivalTime(Time.deltaTime * Time.timeScale);
        }
    }

    /// <summary>
    /// 생존 시간에 변화량을 더하는 메서드입니다.
    /// </summary>
    /// <param name="change"> 변화량( 음수를 전달할 시 생존 시간이 감소합니다. )</param>
    public void AddSurvivalTime(float change)
    {
        // 생존 시간에 변화량을 더합니다.
        _SurviveTime += change;

        // UI 갱신합니다.
        m_GameSceneUI.UpdateSurvivalTimeText(_SurviveTime);

        // 플레이어 객체의 생존 시간을 갱신시켜줍니다.
        playerController.controlledCharacter.UpdateSurvivalTime(_SurviveTime);
    }

    /// <summary>
    /// PUI를 갱신하는 메서드입니다.
    /// </summary>
    private void UpdatePlayerUI()
    {
        m_GameSceneUI.UpdatePlayerUI(playerController.controlledCharacter);
    }

    /// <summary>
    /// 게임 오버 시 호출될 메서드입니다.
    /// </summary>
    private void OnGameOver()
    {
        // 게임을 일시 정지 시킵니다.
        PauseGame();

        // 조작 권한 해제
        SetUpControl(false);

        // 게임 오버 UI를 활성화시키고, UI에 게임 정보를 전달합니다.
        m_GameSceneUI.m_GameOverUI.gameObject.SetActive(true);
        m_GameSceneUI.m_GameOverUI.OnGameOver(_SurviveTime, _Score);

        // 경고 UI를 비활성화합니다.
        ToggleActiveWarningUI();
    }

    /// <summary>
    /// 점수에 변화량을 더하는 메서드입니다.
    /// </summary>
    /// <param name="change"> 변화량( 음수를 전달할 시 점수가 감소합니다. )</param>
    public void AddScore(int change)
    {
        // 점수에 변화량을 더합니다.
        _Score += change;

        // UI를 갱신합니다.
        m_GameSceneUI.m_Text_Score.text = _Score.ToString();
    }

    /// <summary>
    /// 게임을 재개하는 메서드입니다.
    /// </summary>
    public void ContinueGame()
    {
        // 시간 스케일을 되돌립니다.
        _IsPaused = false;
        Time.timeScale = 1.0f;
    }

    /// <summary>
    /// 게임을 일시정지하는 메서드입니다.
    /// </summary>
    public void PauseGame()
    {
        // 시간 스케일을 0 으로 설정합니다.
        _IsPaused = true;
        Time.timeScale = 0.0f;        
    }

    /// <summary>
    /// 경고UI가 활성화상태라면 비활성화하고, 비활성화상태라면 활성화하는 메서드입니다.
    /// </summary>
    private void ToggleActiveWarningUI()
    {
        m_GameSceneUI.m_WarningUI.gameObject.SetActive(m_GameSceneUI.m_WarningUI.gameObject.activeSelf ? false : true);
    }
}
