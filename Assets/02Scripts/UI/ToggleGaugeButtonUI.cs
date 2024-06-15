using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ToggleGaugeUI 에 버튼이 추가된 UI 컴포넌트입니다.
/// </summary>
public class ToggleGaugeButtonUI : ToggleGaugeUI
{
    [Header("# 버튼")]
    public Button m_Button;

    /// <summary>
    /// 버튼 클릭 이벤트를 바인딩하는 메서드입니다.
    /// </summary>
    /// <param name="bindFunction"> 이벤트에 바인드할 함수</param>
    public void BindClickEvent(UnityEngine.Events.UnityAction bindFunction)
    {
        m_Button.onClick.AddListener(bindFunction);
    }

    /// <summary>
    /// 버튼 클릭 이벤트를 언바인딩하는 메서드입니다.
    /// </summary>
    /// <param name="unbindFunction"> 언바인드할 함수</param>
    public void UnbindClickEvent(UnityEngine.Events.UnityAction unbindFunction)
    {
        m_Button.onClick.RemoveListener(unbindFunction);
    }

    /// <summary>
    /// 온 / 오프를 설정하여 백그라운드 이미지 색상을 설정하고 버튼을 비활성화하는 메서드입니다.
    /// </summary>
    /// <param name="toggleSwitch"> 온 / 오프 값</param>
    public override void OnToggleChanged(bool toggleSwitch)
    {
        // 색 설정
        base.OnToggleChanged(toggleSwitch);

        // 버튼 비활성화
        m_Button.interactable = toggleSwitch;
    }
}
