using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Effect_StopLoop : MonoBehaviour
{
    private ParticleSystem _ParticleSystem;

    public List<ParticleSystem> _LoopedParticleSystem;

    private void Awake()
    {
        _ParticleSystem = GetComponent<ParticleSystem>();        
    }

    private void Update()
    {
        if(_ParticleSystem.totalTime >= _ParticleSystem.main.duration)
        {
            ParticleSystem.MainModule newMain;
            foreach (ParticleSystem p in _LoopedParticleSystem)
            {
                newMain = p.main;
                newMain.loop = false;                
            }
        }

    }


}
