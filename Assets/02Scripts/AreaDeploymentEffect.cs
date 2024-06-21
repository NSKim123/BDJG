using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class AreaDeploymentEffect : MonoBehaviour
{   
    public float m_Time;

    public Material m_AreaDeploymentEffectMaterial;

    private void Awake()
    {
        Destroy(gameObject, 3.5f);
        transform.parent = FindAnyObjectByType<Canvas>().transform;
        (transform as RectTransform).anchoredPosition = Vector3.zero;
    }

    private void Update()
    {   
        m_AreaDeploymentEffectMaterial.SetFloat("_Boundary", m_Time * 2.0f);
    }        

    private void OnDestroy()
    {
        m_AreaDeploymentEffectMaterial.SetFloat("_Boundary", 0.0f);
    }
}
