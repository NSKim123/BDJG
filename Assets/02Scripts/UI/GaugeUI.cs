using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GaugeUI : MonoBehaviour
{
    [Header("# 게이지 이미지")]
    public Image m_GaugeImage;    

    private float _TargetRatio;

    private void Update()
    {
        float newRatio = Mathf.Lerp(m_GaugeImage.fillAmount, _TargetRatio, 0.5f);
        m_GaugeImage.fillAmount = newRatio;
    }

    public void SetTargetRatio(float newTargetRatio)
    {
        _TargetRatio = newTargetRatio;
    }
}
