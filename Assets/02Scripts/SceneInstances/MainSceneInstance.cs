using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneInstance : SceneInstanceBase
{
    protected override void Awake()
    {
        Time.timeScale = 1.0f;

        base.Awake();
    }
}
