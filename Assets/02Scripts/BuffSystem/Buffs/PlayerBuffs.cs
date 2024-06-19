using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class GiantBuff : TimerBuff
{
    public GiantBuff(int buffCode, GameObject owner, float buffTime, int maxStack = 1) : base(buffCode, owner, buffTime, maxStack)
    {
    }

    public override Buff Clone(GameObject owner)
    {
        return new GiantBuff(buffCode, owner, maxTime, maxStack);
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

