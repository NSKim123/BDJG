using System;
using System.Collections.Generic;
using UnityEngine;

public class SampleSpeedChangeBuff : TimerBuff
{
    private float _ChangeRate;

    public SampleSpeedChangeBuff(int buffCode, GameObject owner, float changeRate, float maxTime, int maxStack = 1) : base(buffCode, owner, maxTime, maxStack)
    {
        this._ChangeRate = changeRate;
        visibility = true;
    }

    public override Buff Clone(GameObject owner)
    {
        return new SampleSpeedChangeBuff(buffCode, owner, _ChangeRate, maxTime);
    }

    protected override void onStartBuffContext()
    {
        Debug.Log("여기에는 버프가 시작될 때의 동작을 작성하세요.");
        Debug.Log($"버프 시작! {_Owner}의 이동 속도 {Mathf.Abs(_ChangeRate) * 100.0f} % {(_ChangeRate >= 0.0f ? "증가" : "감소") }");
    }

    protected override void onRenewBuffContext()
    {
        Debug.Log("여기에는 버프가 갱신될 때의 동작을 작성하세요.");
    }

    protected override void onUpdateBuffContext()
    {
        Debug.Log("여기에는 Update 이벤트가 호출될 때의 동작을 작성하세요.");
    }

    protected override void onFinishBuffContext()
    {
        Debug.Log("여기에는 이 버프가 해제될 때의 동작을 작성하세요.");
        Debug.Log($"버프 종료! {_Owner}의 이동 속도 {Mathf.Abs(_ChangeRate) * 100.0f} % {(_ChangeRate >= 0.0f ? "감소" : "증가")}");
    }
}


/*
    
    버프를 부여하는 예시를 보여드리자면,
        플레이어에게 이동속력을 10초 동안 20% 감소시키는 버프를 부여하고 싶다면...
          1) 미리 BuffSystem 클래스에서 _BuffDictionary 컨테이너에 <버프 코드, new SampleSpeedChangeBuff(null, 0.2f, 10.0f)> 를 추가하고 
                (BuffSystem 클래스의 LoadAllBuffs 메서드 참고),

          2) PlayerCharacter 의 AddBuff(버프 코드) 호출 
                (버프 코드를 반드시 기억해야 합니다.)                

         위 두 단계에 걸쳐 이동 속도 디버프를 부여할 수 있습니다.
    



    기능 종류에 따른 인터페이스 분리가 필요해보임!!

    버프의 효과를 나타내는 코드를 작성하는 부분은 Buff 클래스 내의 OnStartBuff, OnRenewBuff, OnUpdateBuff, OnFinishedBuff 메서드들입니다.
    즉, 이동속도 감소 코드를 SampleSpeedChangeBuff 내부에서 작성해야합니다.

    버프의 소유주를 나타내는 GameObject 타입의 _Owner 라는 멤버 변수가 있어 
    _Owner.GetComponent<PlayerCharacter>() 나 _Owner.GetComponent<Enemy>() 를 통해 이동 속도 감소를 적용하는 것도 한 가지 방법이지만,
    기능을 세분화하여 인터페이스로 정의한 다음 대상에게 인터페이스로 접근하는 것이 더 좋은 코드가 될 수도 있을 것 같습니다.
    
    
    예) IVelocity 라는 인터페이스를 정의하여, 채이좌가 정의한 클래스에 적용시키는 예시
        속도에 대한 프로퍼티, 속력에 대한 프로퍼티, 움직이는 메서드 등을 정의 ( 이 주석 밑에 인터페이스 정의한 예시를 참고! )
                
        Enemy 클래스에 IVelocity 인터페이스를 상속시키고
        
        인터페이스 구현을 다음과 같이 합니다.
        
        1. moveSpeed 프로퍼티는 NavMesh 의 속력과 연결
        public float moveSpeed => 
                { 
                    get => stateMachine.currentState.enemyAgent.speed;    
                    set => stateMachine.currentState.enemyAgent.speed = value;
                }   
        
        2. moveDirection 프로퍼티는 NavMesh 의 속도와 연결
        public Vector3 moveDirection => 
                { 
                    get => stateMachine.currentState.enemyAgent.velocity.normalized;    
                    set => stateMachine.currentState.enemyAgent.velocity = value * moveSpeed;
                }   

                <-- 지금 보니까 접근제한자 때문에 접근은 못하지만, 예시일 뿐이니까 편한대로 정의하시면 됩니다.
        



    이런 식으로 인터페이스를 구현하면 이동속도 감소 코드는 하단 코드와 같이 간결해질 수 있으며, 
    이동 속도를 가질 수 있는 모든 클래스(즉, IVelocity 인터페이스를 상속받은 클래스)에 
    버프를 활용해 이동속도를 조절시킬 수 있을 것이라 기대합니다.
    


    protected override void onStartBuffContext()
    {
        if(_Owner.TryGetComponent<IVelocity>(out IVelocity velocity)
        {
            velocity.moveSpeed *= (1.0f + _ChangeRate);
        }
    }

    protected override void onFinishBuffContext()
    {
        if(_Owner.TryGetComponent<IVelocity>(out IVelocity velocity)
        {
            velocity.moveSpeed /= (1.0f + _ChangeRate);
        }
    }
*/



public interface IMove
{
    public float moveSpeed { get; set; }

    public Vector3 moveDirection { get; set; }
}