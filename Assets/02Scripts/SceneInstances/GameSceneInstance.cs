using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
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
    private EnemySpawner _EnemySpawner;

    // add �� ���� ��ü
    private MapManager _MapManager;

    // item spawn ��ü
    private ItemSpawner _ItemSpawner;

    /// <summary>
    /// �� ���� ������ �÷��̾� ��Ʈ�ѷ� ��ü
    /// </summary>
    public new GameScenePlayerController playerController => base.playerController as GameScenePlayerController;

    protected override void Awake()
    {
        base.Awake();

        _EnemySpawner = FindAnyObjectByType<EnemySpawner>();
        _EnemySpawner.onEnemyDead += AddScore;

        _MapManager = FindAnyObjectByType<MapManager>();

        _ItemSpawner = FindAnyObjectByType<ItemSpawner>();
    }

    private void Start()
    {
        // �̺�Ʈ �Լ��� ���ε��մϴ�.
        BindEvents();

        // ������ �����մϴ�.
        StartCoroutine(GameStartProcess());
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
    private IEnumerator GameStartProcess()
    {
        // ������ �Ͻ������մϴ�.
        PauseGame();

        // ���� ���� �ʱ�ȭ
        InitGameInfo();

        // �÷��̾� ĳ���� �ʱ�ȭ �� ��ġ ����

        // �� ���� �ʱ�ȭ + (������Ѵٰ� ���� ��, �� ��ü ��� ���־���)
        _EnemySpawner.ResetForRestart();

        // �� �ʱ�ȭ
        _MapManager.RestartMap((WaveName)playerController.controlledCharacter.levelSystem.level);

        // 3�� ī��Ʈ ( �ڷ�ƾ )        
        yield return Count3sBeforeGameStart();

        // �Է� ���� �ο�
        SetUpControl(true); 
        
        // ������ �簳�մϴ�.
        ContinueGame();
    }       

    /// <summary>
    /// ���� �簳 �� 3�ʸ� ī��Ʈ�ϴ� �ڷ�ƾ�Դϴ�.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Count3sBeforeGameStart()
    {
        // ���� ���� �� ���� UI�� Ȱ��ȭ�մϴ�.
        m_GameSceneUI.m_PanelBeforeGame.SetActive(true);
        m_GameSceneUI.m_PanelBeforeGame.GetComponentInChildren<TMP_Text>().text = "Ready?";

        // 3�� ��� �ڷ�ƾ
        yield return new WaitForSecondsRealtime(3.0f);

        // UI �� Start�� ���ϴ�.
        m_GameSceneUI.m_PanelBeforeGame.GetComponentInChildren<TMP_Text>().text = "Start!";

        // 1�� ��� �ڷ�ƾ
        yield return new WaitForSecondsRealtime(1.0f);

        // ���� ���� �� ���� UI�� ��Ȱ��ȭ�մϴ�.
        m_GameSceneUI.m_PanelBeforeGame.SetActive(false);
    }

    /// <summary>
    /// �÷��̾�� ���� ������ �ο������� �����ϴ� �޼����Դϴ�.
    /// </summary>
    /// <param name="ableToControl"> �Է� ���� �ο�</param>
    private void SetUpControl(bool ableToControl)
    {
        if(ableToControl)
        {
            // ���� ��ư �̺�Ʈ ���ε�
            m_GameSceneUI.m_AttackButtonUI.BindClickEvent(playerController.OnAttack);
            // ���� ��ư �̺�Ʈ ���ε�
            m_GameSceneUI.m_JumpButtonUI.BindClickEvent(playerController.OnJump);            
            // �̵� ��ư �̺�Ʈ ���ε�
            m_GameSceneUI.m_Joystick_Move.BindDragEvent(playerController.OnMove);
        }
        else
        {
            // ���� ��ư �̺�Ʈ ����ε�
            m_GameSceneUI.m_AttackButtonUI.UnbindClickEvent(playerController.OnAttack);
            // ���� ��ư �̺�Ʈ ����ε�
            m_GameSceneUI.m_JumpButtonUI.UnbindClickEvent(playerController.OnJump);
            // �̵� ��ư �̺�Ʈ ����ε�
            m_GameSceneUI.m_Joystick_Move.UnbindDragEvent(playerController.OnMove);
        }
        
    }

    /// <summary>
    /// ���� ������ �ʱ�ȭ�ϴ� �޼����Դϴ�.
    /// </summary>
    private void InitGameInfo()
    {
        // ���� �ð� �ʱ�ȭ
        AddSurvivalTime(-_SurviveTime);

        // ���� �ʱ�ȭ
        AddScore(-_Score);
    }

    /// <summary>
    /// �ʿ��� ���ε��� �����ϴ� �޼����Դϴ�.
    /// </summary>
    private void BindEvents()
    {
        // UI ��ü���� �븮�ڿ� �Լ����� ���ε��ŵ�ϴ�.
        BindUIEvents();

        // �÷��̾� ��ü�� �븮�ڿ� �Լ����� ���ε��ŵ�ϴ�.
        BindPlayerEvents();
    }

    /// <summary>
    /// UI ��ü���� �븮�ڿ� �Լ����� ���ε��Ű�� �޼����Դϴ�.
    /// ���� ���� �����ϴ� ��ü��� UI�� �����ŵ�ϴ�.
    /// </summary>
    private void BindUIEvents()
    {
        // ȯ�漳�� ��ư Ŭ�� �̺�Ʈ <-- ���ε� -- ���� �Ͻ� ���� �Լ�
        m_GameSceneUI.m_Button_Configuration.onClick.AddListener(PauseGame);

        // ȯ�漳�� UI ��ҹ�ư Ŭ�� �̺�Ʈ <-- ���ε� -- ���� �簳 �Լ�
        m_GameSceneUI.m_ConfigurationUI.m_Button_Cancel.onClick.AddListener(ContinueGame);

        // ���ӿ��� UI �ٽ��ϱ� ��ư Ŭ�� �̺�Ʈ <-- ���ε� -- ���� ���� �Լ�
        m_GameSceneUI.m_GameOverUI.BindButton1Events(() => StartCoroutine(GameStartProcess()));       
    }

    /// <summary>
    /// �÷��̾� ��ü�� �븮�ڿ� �Լ����� ���ε��Ű�� �޼����Դϴ�.
    /// </summary>
    private void BindPlayerEvents()
    {
        // ���� �ý��� UI�� ǥ���� ���� �ý����� �÷��̾��� ���� �ý������� �����ϰ�, 
        // ���� �ý��� �̺�Ʈ�鿡 ���� �ý��� UI�� �Լ����� ���ε��մϴ�.
        m_GameSceneUI.m_BuffSystemUI.SetTargetBuffSystem(playerController.controlledCharacter.buffSystem);

        // �׾��� �� ����Ǵ� �̺�Ʈ�� ���ε��մϴ�.
        playerController.controlledCharacter.onDead += OnGameOver;

        // ������ ���� �� ���� �� ����Ǵ� �̺�Ʈ�� �Լ��� ���ε��մϴ�.
        playerController.controlledCharacter.attackComponent.bulletGauge.onOverburdenEnter += () => m_GameSceneUI.m_BulletGaugeUI.OnToggleChanged(false);
        playerController.controlledCharacter.attackComponent.bulletGauge.onOverburdenFinished += () => m_GameSceneUI.m_BulletGaugeUI.OnToggleChanged(true);

        //������ �̺�Ʈ�� �� ������ �� ���� �޼��带 ���ε��մϴ�.
        playerController.controlledCharacter.levelSystem.onLevelUp += _EnemySpawner.ResetForLevelUp;
        playerController.controlledCharacter.levelSystem.onLevelUp += _MapManager.SetWaterHeightByLevel;

        // ������Ÿ�Կ����� �ӽ÷� ���ε��� �޼����Դϴ�. �������� ���������� �����մϴ�.
        playerController.controlledCharacter.levelSystem.onLevelUp += _ItemSpawner.ItemSpawn_proto;

    }

    /// <summary>
    /// ���� �ð��� �����ϴ� �޼����Դϴ�.
    /// </summary>
    private void UpdateSurvivalTime()
    {
        // �Ͻ������� ���°� �ƴ϶��
        if (!_IsPaused)
        {
            // ���� �ð��� ���մϴ�.
            AddSurvivalTime(Time.deltaTime * Time.timeScale);
        }
    }

    /// <summary>
    /// ���� �ð��� ��ȭ���� ���ϴ� �޼����Դϴ�.
    /// </summary>
    /// <param name="change"> ��ȭ��( ������ ������ �� ���� �ð��� �����մϴ�. )</param>
    public void AddSurvivalTime(float change)
    {
        // ���� �ð��� ��ȭ���� ���մϴ�.
        _SurviveTime += change;

        // UI �����մϴ�.
        m_GameSceneUI.UpdateSurvivalTimeText(_SurviveTime);

        // �÷��̾� ��ü�� ���� �ð��� ���Ž����ݴϴ�.
        playerController.controlledCharacter.UpdateSurvivalTime(_SurviveTime);
    }

    /// <summary>
    /// PUI�� �����ϴ� �޼����Դϴ�.
    /// </summary>
    private void UpdatePlayerUI()
    {
        m_GameSceneUI.UpdatePlayerUI(playerController.controlledCharacter);
    }

    /// <summary>
    /// ���� ���� �� ȣ��� �޼����Դϴ�.
    /// </summary>
    private void OnGameOver()
    {
        // ������ �Ͻ� ���� ��ŵ�ϴ�.
        PauseGame();

        // ���� ���� UI�� Ȱ��ȭ��Ű��, UI�� ���� ������ �����մϴ�.
        m_GameSceneUI.m_GameOverUI.gameObject.SetActive(true);
        m_GameSceneUI.m_GameOverUI.OnGameOver(_SurviveTime, _Score);
    }

    /// <summary>
    /// ������ ��ȭ���� ���ϴ� �޼����Դϴ�.
    /// </summary>
    /// <param name="change"> ��ȭ��( ������ ������ �� ������ �����մϴ�. )</param>
    public void AddScore(int change)
    {
        // ������ ��ȭ���� ���մϴ�.
        _Score += change;

        // UI�� �����մϴ�.
        m_GameSceneUI.m_Text_Score.text = _Score.ToString();
    }

    /// <summary>
    /// ������ �簳�ϴ� �޼����Դϴ�.
    /// </summary>
    public void ContinueGame()
    {
        // �ð� �������� �ǵ����ϴ�.
        _IsPaused = false;
        Time.timeScale = 1.0f;

        // �� ���� �簳 �� �����Ǿ��ִ� ���� �ൿ �簳
    }

    /// <summary>
    /// ������ �Ͻ������ϴ� �޼����Դϴ�.
    /// </summary>
    public void PauseGame()
    {
        // �ð� �������� 0 ���� �����մϴ�.
        _IsPaused = true;
        Time.timeScale = 0.0f;        

        // �� ���� ���� �� �����Ǿ��ִ� ���� �Ͻ�����
    }
}
