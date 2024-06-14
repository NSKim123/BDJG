using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneUI : MonoBehaviour
{
    [Header("# 생존 시간 텍스트")]
    public TMP_Text m_Text_SurvivalTime;

    [Header("# 점수 텍스트")]
    public TMP_Text m_Text_Score;

    // TO DO : 추후 버프 시스템 개발 후 작업
    //[Header("# 버프 시스템 UI")]

    [Header("# 탄창 게이지 UI")]
    public BulletGaugeUI m_BulletGaugeUI;

    [Header("# 환경 설정 버튼")]
    public Button m_Button_Configuration;

    [Header("# 환경 설정 UI")]
    public ConfigurationUI m_ConfigurationUI;

    [Header("# 이동 버튼")]
    public JoystickUI m_Joystick_Move;

    [Header("# 공격 버튼 UI")]
    public ToggleGaugeButtonUI m_AttackButtonUI;

    [Header("# 점프 버튼 UI")]
    public ToggleGaugeButtonUI m_JumpButtonUI;

    [Header("# 게임 오버 UI")]
    public GameOverUI m_GameOverUI;

    [Header("# 게임 시작 전 띄울 UI")]
    public GameObject m_PanelBeforeGame;
    
    public void BindUIEvents()
    {
        m_Button_Configuration.onClick.AddListener(() => m_ConfigurationUI.gameObject.SetActive(true));

        m_ConfigurationUI.BindUIEvents();

        // TO DO : 메인 화면 이동하는 함수 바인딩 해야함 
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
