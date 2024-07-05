using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class AreaDeploymentEffect : MonoBehaviour
{
    private static string SOUNDNAME_AREADEPLOYMENT_INIT = "Effect_AreaDeployment_step1";
    private static string SOUNDNAME_AREADEPLOYMENT_CHARACTER = "Effect_Slime_GiantLand";
    private static string SOUNDNAME_AREADEPLOYMENT_FINISH = "Effect_AreaDeployment_step2";

    public float m_Time;

    public Material m_AreaDeploymentEffectMaterial;

    private void Awake()
    {
        RectTransform gameSceneUI = FindAnyObjectByType<Canvas>().transform as RectTransform;
        (transform as RectTransform).localScale = gameSceneUI.localScale * 2.0f;
        transform.SetParent(gameSceneUI);
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

    public void PlaySound_Init()
    {
        SoundManager.Instance.PlaySound(SOUNDNAME_AREADEPLOYMENT_INIT, SoundType.Effect);
    }

    public void PlaySound_Character()
    {
        SoundManager.Instance.PlaySound(SOUNDNAME_AREADEPLOYMENT_CHARACTER, SoundType.Effect);
    }

    public void PlaySound_Finish()
    {
        SoundManager.Instance.PlaySound(SOUNDNAME_AREADEPLOYMENT_FINISH, SoundType.Effect);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
