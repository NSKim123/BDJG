using UnityEngine;

/// <summary>
/// 버프의 종류
/// </summary>
public enum BuffType
{
    None,
    Item,   // 아이템과 관련된 버프
}

/// <summary>
/// 하나의 버프를 정의한 클래스입니다.
/// 스택형 버프를 기본적인 버프 클래스로 정의하였습니다.
/// </summary>
public abstract class Buff
{
    /// <summary>
    /// 이 버프의 소유주
    /// </summary>
    protected GameObject _Owner;

    /// <summary>
    /// 버프 코드에 대한 자동 구현 프로퍼티
    /// </summary>
    public int buffCode { get; protected set; }

    /// <summary>
    /// 최대 스택에 대한 자동 구현 프로퍼티
    /// </summary>
    public int maxStack { get; protected set; }

    /// <summary>
    /// 현재 스택에 대한 자동 구현 프로퍼티
    /// </summary>
    public int currentStack { get; protected set; }

    /// <summary>
    /// 이 버프가 UI로 보여질 지에 대한 자동 구현 프로퍼티
    /// </summary>
    public bool visibility { get; protected set; }

    /// <summary>
    /// 이 버프가 곧 사라질 조건을 나타내는 읽기 전용 프로퍼티입니다.
    /// </summary>
    public virtual bool notMuchLeft => false;

    /// <summary>
    /// 이 버프가 사라질 조건을 만족했는지에 대한 자동 구현 프로퍼티입니다.
    /// </summary>
    public bool isFinished { get; protected set; }

    /// <summary>
    /// 이 버프의 종류
    /// </summary>
    public BuffType buffType { get; protected set; }

    /// <summary>
    /// 생성자입니다.
    /// </summary>
    /// <param name="buffCode"> 설정할 버프 코드</param>
    /// <param name="owner"> 설정할 이 버프의 소유주</param>
    /// <param name="maxStack"> 설정할 이 버프의 최대 스택</param>
    public Buff(int buffCode, GameObject owner, BuffType buffType, bool visibility = false, int maxStack = 1)
    {
        this.buffCode = buffCode;
        _Owner = owner;
        this.buffType = buffType;
        this.visibility = visibility;
        this.maxStack = maxStack;
    }

    /// <summary>
    /// 이 버프를 복제하는 메서드입니다.
    /// 버프 사전에서 찾은 버프를 복제하는 용도로 사용합니다.
    /// </summary>
    /// <param name="owner"> 설정할 버프의 소유주</param>
    /// <returns> 복제된 버프 객체</returns>
    public abstract Buff Clone(GameObject owner);

    /// <summary>
    /// 스택을 증가시키는 메서드입니다.
    /// </summary>
    protected virtual void IncreaseStack()
    {
        if (currentStack < maxStack) currentStack += 1;
    }

    /// <summary>
    /// 버프가 시작될 때의 동작을 나타내는 메서드입니다.
    /// </summary>
    public void OnStartBuff()
    {
        // 스택 증가
        IncreaseStack();

        // 버프 시작 내용 실행
        onStartBuffContext();
    }

    /// <summary>
    /// 버프가 갱신될 때의 동작을 나타내는 메서드입니다.
    /// </summary>
    public virtual void OnRenewBuff()
    {
        // 스택 증가
        IncreaseStack();

        // 버프 갱신 내용 실행
        onRenewBuffContext();
    }

    /// <summary>
    /// 버프가 업데이트될 때의 동작을 나타내는 메서드입니다.
    /// </summary>
    public virtual void OnUpdateBuff()
    {
        // 버프 업데이트 내용 실행
        onUpdateBuffContext();
    }

    /// <summary>
    /// 버프가 끝날 때의 동작을 나타내는 메서드입니다.
    /// </summary>
    public void OnFinishedBuff()
    {
        // 버프 해제 내용 실행
        onFinishBuffContext();
    }

    // ===================== 실제 버프 효과들은 이 추상 메서드에 구현시키시면 됩니다. ================================ //

    /// <summary>
    /// 버프 시작 내용에 대한 메서드입니다.
    /// </summary>
    protected abstract void onStartBuffContext();

    /// <summary>
    /// 버프 갱신 내용에 대한 메서드입니다.
    /// </summary>
    protected abstract void onRenewBuffContext();

    /// <summary>
    /// 버프 업데이트 내용 (매 프레임마다의 내용) 에 대한 메서드입니다.
    /// </summary>
    protected abstract void onUpdateBuffContext();

    /// <summary>
    /// 버프 해제 내용에 대한 메서드입니다.
    /// </summary>
    protected abstract void onFinishBuffContext();

    // ============================================================================================================== //
}