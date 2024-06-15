using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �� / ���� ���¿� ���� ��׶��� �̹����� ������ �ٲ�� ������ UI ������Ʈ�Դϴ�.
/// </summary>
public class ToggleGaugeUI : GaugeUI
{
    [Header("# ��׶��� �̹���")]
    public Image m_BackgroundImage;

    [Header("# �¿��� �� ���� ����")]
    [Header("��")]
    public Color m_Color_On = Color.green;

    [Header("����")]
    public Color m_Color_Off = Color.grey;

    /// <summary>
    /// �� / ������ �����Ͽ� ��׶��� �̹��� ������ �����ϴ� �޼����Դϴ�.
    /// </summary>
    /// <param name="toggleSwitch"> �� / ���� ��</param>
    public virtual void OnToggleChanged(bool toggleSwitch)
    {
        m_BackgroundImage.color = toggleSwitch ?  m_Color_On : m_Color_Off;
    }
}
