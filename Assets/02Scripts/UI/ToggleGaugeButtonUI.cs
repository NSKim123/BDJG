using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleGaugeButtonUI : ToggleGaugeUI
{
    [Header("# ¹öÆ°")]
    public Button m_Button;

    public void BindClickEvent(UnityEngine.Events.UnityAction bindFunction)
    {
        m_Button.onClick.AddListener(bindFunction);
    }

    public void UnbindClickEvent(UnityEngine.Events.UnityAction unbindFunction)
    {
        m_Button.onClick.RemoveListener(unbindFunction);
    }

    public override void OnToggleChanged(bool toggleSwitch)
    {
        base.OnToggleChanged(toggleSwitch);

        m_Button.interactable = toggleSwitch;
    }
}
