using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGaugeUI : ToggleGaugeUI
{
    protected override void UpdateFillAmount()
    {
        // 부드럽게 변화시킵니다.
        float newFillAmount = Mathf.MoveTowards(m_GaugeImage.fillAmount, _TargetRatio, 0.2f * Time.deltaTime);
        m_GaugeImage.fillAmount = newFillAmount;
    }
}
