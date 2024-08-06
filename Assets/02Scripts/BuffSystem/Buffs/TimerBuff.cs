using UnityEngine;

/// <summary>
/// ���ӽð��� �����ϴ� ������ ���� Ŭ�����Դϴ�.
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
    public TimerBuff(int buffCode, GameObject owner, BuffType buffType, float buffTime, bool visibility = false, int maxStack = 1)
        : base(buffCode, owner, buffType, visibility, maxStack)
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


