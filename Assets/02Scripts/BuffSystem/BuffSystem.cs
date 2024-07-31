using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� ����Ʈ�� �����ϴ� Ŭ�����Դϴ�.
/// </summary>
public class BuffSystem
{
    /// <summary>
    /// ��� ������ ���� ������ �����ϴ� Dictionary (���� ����)
    /// </summary>
    private static Dictionary<int /*���� �ڵ�*/, Buff /*���� �ڵ忡 �´� ���� ��ü*/ > _BuffDictionary;

    /// <summary>
    /// ���� ����Ʈ ��ü
    /// </summary>
    private List<Buff> _BuffList;

    /// <summary>
    /// �� ���� �ý����� ������
    /// </summary>
    private GameObject _Owner;

    /// <summary>
    /// ���� ����Ʈ�� ���� �б� ���� ������Ƽ
    /// </summary>
    public List<Buff> buffList => _BuffList;

    /// <summary>
    /// ���� ���� �� ȣ��Ǵ� �̺�Ʈ
    /// </summary>
    public event System.Action<Buff /*�ش� ����*/> onBuffStarted;

    /// <summary>
    /// ���� ���� ��(�ٽ� ����� ��) ȣ��Ǵ� �̺�Ʈ
    /// </summary>
    public event System.Action<Buff /*�ش� ����*/> onBuffRenew;

    /// <summary>
    /// ������ ������Ʈ�� �� ȣ��Ǵ� �̺�Ʈ
    /// </summary>
    public event System.Action<Buff /*�ش� ����*/> onBuffUpdated;

    /// <summary>
    /// ������ ������ �� ȣ��Ǵ� �̺�Ʈ
    /// </summary>
    public event System.Action<Buff /*�ش� ����*/> onBuffFinished;

    /// <summary>
    /// �������Դϴ�.
    /// </summary>
    /// <param name="owner"> �� ���� �ý��� ��ü�� ������</param>
    public BuffSystem(GameObject owner)
    {
        // ��� ������ �ε��մϴ�.
        RegisterAllBuffs();   

        // ���� ����Ʈ�� �����մϴ�.
        _BuffList = new List<Buff>();

        // �����ָ� �����մϴ�.
        _Owner = owner;
    }

    /// <summary>
    /// ��� �������� ����ϴ� �޼����Դϴ�.
    /// </summary>
    private void RegisterAllBuffs()
    {
        if (_BuffDictionary == null)
        {
            _BuffDictionary = new Dictionary<int, Buff>
            {
                { 100000, new SampleSpeedChangeBuff(100000, null, BuffType.None, 0.2f, 10.0f) },
                { 100001,             new GiantBuff(100001, null, BuffType.Item, 10.0f, true) },
                { 100002,    new AreaDeploymentBuff(100002, null, BuffType.Item, 1.0f, true) },
                { 100003,        new MachineGunBuff(100003, null, BuffType.Item, 10.0f, true) },
                { 100004,             new ShellBuff(100004, null, BuffType.Item, 10.0f, true) },
                { 100005,         new ScarecrowBuff(100005, null, BuffType.Item, 10.0f, true) },
                { 100006,              new WindBuff(100006, null, BuffType.Item, 3.0f, true) },
            };
        }     
    }

    /// <summary>
    /// ���� �ڵ带 ���� �����ֿ��� ������ �ο��ϴ� �޼����Դϴ�.
    /// </summary>
    /// <param name="buffCode"> �ο��� ������ �ڵ�</param>
    public void AddBuff(int buffCode)
    {
        // �ش� ���� �ڵ尡 ���� ������ ���ٸ� �Լ� ȣ�� ����.
        if (!_BuffDictionary.ContainsKey(buffCode)) return;

        // ���� ���� ����Ʈ�� �ο��� ������ �ִ��� Ȯ���մϴ�.
        Buff newBuff = _BuffList.Find((Buff buffInList) => buffCode == buffInList.buffCode);

        // ���� ���� ����Ʈ�� �ο��� ������ �ִٸ�
        if (newBuff != null)
        {
            // �ش� ������ ���ŵ� �� ����Ǿ���ϴ� ������ �����մϴ�.
            newBuff.OnRenewBuff();
            onBuffRenew?.Invoke(newBuff);
        }
        // ���� ���� ����Ʈ�� �ο��� ������ ���ٸ�
        else
        {
            // ���� �������� ã�� �����Ͽ� �����ɴϴ�.
            newBuff = _BuffDictionary[buffCode].Clone(_Owner);

            // ������ �߰��մϴ�.
            _BuffList.Add(newBuff);

            // ������ �߰��� �� ����Ǿ���ϴ� ������ �����մϴ�.
            newBuff.OnStartBuff();            
            onBuffStarted?.Invoke(newBuff);
        }
    }

