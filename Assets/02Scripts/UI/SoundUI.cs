using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundUI : MonoBehaviour
{
    [Header("# ������ ���� ��ũ�� ��")]
    public Scrollbar m_ScrollBar_Master;

    [Header("# ������� ���� ��ũ�� ��")]
    public Scrollbar m_ScrollBar_BGM;

    [Header("# ȿ���� ���� ��ũ�� ��")]
    public Scrollbar m_ScrollBar_Effect;

    private void Start()
    {
        m_ScrollBar_Master.onValueChanged.AddListener(SetMasterVolume);
        m_ScrollBar_BGM.onValueChanged.AddListener(SetBGMVolume);
        m_ScrollBar_Effect.onValueChanged.AddListener(SetEffectVolume);
    }

    private void OnEnable()
    {
        SetScrollBarsValues();
    }

    private void SetScrollBarsValues()
    {
        m_ScrollBar_Master.value = SoundManager.Instance.m_MasterVolume;
        m_ScrollBar_BGM.value = SoundManager.Instance.m_BGMVolume;
        m_ScrollBar_Effect.value = SoundManager.Instance.m_EffectVolume;
    }

    private void SetMasterVolume(float volume)
    {
        SoundManager.Instance.SetMasterVolume(volume);
    }

    private void SetBGMVolume(float volume)
    {
        SoundManager.Instance.SetBgmVolume(volume);
    }

    private void SetEffectVolume(float volume)
    {
        SoundManager.Instance.SetEffectVolume(volume);
    }
}
