using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleGaugeUI : GaugeUI
{
    [Header("# ��׶��� �̹���")]
    public Image m_BackgroundImage;

    [Header("# �¿��� �� ���� ����")]
    [Header("��")]
    public Color m_Color_On = Color.green;

    [Header("����")]
    public Color m_Color_Off = Color.grey;

    public virtual void OnToggleChanged(bool toggleSwitch)
    {
        Color newBackgroundColor;

        if(toggleSwitch)
        {
            newBackgroundColor = m_Color_On;
        }
        else
        {
            newBackgroundColor = m_Color_Off;
        }

        m_BackgroundImage.color = newBackgroundColor;
    }
}
