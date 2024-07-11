using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 게임 씬에서 사용되는 모든 UI를 관리하는 컴포넌트입니다.
/// </summary>
public class GameSceneUI : MonoBehaviour
{
    [Header("# 생존 시간 텍스트")]
    public TMP_Text m_Text_SurvivalTime;

    [Header("# 점수 텍스트")]
    public TMP_Text m_Text_Score;

    [Header("# 버프 시스템 UI")]
    public BuffSystemUI m_BuffSystemUI;

    [Header("# 탄창 게이지 UI")]
    public BulletGaugeUI m_BulletGaugeUI;

    [Header("# 아이템 슬롯 UI")]
    public ItemSlotsUI m_ItemSlotsUI;

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

    [Header("# 수위 상승 전 경고 UI")]
    public GameObject m_WarningUI;

    private void Awake()
    {
        // UI들의 이벤트를 바인딩합니다.
        BindUIEvents();
    }

    /// <summary>
    /// UI들의 이벤트를 바인딩하는 메서드입니다.
    /// </summary>
    private void BindUIEvents()
    {
        // 환경 설정 버튼 클릭 이벤트 <-- 바인딩 -- 환경설정 창 활성화 함수
        m_Button_Configuration.onClick.AddListener(() => m_ConfigurationUI.gameObject.SetActive(true));
        m_Button_Configuration.onClick.AddListener(() => SoundManager.Instance.PlaySound(Constants.SOUNDNAME_CLICK_ABLEBUTTON, SoundType.Effect));
    }

    /// <summary>
    /// 생존 시간 텍스트 UI를 갱신하는 메서드입니다.
    /// </summary>
    /// <param name="newTime"> 갱신할 시간</param>
    public void UpdateSurvivalTimeText(float newTime)
    {
        int minute = (int)(newTime / 60.0f);
        int second = (int)(newTime - (int)(newTime / 60.0f) * 60.0f);

        m_Text_SurvivalTime.text = $"{minute} : " + (second < 10? "0" : "") + second;
    }

    /// <summary>
    /// 플레이어 캐릭터 객체의 정보를 토대로 UI를 갱신하는 메서드입니다.
    /// </summary>
    /// <param name="playerCharacter"> 컨트롤 중인 플레이어 캐릭터 객체</param>
    public void UpdatePlayerUI(PlayerCharacter playerCharacter)
    {
        // 탄창 게이지 UI 갱신
        m_BulletGaugeUI.SetRatio(playerCharacter.attackComponent.bulletGauge.ratio);

        // 공격 버튼 갱신
        m_AttackButtonUI.SetRatio(playerCharacter.attackComponent.reuseTimeGauge.ratio);
        m_AttackButtonUI.OnToggleChanged(playerCharacter.attackComponent.isAttacktable);

        // 점프 버튼 갱신
        m_JumpButtonUI.SetRatio(playerCharacter.movementComponent.jumpResueTimeGauge.ratio);
        m_JumpButtonUI.OnToggleChanged(playerCharacter.movementComponent.isJumpable);

        m_ItemSlotsUI.OnToggleChanged(playerCharacter.isAbleToUseItem);
    }
}
