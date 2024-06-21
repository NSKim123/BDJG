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
