using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ToggleGaugeUI �� ��ư�� �߰��� UI ������Ʈ�Դϴ�.
/// </summary>
public class ToggleGaugeButtonUI : ToggleGaugeUI
{
    [Header("# ��ư")]
    public Button m_Button;

    /// <summary>
    /// ��ư Ŭ�� �̺�Ʈ�� ���ε��ϴ� �޼����Դϴ�.
    /// </summary>
    /// <param name="bindFunction"> �̺�Ʈ�� ���ε��� �Լ�</param>
    public void BindClickEvent(UnityEngine.Events.UnityAction bindFunction)
    {
        m_Button.onClick.AddListener(bindFunction);
    }

    /// <summary>
    /// ��ư Ŭ�� �̺�Ʈ�� ����ε��ϴ� �޼����Դϴ�.
    /// </summary>
    /// <param name="unbindFunction"> ����ε��� �Լ�</param>
    public void UnbindClickEvent(UnityEngine.Events.UnityAction unbindFunction)
    {
        m_Button.onClick.RemoveListener(unbindFunction);
    }

    /// <summary>
    /// �� / ������ �����Ͽ� ��׶��� �̹��� ������ �����ϰ� ��ư�� ��Ȱ��ȭ�ϴ� �޼����Դϴ�.
    /// </summary>
    /// <param name="toggleSwitch"> �� / ���� ��</param>
    public override void OnToggleChanged(bool toggleSwitch)
    {
        // �� ����
        base.OnToggleChanged(toggleSwitch);

        // ��ư ��Ȱ��ȭ
        m_Button.interactable = toggleSwitch;
    }
}
