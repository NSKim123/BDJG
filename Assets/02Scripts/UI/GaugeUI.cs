using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GaugeUI : MonoBehaviour
{
    [Header("# 게이지 이미지")]
    public Image m_GaugeImage;    

    protected float _TargetRatio;

    protected virtual void Update()
    {
        UpdateFillAmount();
    }

    protected virtual void UpdateFillAmount()
    {        
        m_GaugeImage.fillAmount = _TargetRatio;
    }

    public void SetTargetRatio(float newTargetRatio)
    {
        _TargetRatio = newTargetRatio;
    }
}
