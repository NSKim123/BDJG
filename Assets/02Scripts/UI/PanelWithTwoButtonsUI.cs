using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelWithTwoButtonsUI : MonoBehaviour
{
    [Header("# 버튼 1")]
    public Button m_Button1;

    [Header("# 버튼 2")]
    public Button m_Button2;

    public virtual void BindButton1Events(UnityEngine.Events.UnityAction addEvent)
    {
        m_Button1.onClick.AddListener(addEvent);
    }

    public virtual void BindButton2Events(UnityEngine.Events.UnityAction addEvent)
    {
        m_Button2.onClick.AddListener(addEvent);
    }
}
