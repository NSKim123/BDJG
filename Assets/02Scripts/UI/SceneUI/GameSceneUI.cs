using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneUI : MonoBehaviour
{
    [Header("# ���� �ð� �ؽ�Ʈ")]
    public TMP_Text m_Text_SurvivalTime;

    [Header("# ���� �ؽ�Ʈ")]
    public TMP_Text m_Text_Score;

    // TO DO : ���� ���� �ý��� ���� �� �۾�
    //[Header("# ���� �ý��� UI")]

    [Header("# źâ ������ UI")]
    public BulletGaugeUI m_BulletGaugeUI;

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
    
    public void BindUIEvents()
    {
        m_Button_Configuration.onClick.AddListener(() => m_ConfigurationUI.gameObject.SetActive(true));

        m_ConfigurationUI.BindUIEvents();

        // TO DO : ���� ȭ�� �̵��ϴ� �Լ� ���ε� �ؾ��� 
        // m_GameOverUI.BindButton2Events();
    }

    public void UpdateSurvivalTime(float newTime)
    {
        int minute = (int)(newTime / 60.0f);
        int second = (int)(newTime - (int)(newTime / 60.0f) * 60.0f);

        m_Text_SurvivalTime.text = $"{minute} : " + (second < 10? "0" : "") + second;
    }

    public void UpdatePlayerUI(PlayerCharacter playerCharacter)
    {
        m_BulletGaugeUI.SetTargetRatio(playerCharacter.attackComponent.bulletGauge.ratio);

        m_AttackButtonUI.SetTargetRatio(playerCharacter.attackComponent.reuseTimeGauge.ratio);
        m_AttackButtonUI.OnToggleChanged(playerCharacter.attackComponent.isAttacktable);

        m_JumpButtonUI.SetTargetRatio(playerCharacter.movementComponent.jumpResueTimeGauge.ratio);
        m_JumpButtonUI.OnToggleChanged(playerCharacter.movementComponent.isJumpable);
    }
}
