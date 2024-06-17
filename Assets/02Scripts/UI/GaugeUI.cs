using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ������ ������ ���� �̹����� ä��� UI ������Ʈ�Դϴ�.
/// </summary>
public class GaugeUI : MonoBehaviour
{
    [Header("# ������ �̹���")]
    public Image m_GaugeImage;    

    /// <summary>
    /// ���� ������ ����
    /// </summary>
    protected float _CurrentRatio;

    protected virtual void Update()
    {
        // ������ ������ ���� �̹����� ä��ϴ�.
        UpdateFillAmount();
    }

    /// <summary>
    /// ������ ������ ���� �̹����� ä��� �޼����Դϴ�.
    /// </summary>
    protected virtual void UpdateFillAmount()
    {        
        m_GaugeImage.fillAmount = _CurrentRatio;
    }

    /// <summary>
    /// ������ ������ �����մϴ�.
    /// </summary>
    /// <param name="newRatio"> ������ ������ ����</param>
    public void SetRatio(float newRatio)
    {
        _CurrentRatio = newRatio;
    }
}
