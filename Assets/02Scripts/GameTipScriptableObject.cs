using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameTipScriptableObject", menuName = "ScriptableObject/GameTip")]
public class GameTipScriptableObject : ScriptableObject
{
    public List<GameTipInfo> m_GameTips = new List<GameTipInfo>();

    public GameTipInfo GetRandomGameTipInfo()
    {
        return m_GameTips[UnityEngine.Random.Range(0, m_GameTips.Count)];
    }
}

[Serializable]
public class GameTipInfo
{
    public Sprite m_Icon;
    public string m_Description;
}