    /// <summary>
    /// ���� ����Ʈ�� ������Ʈ�ϴ� �޼����Դϴ�.
    /// Update �Լ����� ȣ�����ּ���.
    /// </summary>
    public void UpdateBuffList()
    {
        // ���� ������ ������ �������� ���� ����Ʈ ����
        List<Buff> finishedBuffList = new();

        // ����Ʈ�� �ִ� �������� ������Ʈ�մϴ�.
        foreach(Buff buff in _BuffList)
        {
            // �ش� ������ ������Ʈ�� �� ����Ǿ���ϴ� ������ �����մϴ�.
            buff.OnUpdateBuff();
            onBuffUpdated(buff);
            
            // ������ ���� ������ �����Ѵٸ�
            if(buff.isFinished)
            {
                // ������ ������ ��� ����Ʈ�� �ش� ������ �߰��մϴ�.
                finishedBuffList.Add(buff);
            }
        }

        // ���� ������ ������ �������� �����մϴ�.
        FinishBuffs(finishedBuffList);
    }

    /// <summary>
    /// �̹� ���� ����Ʈ�� �����۰� ���õ� ������ �����ϴ����� ��ȯ�ϴ� �޼����Դϴ�.
    /// </summary>
    /// <returns> ���� ����Ʈ�� �����۰� ���õ� ������ �����Ѵٸ� true, �ƴ϶�� false ��ȯ </returns>
    public bool IsOtherItemBuffActive()
    {
        return buffList.Exists((buff) => buff.buffType == BuffType.Item);
    }

    /// <summary>
    /// ���� ������ ������ �������� �����ϴ� �޼����Դϴ�.
    /// </summary>
    /// <param name="finishedBuffList"> ���� ������ ������ �������� ���� ����Ʈ</param>
    private void FinishBuffs(List<Buff> finishedBuffList)
    {
        foreach(Buff FinishedBuff in finishedBuffList)
        {
            // �ش� ������ ���� �� ����Ǿ���ϴ� ������ �����մϴ�.
            FinishedBuff.OnFinishedBuff();
            onBuffFinished?.Invoke(FinishedBuff);

            // �����մϴ�.
            _BuffList.Remove(FinishedBuff);
        }
    }

    /// <summary>
    /// ���� ����Ʈ�� �ʱ�ȭ�մϴ�.
    /// </summary>
    public void Clear()
    {
        foreach (Buff FinishedBuff in _BuffList)
        {
            // �ش� ������ ���� �� ����Ǿ���ϴ� ������ �����մϴ�.
            FinishedBuff.OnFinishedBuff();
            onBuffFinished?.Invoke(FinishedBuff);
        }

        _BuffList.Clear();
    }
}

/// <summary>
/// ������ ����
/// </summary>
public enum BuffType
{
    None,
    Item,   // �����۰� ���õ� ����
}

/// <summary>
/// �ϳ��� ������ ������ Ŭ�����Դϴ�.
/// ������ ������ �⺻���� ���� Ŭ������ �����Ͽ����ϴ�.
/// ������ ������ �ƴ� ������ ����� �ʹٸ� �������� maxStack �Ű������� 1�� �����Ͽ� �ִ� ������ 1�� �����ϼ���.( �Ű����� �⺻���� 1�� �����Ǿ��ֽ��ϴ�. )
/// </summary>
public abstract class Buff
{
    /// <summary>
    /// �� ������ ������
    /// </summary>
    protected GameObject _Owner;

    /// <summary>
    /// ���� �ڵ忡 ���� �ڵ� ���� ������Ƽ
    /// </summary>
    public int buffCode { get; protected set; }

    /// <summary>
    /// �ִ� ���ÿ� ���� �ڵ� ���� ������Ƽ
    /// </summary>
    public int maxStack { get; protected set; }

    /// <summary>
    /// ���� ���ÿ� ���� �ڵ� ���� ������Ƽ
    /// </summary>
    public int currentStack { get; protected set; }

    /// <summary>
    /// �� ������ UI�� ������ ���� ���� �ڵ� ���� ������Ƽ
    /// </summary>
    public bool visibility { get; protected set; }

    /// <summary>
    /// �� ������ �� ����� ������ ��Ÿ���� �б� ���� ������Ƽ�Դϴ�.
    /// </summary>
    public virtual bool notMuchLeft => false;

    /// <summary>
    /// �� ������ ����� ������ �����ߴ����� ���� �ڵ� ���� ������Ƽ�Դϴ�.
    /// </summary>
    public bool isFinished { get; protected set; }

    /// <summary>
    /// �� ������ ����
    /// </summary>
    public BuffType buffType { get; protected set; }

    /// <summary>
    /// �������Դϴ�.
    /// </summary>
    /// <param name="buffCode"> ������ ���� �ڵ�</param>
    /// <param name="owner"> ������ �� ������ ������</param>
    /// <param name="maxStack"> ������ �� ������ �ִ� ����</param>
    public Buff(int buffCode, GameObject owner, BuffType buffType, bool visibility = false, int maxStack = 1)
    {
        this.buffCode = buffCode;
        _Owner = owner;
        this.buffType = buffType;
        this.visibility = visibility;
        this.maxStack = maxStack;
    }

