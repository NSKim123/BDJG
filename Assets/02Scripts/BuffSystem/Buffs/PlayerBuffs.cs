﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class GiantBuff : TimerBuff
{
    public GiantBuff(int buffCode, GameObject owner, BuffType buffType, 
                        float buffTime, bool visibility = false, int maxStack = 1) 
        : base(buffCode, owner, buffType, buffTime, visibility, maxStack)
    {
    }

    public override Buff Clone(GameObject owner)
    {
        return new GiantBuff(buffCode, owner, buffType, maxTime, visibility, maxStack);
    }

    protected override void onStartBuffContext()
    {
        _Owner.GetComponent<PlayerCharacter>().OnStartGiant();
    }    

    protected override void onRenewBuffContext()
    {
        
    }    

    protected override void onUpdateBuffContext()
    {
        
    }

    protected override void onFinishBuffContext()
    {
        _Owner.GetComponent<PlayerCharacter>().OnFinishGiant();
    }
}

public class AreaDeploymentBuff : TimerBuff
{
    GameObject effect;

    public AreaDeploymentBuff(int buffCode, GameObject owner, BuffType buffType, float buffTime, bool visibility = false, int maxStack = 1)
        : base(buffCode, owner, buffType, buffTime, visibility, maxStack)
    {
        
    }

    public override Buff Clone(GameObject owner)
    {
        return new AreaDeploymentBuff(buffCode, owner, buffType, maxTime, visibility, maxStack);
    }

    protected override void onFinishBuffContext()
    {
        
    }

    protected override void onRenewBuffContext()
    {

    }

    protected override void onStartBuffContext()
    {
        effect = Resources.Load<GameObject>("Effects/AreaDeploymentUIEffect");
        _Owner.GetComponent<PlayerCharacter>().StartCoroutine(C_AreaDeployment());
    }

    protected override void onUpdateBuffContext()
    {

    }

    private IEnumerator C_AreaDeployment()
    {
        Time.timeScale = 0.0f;
        GameObject instantiatedEffect = GameObject.Instantiate(effect);

        (instantiatedEffect.transform as RectTransform).anchorMin = Vector2.zero;
        (instantiatedEffect.transform as RectTransform).anchorMax = Vector2.one;
        (instantiatedEffect.transform as RectTransform).localScale = Vector2.one * 2.0f;
        (instantiatedEffect.transform as RectTransform).offsetMin = Vector2.zero;
        (instantiatedEffect.transform as RectTransform).offsetMax = Vector2.zero;

        yield return new WaitForSecondsRealtime(3.3f);

        Time.timeScale = 1.0f;

        Collider[] enemies = Physics.OverlapSphere(_Owner.transform.position, 40f, LayerMask.GetMask("Enemy"));

        foreach (var enemy in enemies)
        {
            enemy.GetComponent<IHit>().OnDead();
        }
    }
}



public class MachineGunBuff : TimerBuff
{
    private PlayerCharacter _PlayerCharacter;

    public MachineGunBuff(int buffCode, GameObject owner, BuffType buffType, float buffTime, bool visibility = false, int maxStack = 1)
        : base(buffCode, owner, buffType, buffTime, visibility, maxStack)
    {
        if(owner != null)
            _PlayerCharacter = _Owner.GetComponent<PlayerCharacter>();
    }

    public override Buff Clone(GameObject owner)
    {
        return new MachineGunBuff(buffCode, owner, buffType, maxTime, visibility, maxStack);
    }

    protected override void onStartBuffContext()
    {
        _PlayerCharacter.OnStartMachineGun();
        
    }

    protected override void onRenewBuffContext()
    {
        
    }

    protected override void onUpdateBuffContext()
    {
        _PlayerCharacter.OnUpdateMachineGun();
    }

    protected override void onFinishBuffContext()
    {
        _PlayerCharacter.OnFinishMachinGun();
        //3초 간 탄환을 쏠 수 없는 디버프 부여
    }
}

