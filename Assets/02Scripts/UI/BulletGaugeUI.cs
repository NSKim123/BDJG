using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �÷��̾��� źâ ������ UI ������Ʈ�Դϴ�.
/// </summary>
public class BulletGaugeUI : ToggleGaugeUI
{
    /// <summary>
    /// ������ ������ ���� �̹����� ä��� �޼����Դϴ�.
    /// �θ� Ŭ������ �Լ��ʹ� �޸� �ε巴�� ��ȭ��ŵ�ϴ�.
    /// </summary>
    protected override void UpdateFillAmount()
    {
        // �ε巴�� ��ȭ��ŵ�ϴ�.
        float newFillAmount = Mathf.MoveTowards(m_GaugeImage.fillAmount, _CurrentRatio, 0.2f * Time.deltaTime);
        m_GaugeImage.fillAmount = newFillAmount;
    }
}
