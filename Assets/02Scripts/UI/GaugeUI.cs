using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 게이지 비율에 따라 이미지를 채우는 UI 컴포넌트입니다.
/// </summary>
public class GaugeUI : MonoBehaviour
{
    [Header("# 게이지 이미지")]
    public Image m_GaugeImage;    

    /// <summary>
    /// 현재 게이지 비율
    /// </summary>
    protected float _CurrentRatio;

    protected virtual void Update()
    {
        // 게이지 비율에 따라 이미지를 채웁니다.
        UpdateFillAmount();
    }

    /// <summary>
    /// 게이지 비율에 따라 이미지를 채우는 메서드입니다.
    /// </summary>
    protected virtual void UpdateFillAmount()
    {        
        m_GaugeImage.fillAmount = _CurrentRatio;
    }

    /// <summary>
    /// 게이지 비율을 설정합니다.
    /// </summary>
    /// <param name="newRatio"> 설정할 게이지 비율</param>
    public void SetRatio(float newRatio)
    {
        _CurrentRatio = newRatio;
    }
}
