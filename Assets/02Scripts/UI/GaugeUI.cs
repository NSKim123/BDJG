using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GaugeUI : MonoBehaviour
{
    [Header("# ������ �̹���")]
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
