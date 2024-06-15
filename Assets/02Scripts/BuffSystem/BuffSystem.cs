using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSystem
{
    private static Dictionary<int, Buff> _BuffDictionary;

    private static BuffIconScriptableObject _BuffIcons;

    private List<Buff> _BuffList;

    private GameObject _Owner;

    public BuffSystem(GameObject owner)
    {
        LoadAllBuffs();   

        _BuffList = new List<Buff>();

        _Owner = owner;
    }

    private void LoadAllBuffs()
    {
        if (_BuffDictionary == null)
        {
            _BuffDictionary = new Dictionary<int, Buff>
            {
                { 100000, new MoveSpeedBuff(null, 0.2f, 10.0f) },

            };

        }        

        if(_BuffIcons == null)
        {
            _BuffIcons = Resources.Load<BuffIconScriptableObject>("ScriptableObject/BuffIconScriptableObject");
        }
    }

    public void AddBuff(int buffCode)
    {
        if (!_BuffDictionary.ContainsKey(buffCode)) return;

        Buff newBuff = _BuffList.Find((Buff buffInList) => _BuffDictionary[buffCode] == buffInList);

        if (newBuff != null)
        {
            newBuff.OnRenewBuff();
        }
        else
        {
            newBuff = _BuffDictionary[buffCode].Clone(_Owner);
            _BuffList.Add(newBuff);
            newBuff.OnStartBuff();
        }
    }

    public void UpdateBuffList()
    {
        List<Buff> finishedBuffList = new();

        foreach(var buff in _BuffList)
        {
            buff.OnUpdateBuff();

            if(buff.isFinished)
            {
                finishedBuffList.Add(buff);
            }
        }

        FinishBuffs(finishedBuffList);
    }

    private void FinishBuffs(List<Buff> finishedBuffList)
    {
        foreach(var buff in finishedBuffList)
        {
            buff.OnFinishedBuff();
            _BuffList.Remove(buff);
        }
    }
}

public abstract class Buff
{
    protected GameObject _Owner;

    public Buff(GameObject owner)
    {
        _Owner = owner;
    }

    public abstract Buff Clone(GameObject owner);

    public bool isFinished { get; protected set; }

    public abstract void OnStartBuff();

    public abstract void OnUpdateBuff();

    public abstract void OnRenewBuff();

    public abstract void OnFinishedBuff();
    
}

public abstract class TimerBuff : Buff
{
    public TimerBuff(GameObject owner, float buffTime) : base(owner)
    {
        this.maxTime = buffTime;
        this.currentTime = buffTime;
    }

    public float maxTime { get; set; }

    public float currentTime { get; set; }

    private void RenewBuffTimer()
    {
        currentTime = maxTime;
    }

    private void UpdateBuffTimer()
    {
        currentTime -= Time.deltaTime;

        if (currentTime <= 0.0f) isFinished = true;
    }

    public override void OnRenewBuff()
    {
        RenewBuffTimer();
    }

    public override void OnUpdateBuff()
    {
        UpdateBuffTimer();       
    }
}

public abstract class StackBuff : Buff
{
    public StackBuff(GameObject owner, int maxStack) : base(owner)
    {
        this.maxStack = maxStack;
        currentStack = 0;
    }

    public int maxStack { get; set; }

    public int currentStack { get; set; }


    public override void OnStartBuff()
    {
        currentStack += 1;
    }

    public override void OnRenewBuff()
    {
        OnStartBuff();
    }
}

public abstract class StackTimerBuff : TimerBuff
{
    public int maxStack { get; set; }

    public int currentStack { get; set; }

    protected StackTimerBuff(GameObject owner, float buffTime, int maxStack) : base(owner, buffTime)
    {
        this.maxStack = maxStack;
        currentStack = 0;
    }

    public override void OnStartBuff()
    {
        currentStack += 1;
    }

    public override void OnRenewBuff()
    {
        base.OnRenewBuff();
        OnStartBuff();
    }
}