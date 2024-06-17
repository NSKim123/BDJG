using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 두 개의 버튼이 존재하는 Panel UI 컴포넌트입니다.
/// </summary>
public class PanelWithTwoButtonsUI : MonoBehaviour
{
    [Header("# 버튼 1")]
    public Button m_Button1;

    [Header("# 버튼 2")]
    public Button m_Button2;

    protected virtual void Awake()
    {
        // 버튼 이벤트를 바인딩합니다.
        BindButtonsEvents();
    }

    /// <summary>
    /// 버튼 이벤트를 바인딩하는 메서드입니다.
    /// </summary>
    protected virtual void BindButtonsEvents() { }

    /// <summary>
    /// 첫 번째 버튼의 클릭 이벤트에 함수를 바인딩하는 메서드입니다.
    /// </summary>
    /// <param name="addEvent"> 이벤트에 바인딩할 함수</param>
    public virtual void BindButton1Events(UnityEngine.Events.UnityAction addEvent)
    {
        m_Button1.onClick.AddListener(addEvent);
    }

    /// <summary>
    /// 두 번째 버튼의 클릭 이벤트에 함수를 바인딩하는 메서드입니다.
    /// </summary>
    /// <param name="addEvent"> 이벤트에 바인딩할 함수</param>
    public virtual void BindButton2Events(UnityEngine.Events.UnityAction addEvent)
    {
        m_Button2.onClick.AddListener(addEvent);
    }

    /// <summary>
    /// 첫 번째 버튼의 클릭 이벤트에 함수를 언바인딩하는 메서드입니다.
    /// </summary>
    /// <param name="addEvent"> 언바인딩할 함수</param>
    public virtual void UnbindButton1Events(UnityEngine.Events.UnityAction addEvent)
    {
        m_Button1.onClick.RemoveListener(addEvent);
    }

    /// <summary>
    /// 두 번째 버튼의 클릭 이벤트에 함수를 언바인딩하는 메서드입니다.
    /// </summary>
    /// <param name="addEvent"> 언바인딩할 함수</param>
    public virtual void UnbindButton2Events(UnityEngine.Events.UnityAction addEvent)
    {
        m_Button2.onClick.RemoveListener(addEvent);
    }
}
