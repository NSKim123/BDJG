using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �� ���� ��ư�� �����ϴ� Panel UI ������Ʈ�Դϴ�.
/// </summary>
public class PanelWithTwoButtonsUI : MonoBehaviour
{
    [Header("# ��ư 1")]
    public Button m_Button1;

    [Header("# ��ư 2")]
    public Button m_Button2;

    protected virtual void Awake()
    {
        // ��ư �̺�Ʈ�� ���ε��մϴ�.
        BindButtonsEvents();
    }

    /// <summary>
    /// ��ư �̺�Ʈ�� ���ε��ϴ� �޼����Դϴ�.
    /// </summary>
    protected virtual void BindButtonsEvents() { }

    /// <summary>
    /// ù ��° ��ư�� Ŭ�� �̺�Ʈ�� �Լ��� ���ε��ϴ� �޼����Դϴ�.
    /// </summary>
    /// <param name="addEvent"> �̺�Ʈ�� ���ε��� �Լ�</param>
    public virtual void BindButton1Events(UnityEngine.Events.UnityAction addEvent)
    {
        m_Button1.onClick.AddListener(addEvent);
    }

    /// <summary>
    /// �� ��° ��ư�� Ŭ�� �̺�Ʈ�� �Լ��� ���ε��ϴ� �޼����Դϴ�.
    /// </summary>
    /// <param name="addEvent"> �̺�Ʈ�� ���ε��� �Լ�</param>
    public virtual void BindButton2Events(UnityEngine.Events.UnityAction addEvent)
    {
        m_Button2.onClick.AddListener(addEvent);
    }

    /// <summary>
    /// ù ��° ��ư�� Ŭ�� �̺�Ʈ�� �Լ��� ����ε��ϴ� �޼����Դϴ�.
    /// </summary>
    /// <param name="addEvent"> ����ε��� �Լ�</param>
    public virtual void UnbindButton1Events(UnityEngine.Events.UnityAction addEvent)
    {
        m_Button1.onClick.RemoveListener(addEvent);
    }

    /// <summary>
    /// �� ��° ��ư�� Ŭ�� �̺�Ʈ�� �Լ��� ����ε��ϴ� �޼����Դϴ�.
    /// </summary>
    /// <param name="addEvent"> ����ε��� �Լ�</param>
    public virtual void UnbindButton2Events(UnityEngine.Events.UnityAction addEvent)
    {
        m_Button2.onClick.RemoveListener(addEvent);
    }
}
