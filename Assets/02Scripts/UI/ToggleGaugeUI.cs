using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 온 / 오프 상태에 따라 백그라운드 이미지의 색상이 바뀌는 게이지 UI 컴포넌트입니다.
/// </summary>
public class ToggleGaugeUI : GaugeUI
{
    [Header("# 백그라운드 이미지")]
    public Image m_BackgroundImage;

    [Header("# 온오프 시 색상 관련")]
    [Header("온")]
    public Color m_Color_On = Color.green;

    [Header("오프")]
    public Color m_Color_Off = Color.grey;

    /// <summary>
    /// 온 / 오프를 설정하여 백그라운드 이미지 색상을 설정하는 메서드입니다.
    /// </summary>
    /// <param name="toggleSwitch"> 온 / 오프 값</param>
    public virtual void OnToggleChanged(bool toggleSwitch)
    {
        m_BackgroundImage.color = toggleSwitch ?  m_Color_On : m_Color_Off;
    }
}
