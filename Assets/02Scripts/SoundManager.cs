using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public enum SoundType
{
    Bgm,
    Effect,
}

public class SoundManager : SingletonBase<SoundManager>
{
    [SerializeField] private AudioSource[] _audioSources = new AudioSource[Enum.GetValues(typeof(SoundType)).Length];

    private Dictionary<string, AudioClip> _bgmAudioClips = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> _effectAudioClips = new Dictionary<string, AudioClip>();

    public float m_MasterVolume = 1.0f;

    public float m_BGMVolume = 1.0f;

    public float m_EffectVolume = 1.0f;


    protected override void Awake()
    {
        base.Awake();
       if(_instance == this)
       {
            DontDestroyOnLoad(gameObject);
       }
        Init();
    }

    /// <summary>
    /// �ʿ��� AudioSource ������Ʈ���� �����ؼ� �ʱ� �����մϴ�.
    /// </summary>
    private void Init()
    {
        foreach (SoundType name in Enum.GetValues(typeof(SoundType)))
        {
            GameObject obj = new GameObject(name.ToString());
            _audioSources[(int)name] = obj.AddComponent<AudioSource>();
            _audioSources[(int)name].playOnAwake = false;
            obj.transform.parent = this.transform;
        }

        _audioSources[(int)SoundType.Bgm].loop = true;
    }


    /// <summary>
    /// ��� ���带 ����ϴ�.
    /// </summary>
    public void StopSound()
    {
        // Stop bgm
        /* AudioSource audioSource = _audioSources[(int)SoundType.Bgm];
        //if (audioSource == null)
        //{
        //    return;
        //}

        //if (audioSource.isPlaying)
        //{
        //    audioSource.Stop();
        }*/

        // Stop all sounds
        foreach (AudioSource audiosource in _audioSources)
        {
            if (audiosource == null || audiosource.clip == null)
            {
                break;
            }

            if (audiosource.isPlaying)
            {
                audiosource.Stop();
            }
        }

    }

    /// <summary>
    /// �����Ŭ���� �����ͼ� ����մϴ�.
    /// </summary>
    /// <param name="audioName">������ ���ϸ��Դϴ�.</param>
    /// <param name="type">������ �����Դϴ�.</param>
    /// <param name="pitch">������ �������Դϴ�.</param>
    public void PlaySound(string audioName, SoundType type, float pitch = 1, float volume = 1)
    {
        AudioClip clip = GetOrLoadAudioClip(audioName, type);
        
        Play(clip, type, pitch, volume);

    }

    /// <summary>
    /// ������ҽ��� Ŭ���� ����մϴ�.
    /// </summary>
    /// <param name="audioClip"></param>
    /// <param name="type"></param>
    private void Play(AudioClip audioClip, SoundType type, float pitch = 1, float volume = 1)
    {
        if (audioClip == null)
        {
            return;
        }

        //if (volume != 1)
        //{
        //    float current = _audioSources[(int)SoundType.Bgm].volume;
        //    _audioSources[(int)SoundType.Bgm].volume = 0.5f;
        //}

        // bgm�� �ϳ��� ���
        if (type == SoundType.Bgm)
        {
            AudioSource audioSource = _audioSources[(int)SoundType.Bgm];
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
            
            audioSource.clip = audioClip;
            audioSource.pitch = pitch;
            audioSource.volume = volume * m_MasterVolume * m_BGMVolume;
            audioSource.Play();
        }
        else
        {
            AudioSource audioSource = _audioSources[(int)SoundType.Effect];
            audioSource.pitch = pitch;
            audioSource.volume = volume * m_MasterVolume * m_EffectVolume;
            audioSource.PlayOneShot(audioClip);  // ��ø�����ϰ� ���
        }
    }

    /// <summary>
    /// �����Ŭ���� �ִٸ� �������� ���ٸ� �ε��մϴ�.
    /// </summary>
    /// <param name="audioName">����������� �̸�</param>
    /// <param name="type">�����Ÿ��</param>
    /// <returns></returns>
    private AudioClip GetOrLoadAudioClip(string audioName, SoundType type = SoundType.Effect)
    {
        AudioClip audioClip;

        if (type == SoundType.Bgm)
        {
            // ��ųʸ��� �ִٸ� �������� ���ٸ� ���� �ε��ؼ� ��ųʸ��� ����
            if (_bgmAudioClips.TryGetValue(audioName, out audioClip) == false)
            {
                audioClip = Resources.Load<AudioClip>($"Sounds/{audioName}");
                _bgmAudioClips.Add(audioName, audioClip);
            }
            
        }
        else
        {
            if (_effectAudioClips.TryGetValue(audioName, out audioClip) == false)
            {
                audioClip = Resources.Load<AudioClip>($"Sounds/{audioName}");
                _effectAudioClips.Add(audioName, audioClip);
            }
        }

        if (audioClip == null)
        {
            Debug.Log("���� ���� ����");
        }

        return audioClip;

    }

    /// <summary>
    /// �����Ŭ���� �ִ� ��ųʸ��� ���� ���ϴ�.
    /// </summary>
    public void ClearDictionary()
    {
        _bgmAudioClips.Clear();
        _effectAudioClips.Clear();
    }

    public void SetMasterVolume(float value)
    {
        m_MasterVolume = value;
        SetBgmVolume(m_BGMVolume);
        SetEffectVolume(m_EffectVolume);
    }

    /// <summary>
    /// bgm�� ������ �����մϴ�.
    /// </summary>
    /// <param name="value"></param>
    public void SetBgmVolume(float value)
    {   
        m_BGMVolume = value;
        AudioSource bgmAudioSource = _audioSources[(int)SoundType.Bgm];
        bgmAudioSource.volume = value * m_MasterVolume;
    }

    /// <summary>
    /// ȿ������ ������ �����մϴ�.
    /// </summary>
    /// <param name="value"></param>
    public void SetEffectVolume(float value)
    {
        m_EffectVolume = value;
        AudioSource bgmAudioSource = _audioSources[(int)SoundType.Effect];
        bgmAudioSource.volume = value * m_MasterVolume;
    }
}