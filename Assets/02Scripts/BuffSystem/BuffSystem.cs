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
        // ���� ������ ��� ������ ����մϴ�.
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





