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


    private void Awake()
    {
        Init();
    }

    /// <summary>
    /// 필요한 AudioSource 오브젝트들을 생성해서 초기 세팅합니다.
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
    /// 모든 사운드를 멈춥니다.
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
    /// 오디오클립을 가져와서 재생합니다.
    /// </summary>
    /// <param name="audioName">사운드의 파일명입니다.</param>
    /// <param name="type">사운드의 종류입니다.</param>
    public void PlaySound(string audioName, SoundType type, float pitch = 1)
    {
        AudioClip clip = GetOrLoadAudioClip(audioName, type);
        
        Play(clip, type, pitch);

    }

    /// <summary>
    /// 오디오소스의 클립을 재생합니다.
    /// </summary>
    /// <param name="audioClip"></param>
    /// <param name="type"></param>
    private void Play(AudioClip audioClip, SoundType type, float pitch = 1)
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

        // bgm은 하나만 재생
        if (type == SoundType.Bgm)
        {
            AudioSource audioSource = _audioSources[(int)SoundType.Bgm];
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
            
            audioSource.clip = audioClip;
            audioSource.pitch = pitch;
            audioSource.Play();
        }
        else
        {
            AudioSource audioSource = _audioSources[(int)SoundType.Effect];
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(audioClip);  // 중첩가능하게 재생
        }
    }

    /// <summary>
    /// 오디오클립이 있다면 꺼내쓰고 없다면 로드합니다.
    /// </summary>
    /// <param name="audioName">오디오파일의 이름</param>
    /// <param name="type">오디오타입</param>
    /// <returns></returns>
    private AudioClip GetOrLoadAudioClip(string audioName, SoundType type = SoundType.Effect)
    {
        AudioClip audioClip;

        if (type == SoundType.Bgm)
        {
            // 딕셔너리에 있다면 꺼내쓰고 없다면 새로 로드해서 딕셔너리에 저장
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
            Debug.Log("사운드 파일 없음");
        }

        return audioClip;

    }

    /// <summary>
    /// 오디오클립이 있는 딕셔너리를 전부 비웁니다.
    /// </summary>
    public void ClearDictionary()
    {
        _bgmAudioClips.Clear();
        _effectAudioClips.Clear();
    }

    /// <summary>
    /// bgm의 볼륨을 조정합니다.
    /// </summary>
    /// <param name="value"></param>
    public void SetBgmVolume(float value)
    {
        AudioSource bgmAudioSource = _audioSources[(int)SoundType.Bgm];
        bgmAudioSource.volume = value;
    }

    /// <summary>
    /// 효과음의 볼륨을 조정합니다.
    /// </summary>
    /// <param name="value"></param>
    public void SetEffectVolume(float value)
    {
        AudioSource bgmAudioSource = _audioSources[(int)SoundType.Effect];
        bgmAudioSource.volume = value;
    }
}