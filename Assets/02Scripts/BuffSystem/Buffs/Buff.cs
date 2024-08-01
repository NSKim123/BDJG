using UnityEngine;

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