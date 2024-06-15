using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 레벨에 따라 사용할 플레이어의 모델을 설정할 수 있는 스크립터블 오브젝트 클래스입니다.
/// </summary>
[CreateAssetMenu(fileName = "PlayerModelData", menuName = "ScriptableObject/Player/PlayerModelScriptableObject")]
public class PlayerModelScriptableObject : ScriptableObject
{
    /// <summary>
    /// PlayerModelInfo 객체를 모아둔 리스트
    /// </summary>
    public List<PlayerModelInfo> m_Models = new List<PlayerModelInfo>();

    /// <summary>
    /// 전달된 레벨에 따라 그에 맞는 모델 프리팹을 반환하는 메서드입니다.
    /// </summary>
    /// <param name="level"> 레벨</param>
    /// <returns> 매개변수로 전달받은 레벨에 맞는 모델 프리팹을 반환합니다.</returns>
    public GameObject FindModelByLevel(int level)
    {
        return m_Models.Find((PlayerModelInfo modelInfo) => modelInfo.m_Level == level).m_ModelPrefab;
    }
}

/// <summary>
/// 레벨에 따라 어떤 모델을 사용할지 나타내는 클래스입니다.
/// </summary>
[Serializable]
public class PlayerModelInfo
{
    /// <summary>
    /// 플레이어의 레벨
    /// </summary>
    public int m_Level;

    /// <summary>
    /// 사용할 모델 프리팹
    /// </summary>
    public GameObject m_ModelPrefab;
}