using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// ���� �˰��� �����ϰ�, ���� ���� �����ϴ� ��ü���� �����ϴ� ������Ʈ�� ������Ʈ�Դϴ�.
/// </summary>
public class GameSceneInstance : SceneInstanceBase
{
    [Header("# GameScene UI")]
    public GameSceneUI m_GameSceneUI;

    /// <summary>
    /// ���� �ð�
    /// </summary>
    private float _SurviveTime;

    /// <summary>
    /// ����
    /// </summary>
    private int _Score;

    /// <summary>
    /// �Ͻ� �����Ǿ�����
    /// </summary>
    private bool _IsPaused = true;

    // add ���� ������ ��ü

    // add �� ���� ��ü

    /// <summary>
    /// �� ���� ������ �÷��̾� ��Ʈ�ѷ� ��ü
    /// </summary>
    public new GameScenePlayerController playerController => base.playerController as GameScenePlayerController;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        // �̺�Ʈ �Լ��� ���ε��մϴ�.
        BindEvents();

        // ������ �����մϴ�.
        StartGame();
    }

    private void Update()
    {
        // ���� �ð��� �����մϴ�.
        UpdateSurvivalTime();

        // PUI�� �����մϴ�.
        UpdatePlayerUI();
    }

    /// <summary>
    /// ���� �˰����� �����ϴ� �޼����Դϴ�.
    /// </summary>
    private void StartGame()
    {
        // ������ �Ͻ������մϴ�.
        PauseGame();

        // ���� ���� �ʱ�ȭ
        InitGameInfo();

        // �÷��̾� ĳ���� �ʱ�ȭ �� ��ġ ����

        // �� ���� �ʱ�ȭ + (������Ѵٰ� ���� ��, �� ��ü ��� ���־���)

        // �� �ʱ�ȭ

        // 3�� ī��Ʈ ( �ڷ�ƾ )
        StartCoroutine(Count3sBeforeGameStart());

        // �Է� ���� �ο�
        SetUpControl(true); 
        
        // ������ �簳�մϴ�.
        ContinueGame();
    }   

    private IEnumerator Count3sBeforeGameStart()
    {
        // Ready? UI Ȱ��ȭ
        m_GameSceneUI.m_PanelBeforeGame.SetActive(true);
        m_GameSceneUI.m_PanelBeforeGame.GetComponentInChildren<TMP_Text>().text = "Ready?";

        // 3�� ��� �ڷ�ƾ
        yield return new WaitForSeconds(3.0f);

        // Start�� �ٲٰ�
        m_GameSceneUI.m_PanelBeforeGame.GetComponentInChildren<TMP_Text>().text = "Start!";

        // 1�� ��� �ڷ�ƾ
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
        // m_GameSceneUI ���ο��� ���ε� (UI�鳢���� ��ȣ�ۿ�)
        m_GameSceneUI.BindUIEvents();

        // ---------------- m_GameSceneUI �ܺο��� ���ε��ϴ� �Լ�. (���� ���� �ִ� ��ü��� ����) ----------------    
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

        // �� ���� �簳 �� �����Ǿ��ִ� ���� �ൿ �簳
    }

    public void PauseGame()
    {
        _IsPaused = true;
        Time.timeScale = 0.0f;        

        // �� ���� ���� �� �����Ǿ��ִ� ���� �Ͻ�����
    }
}
