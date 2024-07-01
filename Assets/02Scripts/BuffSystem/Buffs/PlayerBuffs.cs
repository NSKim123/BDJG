using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class GiantBuff : TimerBuff
{
    public GiantBuff(int buffCode, GameObject owner, BuffType buffType, float buffTime, bool visibility = false, int maxStack = 1) 
        : base(buffCode, owner, buffType, buffTime, visibility, maxStack)
    {
    }

    public override Buff Clone(GameObject owner)
    {
        return new GiantBuff(buffCode, owner, buffType, maxTime, visibility, maxStack);
    }

    protected override void onFinishBuffContext()
    {
        _Owner.GetComponent<PlayerCharacter>().OnFinishGiant();
    }

    protected override void onRenewBuffContext()
    {
        
    }

    protected override void onStartBuffContext()
    {
        _Owner.GetComponent<PlayerCharacter>().OnStartGiant();
    }

    protected override void onUpdateBuffContext()
    {
        
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
        GameObject.Instantiate(effect);

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
        _PlayerCharacter.attackComponent.m_PushPowerMultiplier *= 1.5f;

        _PlayerCharacter.attackComponent.bulletGauge.currentValue = _PlayerCharacter.attackComponent.bulletGauge.max;
        _PlayerCharacter.attackComponent.m_CostBulletGauge -= 1;
        _PlayerCharacter.attackComponent.m_AttackReuseTime = 0.0f;
        
    }

    protected override void onRenewBuffContext()
    {
        
    }

    protected override void onUpdateBuffContext()
    {
        _PlayerCharacter.movementComponent.SetMovable(false);
        _PlayerCharacter.movementComponent.SetImmuneState(true);

    }

    protected override void onFinishBuffContext()
    {
        _PlayerCharacter.attackComponent.m_PushPowerMultiplier /= 1.5f;

        _PlayerCharacter.attackComponent.bulletGauge.currentValue = _PlayerCharacter.attackComponent.bulletGauge.min;
        _PlayerCharacter.attackComponent.m_CostBulletGauge += 1;
        _PlayerCharacter.attackComponent.m_AttackReuseTime = 0.33f;        

        _PlayerCharacter.movementComponent.SetMovable(true);
        _PlayerCharacter.movementComponent.SetImmuneState(false);

        //3초 간 탄환을 쏠 수 없는 디버프 부여
    }
}

public class ShellBuff : TimerBuff
{
    private Shell _ShellPrefab;

    private Bullet _PrevBullet;

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
        _ShellPrefab = Resources.Load<Shell>("Prefabs/Shell");
        _PlayerCharacter.attackComponent.m_PushPowerMultiplier *= 3.0f;
        _PrevBullet = _PlayerCharacter.attackComponent.m_Bullet;
        _PlayerCharacter.attackComponent.m_Bullet = _ShellPrefab;

        _PlayerCharacter.attackComponent.bulletGauge.currentValue = _PlayerCharacter.attackComponent.bulletGauge.max;
        _PlayerCharacter.attackComponent.m_CostBulletGauge = (_PlayerCharacter.attackComponent.bulletGauge.max - _PlayerCharacter.attackComponent.bulletGauge.min) / 4;
        _PlayerCharacter.attackComponent.m_AttackReuseTime = 2.0f;
    }

    protected override void onRenewBuffContext()
    {
        
    }

    protected override void onUpdateBuffContext()
    {
        
    }

    protected override void onFinishBuffContext()
    {
        _PlayerCharacter.attackComponent.m_PushPowerMultiplier /= 3.0f;
        _PlayerCharacter.attackComponent.m_Bullet = _PrevBullet;

        _PlayerCharacter.attackComponent.bulletGauge.currentValue = _PlayerCharacter.attackComponent.bulletGauge.min;
        _PlayerCharacter.attackComponent.m_CostBulletGauge = 1;
        _PlayerCharacter.attackComponent.m_AttackReuseTime = 0.33f;

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
    }

    protected override void onRenewBuffContext()
    {
        
    }

    protected override void onUpdateBuffContext()
    {
        _PlayerCharacter.movementComponent.SetMovable(false);
    }

    protected override void onFinishBuffContext()
    {
        _PlayerCharacter.movementComponent.SetMovable(true);

        GameObject.Destroy(_InstantiatedPrefab);

        // 이펙트 제거
        // 적 스폰 재개
    }

}