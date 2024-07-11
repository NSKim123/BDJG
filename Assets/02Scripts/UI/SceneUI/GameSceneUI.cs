using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ���� ������ ���Ǵ� ��� UI�� �����ϴ� ������Ʈ�Դϴ�.
/// </summary>
public class GameSceneUI : MonoBehaviour
{
    [Header("# ���� �ð� �ؽ�Ʈ")]
    public TMP_Text m_Text_SurvivalTime;

    [Header("# ���� �ؽ�Ʈ")]
    public TMP_Text m_Text_Score;

    [Header("# ���� �ý��� UI")]
    public BuffSystemUI m_BuffSystemUI;

    [Header("# źâ ������ UI")]
    public BulletGaugeUI m_BulletGaugeUI;

    [Header("# ������ ���� UI")]
    public ItemSlotsUI m_ItemSlotsUI;

    [Header("# ȯ�� ���� ��ư")]
    public Button m_Button_Configuration;

    [Header("# ȯ�� ���� UI")]
    public ConfigurationUI m_ConfigurationUI;

    [Header("# �̵� ��ư")]
    public JoystickUI m_Joystick_Move;

    [Header("# ���� ��ư UI")]
    public ToggleGaugeButtonUI m_AttackButtonUI;

    [Header("# ���� ��ư UI")]
    public ToggleGaugeButtonUI m_JumpButtonUI;

    [Header("# ���� ���� UI")]
    public GameOverUI m_GameOverUI;

    [Header("# ���� ���� �� ��� UI")]
    public GameObject m_PanelBeforeGame;

    [Header("# ���� ��� �� ��� UI")]
    public GameObject m_WarningUI;

    private void Awake()
    {
        // UI���� �̺�Ʈ�� ���ε��մϴ�.
        BindUIEvents();
    }

    /// <summary>
    /// UI���� �̺�Ʈ�� ���ε��ϴ� �޼����Դϴ�.
    /// </summary>
    private void BindUIEvents()
    {
        // ȯ�� ���� ��ư Ŭ�� �̺�Ʈ <-- ���ε� -- ȯ�漳�� â Ȱ��ȭ �Լ�
        m_Button_Configuration.onClick.AddListener(() => m_ConfigurationUI.gameObject.SetActive(true));
        m_Button_Configuration.onClick.AddListener(() => SoundManager.Instance.PlaySound(Constants.SOUNDNAME_CLICK_ABLEBUTTON, SoundType.Effect));
    }

    /// <summary>
    /// ���� �ð� �ؽ�Ʈ UI�� �����ϴ� �޼����Դϴ�.
    /// </summary>
    /// <param name="newTime"> ������ �ð�</param>
    public void UpdateSurvivalTimeText(float newTime)
    {
        int minute = (int)(newTime / 60.0f);
        int second = (int)(newTime - (int)(newTime / 60.0f) * 60.0f);

        m_Text_SurvivalTime.text = $"{minute} : " + (second < 10? "0" : "") + second;
    }

    /// <summary>
    /// �÷��̾� ĳ���� ��ü�� ������ ���� UI�� �����ϴ� �޼����Դϴ�.
    /// </summary>
    /// <param name="playerCharacter"> ��Ʈ�� ���� �÷��̾� ĳ���� ��ü</param>
    public void UpdatePlayerUI(PlayerCharacter playerCharacter)
    {
        // źâ ������ UI ����
        m_BulletGaugeUI.SetRatio(playerCharacter.attackComponent.bulletGauge.ratio);

        // ���� ��ư ����
        m_AttackButtonUI.SetRatio(playerCharacter.attackComponent.reuseTimeGauge.ratio);
        m_AttackButtonUI.OnToggleChanged(playerCharacter.attackComponent.isAttacktable);

        // ���� ��ư ����
        m_JumpButtonUI.SetRatio(playerCharacter.movementComponent.jumpResueTimeGauge.ratio);
        m_JumpButtonUI.OnToggleChanged(playerCharacter.movementComponent.isJumpable);

        m_ItemSlotsUI.OnToggleChanged(playerCharacter.isAbleToUseItem);
    }
}