    /// <summary>
    /// �� ������ �����ϴ� �޼����Դϴ�.
    /// ���� �������� ã�� ������ �����ϴ� �뵵�� ����մϴ�.
    /// </summary>
    /// <param name="owner"> ������ ������ ������</param>
    /// <returns> ������ ���� ��ü</returns>
    public abstract Buff Clone(GameObject owner);    

    /// <summary>
    /// ������ ������Ű�� �޼����Դϴ�.
    /// </summary>
    protected virtual void IncreaseStack()
    {
        if (currentStack < maxStack) currentStack += 1;
    }   

    /// <summary>
    /// ������ ���۵� ���� ������ ��Ÿ���� �޼����Դϴ�.
    /// </summary>
    public void OnStartBuff()
    {
        // ���� ����
        IncreaseStack();

        // ���� ���� ���� ����
        onStartBuffContext();
    }

    /// <summary>
    /// ������ ���ŵ� ���� ������ ��Ÿ���� �޼����Դϴ�.
    /// </summary>
    public virtual void OnRenewBuff()
    {
        // ���� ����
        IncreaseStack();

        // ���� ���� ���� ����
        onRenewBuffContext();
    }

    /// <summary>
    /// ������ ������Ʈ�� ���� ������ ��Ÿ���� �޼����Դϴ�.
    /// </summary>
    public virtual void OnUpdateBuff()
    {
        // ���� ������Ʈ ���� ����
        onUpdateBuffContext();
    }

    /// <summary>
    /// ������ ���� ���� ������ ��Ÿ���� �޼����Դϴ�.
    /// </summary>
    public void OnFinishedBuff()
    {
        // ���� ���� ���� ����
        onFinishBuffContext();
    }

    // ===================== ���� ���� ȿ������ �� �߻� �޼��忡 ������Ű�ø� �˴ϴ�. ================================ //

    /// <summary>
    /// ���� ���� ���뿡 ���� �޼����Դϴ�.
    /// </summary>
    protected abstract void onStartBuffContext();

    /// <summary>
    /// ���� ���� ���뿡 ���� �޼����Դϴ�.
    /// </summary>
    protected abstract void onRenewBuffContext();

    /// <summary>
    /// ���� ������Ʈ ���� (�� �����Ӹ����� ����) �� ���� �޼����Դϴ�.
    /// </summary>
    protected abstract void onUpdateBuffContext();

    /// <summary>
    /// ���� ���� ���뿡 ���� �޼����Դϴ�.
    /// </summary>
    protected abstract void onFinishBuffContext();

    // ============================================================================================================== //
}

/// <summary>
/// �ð��� �����ϴ� ������ ���� Ŭ�����Դϴ�.
/// </summary>
public abstract class TimerBuff : Buff
{
    /// <summary>
    /// �������Դϴ�.
    /// </summary>
    /// <param name="buffCode"> ������ ���� �ڵ�</param>
    /// <param name="owner"> ������ �� ������ ������</param>
    /// <param name="buffTime"> ������ ���� �ִ� �ð�</param>
    /// <param name="maxStack"> ������ �� ������ �ִ� ����</param>
    public TimerBuff(int buffCode, GameObject owner, BuffType buffType, float buffTime, bool visibility = false, int maxStack = 1) : base(buffCode, owner, buffType, visibility, maxStack)
    {
        this.maxTime = buffTime;
        this.currentTime = buffTime;
    }

    /// <summary>
    /// ���� �ִ� �ð��� ��Ÿ���� �ڵ� ���� ������Ƽ�Դϴ�.
    /// </summary>
    public float maxTime { get; protected set; }

    /// <summary>
    /// ���� ���� ���� �ð��� ��Ÿ���� �ڵ� ���� ������Ƽ�Դϴ�.
    /// </summary>
    public float currentTime { get; protected set; }

    /// <summary>
    /// ������ �� �� ���Ҵ����� ���� �б� ���� ������Ƽ�Դϴ�.
    /// </summary>
    public override bool notMuchLeft => (currentTime < 5.0f && !isFinished);

    /// <summary>
    /// ���� �ð��� �����մϴ�.
    /// </summary>
    private void RenewBuffTimer()
    {
        currentTime = maxTime;
    }

    /// <summary>
    /// ���� �ð��� ������Ʈ�մϴ�.
    /// </summary>
    private void UpdateBuffTimer()
    {
        currentTime -= Time.deltaTime;

        if (currentTime <= 0.0f) isFinished = true;
    }

    /// <summary>
    /// ������ ���ŵ� ���� ������ ��Ÿ���� �޼����Դϴ�.
    /// </summary>
    public sealed override void OnRenewBuff()
    {
        base.OnRenewBuff();
        RenewBuffTimer();
    }

    /// <summary>
    /// ������ ������Ʈ�� ���� ������ ��Ÿ���� �޼����Դϴ�.
    /// </summary>
    public sealed override void OnUpdateBuff()
    {
        base.OnUpdateBuff();
        UpdateBuffTimer();       
    }
}

