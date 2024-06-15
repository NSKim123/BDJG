using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSystem
{
    private static Dictionary<int, Buff> _BuffDictionary;

    private List<Buff> _BuffList;

    private GameObject _Owner;

    public List<Buff> buffList => _BuffList;

    public event System.Action<Buff> onBuffStarted;
    public event System.Action<Buff> onBuffRenew;
    public event System.Action<Buff> onBuffNotMuchLeft;
    public event System.Action<Buff> onBuffFinished;


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
                { 100000, new MoveSpeedBuff(100000, null, 0.2f, 10.0f) },
                { 100001, new MoveSpeedBuff(100001, null, -0.2f, 15.0f) },
                { 100002, new MoveSpeedBuff(100002, null, -0.3f, 5.0f) },
                { 100003, new MoveSpeedBuff(100003, null, -0.4f, 7.0f) },
            };

        }     
    }

    public void AddBuff(int buffCode)
    {
        if (!_BuffDictionary.ContainsKey(buffCode)) return;

        Buff newBuff = _BuffList.Find((Buff buffInList) => buffCode == buffInList.buffCode);

        if (newBuff != null)
        {
            newBuff.OnRenewBuff();
            onBuffRenew?.Invoke(newBuff);
        }
        else
        {
            newBuff = _BuffDictionary[buffCode].Clone(_Owner);
            _BuffList.Add(newBuff);
            newBuff.OnStartBuff();            
            onBuffStarted?.Invoke(newBuff);
        }
    }

    public void UpdateBuffList()
    {
        List<Buff> finishedBuffList = new();

        foreach(var buff in _BuffList)
        {
            buff.OnUpdateBuff();

            if(buff.notMuchLeft)
            {
                onBuffNotMuchLeft?.Invoke(buff);
            }
            else if(buff.isFinished)
            {
                finishedBuffList.Add(buff);
            }
        }

        FinishBuffs(finishedBuffList);
    }

    private void FinishBuffs(List<Buff> finishedBuffList)
    {
        foreach(var FinishedBuff in finishedBuffList)
        {
            FinishedBuff.OnFinishedBuff();
            onBuffFinished?.Invoke(FinishedBuff);
            _BuffList.Remove(FinishedBuff);
        }
    }
}

public abstract class Buff
{
    protected GameObject _Owner;

    public int buffCode { get; protected set; }

    public int maxStack { get; protected set; }

    public int currentStack { get; protected set; }

    public bool visibility { get; protected set; }

    public Buff(int buffCode, GameObject owner, int maxStack = 1)
    {
        this.buffCode = buffCode;
        _Owner = owner;
        this.maxStack = maxStack;
    }

    public abstract Buff Clone(GameObject owner);

    public virtual bool notMuchLeft => false;

    public bool isFinished { get; protected set; }

    protected virtual void IncreaseStack()
    {
        if (currentStack < maxStack) currentStack += 1;
    }   

    public virtual void OnStartBuff()
    {
        IncreaseStack();
        onStartBuffContext();
    }

    public virtual void OnRenewBuff()
    {
        IncreaseStack();
        onRenewBuffContext();
    }

    public virtual void OnUpdateBuff() => onUpdateBuffContext();

    public virtual void OnFinishedBuff() => onFinishBuffContext();

    // 실제 버프 효과들은 이 추상 메서드에 구현시켜면 됩니다
    protected abstract void onStartBuffContext();
    protected abstract void onRenewBuffContext();
    protected abstract void onUpdateBuffContext();
    protected abstract void onFinishBuffContext();
}

public abstract class TimerBuff : Buff
{
    public TimerBuff(int buffCode, GameObject owner, float buffTime, int maxStack = 1) : base(buffCode, owner, maxStack)
    {
        this.maxTime = buffTime;
        this.currentTime = buffTime;
    }

    public float maxTime { get; protected set; }

    public float currentTime { get; protected set; }

    public override bool notMuchLeft => (currentTime < 5.0f && !isFinished);

    private void RenewBuffTimer()
    {
        currentTime = maxTime;
    }

    private void UpdateBuffTimer()
    {
        currentTime -= Time.deltaTime;

        if (currentTime <= 0.0f) isFinished = true;
    }

    public sealed override void OnRenewBuff()
    {
        base.OnRenewBuff();
        RenewBuffTimer();
    }

    public sealed override void OnUpdateBuff()
    {
        base.OnUpdateBuff();
        UpdateBuffTimer();       
    }
}

