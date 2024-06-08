using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerModelData", menuName = "ScriptableObject/PlayerModelScriptableObject")]
public class PlayerModelScriptableObject : ScriptableObject
{
    public List<PlayerModelInfo> m_Models = new List<PlayerModelInfo>();

    public GameObject FindModelByLevel(int level)
    {
        return m_Models.Find((PlayerModelInfo modelInfo) => modelInfo.m_Level == level).m_ModelPrefab;
    }
}

[Serializable]
public class PlayerModelInfo
{
    public int m_Level;
    public GameObject m_ModelPrefab;
}