using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 버프 리스트를 관리하는 클래스입니다.
/// </summary>
public class BuffSystem
{
    /// <summary>
    /// 모든 버프에 대한 정보를 저장하는 Dictionary (버프 사전)
    /// </summary>
    private static Dictionary<int /*버프 코드*/, Buff /*버프 코드에 맞는 버프 객체*/ > _BuffDictionary;

    /// <summary>
    /// 버프 리스트 객체
    /// </summary>
    private List<Buff> _BuffList;

    /// <summary>
    /// 이 버프 시스템의 소유주
    /// </summary>
    private GameObject _Owner;

    /// <summary>
    /// 버프 리스트에 대한 읽기 전용 프로퍼티
    /// </summary>
    public List<Buff> buffList => _BuffList;

    /// <summary>
    /// 버프 시작 시 호출되는 이벤트
    /// </summary>
    public event System.Action<Buff /*해당 버프*/> onBuffStarted;

    /// <summary>
    /// 버프 갱신 시(다시 얻었을 때) 호출되는 이벤트
    /// </summary>
    public event System.Action<Buff /*해당 버프*/> onBuffRenew;

    /// <summary>
    /// 버프가 업데이트될 때 호출되는 이벤트
    /// </summary>
    public event System.Action<Buff /*해당 버프*/> onBuffUpdated;

    /// <summary>
    /// 버프가 끝났을 때 호출되는 이벤트
    /// </summary>
    public event System.Action<Buff /*해당 버프*/> onBuffFinished;

    /// <summary>
    /// 생성자입니다.
    /// </summary>
    /// <param name="owner"> 이 버프 시스템 객체의 소유주</param>
    public BuffSystem(GameObject owner)
    {
        // 모든 버프를 로드합니다.
        RegisterAllBuffs();   

        // 버프 리스트를 생성합니다.
        _BuffList = new List<Buff>();

        // 소유주를 저장합니다.
        _Owner = owner;
    }

    /// <summary>
    /// 모든 버프들을 등록하는 메서드입니다.
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
    /// 버프 코드를 통해 소유주에게 버프를 부여하는 메서드입니다.
    /// </summary>
    /// <param name="buffCode"> 부여할 버프의 코드</param>
    public void AddBuff(int buffCode)
    {
        // 해당 버프 코드가 버프 사전에 없다면 함수 호출 종료.
        if (!_BuffDictionary.ContainsKey(buffCode)) return;

        // 현재 버프 리스트에 부여할 버프가 있는지 확인합니다.
        Buff newBuff = _BuffList.Find((Buff buffInList) => buffCode == buffInList.buffCode);

        // 현재 버프 리스트에 부여할 버프가 있다면
        if (newBuff != null)
        {
            // 해당 버프가 갱신될 때 실행되어야하는 동작을 실행합니다.
            newBuff.OnRenewBuff();
            onBuffRenew?.Invoke(newBuff);
        }
        // 현재 버프 리스트에 부여할 버프가 없다면
        else
        {
            // 버프 사전에서 찾아 복제하여 가져옵니다.
            newBuff = _BuffDictionary[buffCode].Clone(_Owner);

            // 버프를 추가합니다.
            _BuffList.Add(newBuff);

            // 버프가 추가될 때 실행되어야하는 동작을 실행합니다.
            newBuff.OnStartBuff();            
            onBuffStarted?.Invoke(newBuff);
        }
    }

    /// <summary>
    /// 버프 리스트를 업데이트하는 메서드입니다.
    /// Update 함수에서 호출해주세요.
    /// </summary>
    public void UpdateBuffList()
    {
        // 삭제 조건을 충족한 버프들을 담을 리스트 생성
        List<Buff> finishedBuffList = new();

        // 리스트에 있는 버프들을 업데이트합니다.
        foreach(Buff buff in _BuffList)
        {
            // 해당 버프가 업데이트될 때 실행되어야하는 동작을 실행합니다.
            buff.OnUpdateBuff();
            onBuffUpdated(buff);
            
            // 버프가 끝날 조건을 만족한다면
            if(buff.isFinished)
            {
                // 삭제할 버프를 담는 리스트에 해당 버프를 추가합니다.
                finishedBuffList.Add(buff);
            }
        }

        // 삭제 조건을 충족한 버프들을 삭제합니다.
        FinishBuffs(finishedBuffList);
    }

    /// <summary>
    /// 이미 버프 리스트에 아이템과 관련된 버프가 존재하는지를 반환하는 메서드입니다.
    /// </summary>
    /// <returns> 버프 리스트에 아이템과 관련된 버프가 존재한다면 true, 아니라면 false 반환 </returns>
    public bool IsOtherItemBuffActive()
    {
        return buffList.Exists((buff) => buff.buffType == BuffType.Item);
    }

    /// <summary>
    /// 삭제 조건을 충족한 버프들을 삭제하는 메서드입니다.
    /// </summary>
    /// <param name="finishedBuffList"> 삭제 조건을 충족한 버프들을 담은 리스트</param>
    private void FinishBuffs(List<Buff> finishedBuffList)
    {
        foreach(Buff FinishedBuff in finishedBuffList)
        {
            // 해당 버프가 끝날 때 실행되어야하는 동작을 실행합니다.
            FinishedBuff.OnFinishedBuff();
            onBuffFinished?.Invoke(FinishedBuff);

            // 삭제합니다.
            _BuffList.Remove(FinishedBuff);
        }
    }

    /// <summary>
    /// 버프 리스트를 초기화합니다.
    /// </summary>
    public void Clear()
    {
        foreach (Buff FinishedBuff in _BuffList)
        {
            // 해당 버프가 끝날 때 실행되어야하는 동작을 실행합니다.
            FinishedBuff.OnFinishedBuff();
            onBuffFinished?.Invoke(FinishedBuff);
        }

        _BuffList.Clear();
    }
}

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
/// 스택형 버프가 아닌 버프를 만들고 싶다면 생성자의 maxStack 매개변수에 1을 전달하여 최대 스택을 1로 설정하세요.( 매개변수 기본값도 1로 설정되어있습니다. )
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

/// <summary>
/// 시간이 존재하는 버프에 대한 클래스입니다.
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
    public TimerBuff(int buffCode, GameObject owner, BuffType buffType, float buffTime, bool visibility = false, int maxStack = 1) : base(buffCode, owner, buffType, visibility, maxStack)
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

