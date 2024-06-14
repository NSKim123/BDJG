using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// 게임 알고리즘 실행하고, 게임 씬에 존재하는 객체들을 연결하는 오브젝트의 컴포넌트입니다.
/// </summary>
public class GameSceneInstance : SceneInstanceBase
{
    [Header("# GameScene UI")]
    public GameSceneUI m_GameSceneUI;

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

    // add 몬스터 스포너 객체

    // add 맵 관리 객체

    /// <summary>
    /// 이 씬에 생성된 플레이어 컨트롤러 객체
    /// </summary>
    public new GameScenePlayerController playerController => base.playerController as GameScenePlayerController;

    protected override void Awake()
    {
        base.Awake();
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
    /// 게임 알고리즘을 시작하는 메서드입니다.
    /// </summary>
    private void StartGame()
    {
        // 게임을 일시정지합니다.
        PauseGame();

        // 게임 정보 초기화
        InitGameInfo();

        // 플레이어 캐릭터 초기화 및 위치 조정

        // 적 스폰 초기화 + (재시작한다고 했을 때, 적 객체 모두 없애야함)

        // 맵 초기화

        // 3초 카운트 ( 코루틴 )
        StartCoroutine(Count3sBeforeGameStart());

        // 입력 권한 부여
        SetUpControl(true); 
        
        // 게임을 재개합니다.
        ContinueGame();
    }   

    private IEnumerator Count3sBeforeGameStart()
    {
        // Ready? UI 활성화
        m_GameSceneUI.m_PanelBeforeGame.SetActive(true);
        m_GameSceneUI.m_PanelBeforeGame.GetComponentInChildren<TMP_Text>().text = "Ready?";

        // 3초 대기 코루틴
        yield return new WaitForSeconds(3.0f);

        // Start로 바꾸고
        m_GameSceneUI.m_PanelBeforeGame.GetComponentInChildren<TMP_Text>().text = "Start!";

        // 1초 대기 코루틴
        yield return new WaitForSeconds(1.0f);

        m_GameSceneUI.m_PanelBeforeGame.SetActive(false);
    }

    private void SetUpControl(bool ableToControl)
    {
        if(ableToControl)
        {
            m_GameSceneUI.m_AttackButtonUI.BindClickEvent(playerController.OnAttack);
            m_GameSceneUI.m_JumpButtonUI.BindClickEvent(playerController.OnJump);
            m_GameSceneUI.m_Joystick_Move.onDrag += playerController.OnMove;
        }
        else
        {
            m_GameSceneUI.m_AttackButtonUI.UnbindClickEvent(playerController.OnAttack);
            m_GameSceneUI.m_JumpButtonUI.UnbindClickEvent(playerController.OnJump);
            m_GameSceneUI.m_Joystick_Move.onDrag -= playerController.OnMove;
        }
        
    }

    private void InitGameInfo()
    {
        AddSurvivalTime(-_SurviveTime);
        AddScore(-_Score);
    }

    private void BindEvents()
    {
        BindUIEvents();

        playerController.controlledCharacter.onDead += OnGameOver;

        playerController.controlledCharacter.attackComponent.bulletGauge.onOverburden += () => m_GameSceneUI.m_BulletGaugeUI.OnToggleChanged(false);
        playerController.controlledCharacter.attackComponent.bulletGauge.onOverburdenFinished += () => m_GameSceneUI.m_BulletGaugeUI.OnToggleChanged(true);
    }

    private void BindUIEvents()
    {        
        // m_GameSceneUI 내부에서 바인드 (UI들끼리의 상호작용)
        m_GameSceneUI.BindUIEvents();

        // ---------------- m_GameSceneUI 외부에서 바인드하는 함수. (게임 씬에 있는 객체들과 연결) ----------------    
        m_GameSceneUI.m_Button_Configuration.onClick.AddListener(PauseGame);
        m_GameSceneUI.m_ConfigurationUI.m_Button_Cancel.onClick.AddListener(ContinueGame);
        m_GameSceneUI.m_GameOverUI.BindButton1Events(StartGame);
        // --------------------------------------------------------------------------------------------------------
    }

    private void UpdateSurvivalTime()
    {
        if (!_IsPaused)
        {
            AddSurvivalTime(Time.deltaTime * Time.timeScale);
        }
    }

    public void AddSurvivalTime(float change)
    {
        _SurviveTime += change;
        m_GameSceneUI.UpdateSurvivalTime(_SurviveTime);
        playerController.controlledCharacter.UpdateSurvivalTime(_SurviveTime);
    }

    private void UpdatePlayerUI()
    {
        m_GameSceneUI.UpdatePlayerUI(playerController.controlledCharacter);
    }

    private void OnGameOver()
    {
        PauseGame();

        m_GameSceneUI.m_GameOverUI.gameObject.SetActive(true);
        m_GameSceneUI.m_GameOverUI.OnGameOver(_SurviveTime, _Score);
    }
    
    public void AddScore(int change)
    {
        _Score = change;
        m_GameSceneUI.m_Text_Score.text = _Score.ToString();
    }

    public void ContinueGame()
    {
        _IsPaused = false;
        Time.timeScale = 1.0f;

        // 적 스폰 재개 및 스폰되어있는 적들 행동 재개
    }

    public void PauseGame()
    {
        _IsPaused = true;
        Time.timeScale = 0.0f;        

        // 적 스폰 종료 및 스폰되어있는 적들 일시정지
    }
}
