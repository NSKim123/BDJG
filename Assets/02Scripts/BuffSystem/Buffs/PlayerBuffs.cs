using System;
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

