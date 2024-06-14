using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleGaugeUI : GaugeUI
{
    [Header("# 백그라운드 이미지")]
    public Image m_BackgroundImage;

    [Header("# 온오프 시 색상 관련")]
    [Header("온")]
    public Color m_Color_On = Color.green;

    [Header("오프")]
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
