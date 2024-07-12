using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : SingletonBase<GameData>
{
    public string m_NextSceneName;

    protected override void Awake()
    {
        base.Awake();

        if (_instance == this)
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
