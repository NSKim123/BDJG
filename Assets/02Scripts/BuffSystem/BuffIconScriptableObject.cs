using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="BuffIconScriptableObject", menuName = "ScriptableObject/BuffSystem/BuffIconScriptableObject")]
public class BuffIconScriptableObject : ScriptableObject
{
    public List<BuffInfo> m_BuffInfos = new List<BuffInfo>();

    public Sprite GetBuffIconByBuffCode(int buffCode)
    {
        return m_BuffInfos.Find((buffInfo) => buffInfo.m_BuffCode == buffCode).m_Icon;
    }
}

[Serializable]
public class BuffInfo
{
    public int m_BuffCode;
    public Sprite m_Icon;
}