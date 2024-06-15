using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어의 탄창 게이지 UI 컴포넌트입니다.
/// </summary>
public class BulletGaugeUI : ToggleGaugeUI
{
    /// <summary>
    /// 게이지 비율에 따라 이미지를 채우는 메서드입니다.
    /// 부모 클래스의 함수와는 달리 부드럽게 변화시킵니다.
    /// </summary>
    protected override void UpdateFillAmount()
    {
        // 부드럽게 변화시킵니다.
        float newFillAmount = Mathf.MoveTowards(m_GaugeImage.fillAmount, _CurrentRatio, 0.2f * Time.deltaTime);
        m_GaugeImage.fillAmount = newFillAmount;
    }
}
