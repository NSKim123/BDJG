using UnityEngine;

/// <summary>
/// 지속시간이 존재하는 버프에 대한 클래스입니다.
/// </summary>
public abstract class TimerBuff : Buff
{
    /// <summary>
    /// 생성자입니다.
    /// </summary>
    /// <param name="buffCode"> 설정할 버프 코드</param>
    /// <param name="owner"> 설정할 이 버프의 소유주</param>
    /// <param name="buffTime"> 설정할 버프 최대 시간</param>
    /// <param name="maxStack"> 설정할 이 버프의 최대 스택</param>
    public TimerBuff(int buffCode, GameObject owner, BuffType buffType, float buffTime, bool visibility = false, int maxStack = 1)
        : base(buffCode, owner, buffType, visibility, maxStack)
    {
        this.maxTime = buffTime;
        this.currentTime = buffTime;
    }

    /// <summary>
    /// 버프 최대 시간을 나타내는 자동 구현 프로퍼티입니다.
    /// </summary>
    public float maxTime { get; protected set; }

    /// <summary>
    /// 현재 남은 버프 시간을 나타내는 자동 구현 프로퍼티입니다.
    /// </summary>
    public float currentTime { get; protected set; }

    /// <summary>
    /// 버프가 얼마 안 남았는지에 대한 읽기 전용 프로퍼티입니다.
    /// </summary>
    public override bool notMuchLeft => (currentTime < 5.0f && !isFinished);

    /// <summary>
    /// 버프 시간을 갱신합니다.
    /// </summary>
    private void RenewBuffTimer()
    {
        currentTime = maxTime;
    }

    /// <summary>
    /// 버프 시간을 업데이트합니다.
    /// </summary>
    private void UpdateBuffTimer()
    {
        currentTime -= Time.deltaTime;

        if (currentTime <= 0.0f) isFinished = true;
    }

    /// <summary>
    /// 버프가 갱신될 때의 동작을 나타내는 메서드입니다.
    /// </summary>
    public sealed override void OnRenewBuff()
    {
        base.OnRenewBuff();
        RenewBuffTimer();
    }

    /// <summary>
    /// 버프가 업데이트될 때의 동작을 나타내는 메서드입니다.
    /// </summary>
    public sealed override void OnUpdateBuff()
    {
        base.OnUpdateBuff();
        UpdateBuffTimer();
    }
}


