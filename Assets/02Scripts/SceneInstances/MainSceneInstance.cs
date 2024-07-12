using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneInstance : SceneInstanceBase
{
    protected override void Awake()
    {
        Time.timeScale = 1.0f;

        base.Awake();

        SoundManager.Instance.PlaySound("bgm", SoundType.Bgm, 1.0f, 0.3f);
    }
}