public class ShellBuff : TimerBuff
{    
    private PlayerCharacter _PlayerCharacter;

    public ShellBuff(int buffCode, GameObject owner, BuffType buffType, float buffTime, bool visibility = false, int maxStack = 1) : base(buffCode, owner, buffType, buffTime, visibility, maxStack)
    {
        if (owner != null)
            _PlayerCharacter = _Owner.GetComponent<PlayerCharacter>();
    }

    public override Buff Clone(GameObject owner)
    {
        return new ShellBuff(buffCode, owner, buffType, maxTime, visibility, maxStack);
    }


    protected override void onStartBuffContext()
    {
        _PlayerCharacter.OnStartShell();
    }

    protected override void onRenewBuffContext()
    {
        
    }

    protected override void onUpdateBuffContext()
    {
        _PlayerCharacter.OnUpdateShell();
    }

    protected override void onFinishBuffContext()
    {
        _PlayerCharacter.OnFinishShell();
        //3초관 탄환을 쏠 수 없는 디버프 부여
    }
}

public class ScarecrowBuff : TimerBuff
{
    private GameObject _AlterEgoPrefab;

    private GameObject _InstantiatedPrefab;

    private PlayerCharacter _PlayerCharacter;

    public ScarecrowBuff(int buffCode, GameObject owner, BuffType buffType, float buffTime, bool visibility = false, int maxStack = 1) : base(buffCode, owner, buffType, buffTime, visibility, maxStack)
    {
        if (owner != null)
            _PlayerCharacter = _Owner.GetComponent<PlayerCharacter>();
    }

    public override Buff Clone(GameObject owner)
    {
        return new ScarecrowBuff(buffCode, owner, buffType, maxTime, visibility, maxStack);
    }

    protected override void onStartBuffContext()
    {
        _AlterEgoPrefab = Resources.Load<GameObject>("Prefabs/Scarecrow");
        _InstantiatedPrefab = GameObject.Instantiate(_AlterEgoPrefab);
        _InstantiatedPrefab.transform.position = _PlayerCharacter.transform.position;
    }

    protected override void onRenewBuffContext()
    {
        
    }

    protected override void onUpdateBuffContext()
    {
        
    }

    protected override void onFinishBuffContext()
    {
        GameObject.Destroy(_InstantiatedPrefab);
    }
}

public class WindBuff : TimerBuff
{
    private static string AUDIONAME_WIND = "Effect_Wind_ver1";

    private GameObject _WindPrefab;

    private GameObject _InstantiatedPrefab;

    private GameObject _Effect;

    private PlayerCharacter _PlayerCharacter;

    public WindBuff(int buffCode, GameObject owner, BuffType buffType, float buffTime, bool visibility = false, int maxStack = 1) : base(buffCode, owner, buffType, buffTime, visibility, maxStack)
    {
        if (owner != null)
            _PlayerCharacter = _Owner.GetComponent<PlayerCharacter>();
    }

    public override Buff Clone(GameObject owner)
    {
        return new WindBuff(buffCode, owner, buffType, maxTime, visibility, maxStack);
    }

    protected override void onStartBuffContext()
    {
        _WindPrefab = Resources.Load<GameObject>("Prefabs/Wind");
        _InstantiatedPrefab = GameObject.Instantiate(_WindPrefab);

        // 이펙트 생성

        // 적 스폰 일시정지
        EnemyManager.Instance.spawner.PauseSwitchEnemySpawn(true);

        SoundManager.Instance.PlaySound(AUDIONAME_WIND, SoundType.Effect);
    }

    protected override void onRenewBuffContext()
    {
        
    }

    protected override void onUpdateBuffContext()
    {
        
    }

    protected override void onFinishBuffContext()
    {
        GameObject.Destroy(_InstantiatedPrefab);

        // 이펙트 제거

        // 적 스폰 재개
        EnemyManager.Instance.spawner.PauseSwitchEnemySpawn(false);

    }

}