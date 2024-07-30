using System;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 플레이어의 캐릭터의 이동, 점프, 피격(밀려남)을 수행하는 컴포넌트입니다.
/// </summary>
/// 이동 부분 + 유니티 이벤트 함수 부분
public partial class PlayerMovement : MonoBehaviour
{
    [Header("# 이동 관련")]
    [Header("이동 속력")]
    public float m_Speed = 3.5f;

    [Header("회전 속력")]
    public float m_RotateSpeedInDegree = 30.0f;

    [Header("------------------------------------------------------------------------------")]
        
    /// <summary>
    /// 목표 Yaw 회전값을 나타냅니다.
    /// </summary>
    private float _TargetYawAngle;

    /// <summary>
    /// 이동이 가능한지를 나타냅니다.
    /// </summary>
    private bool _AbleToMove = true;    

    /// <summary>
    /// 입력받은 이동 방향을 저장하는 멤버변수입니다.
    /// </summary>
    private Vector2 _InputDirection;

    /// <summary>
    /// 목표 속도 벡터 입니다.
    /// </summary>
    private Vector3 _TargetVelocity;
    
    /// <summary>
    /// CharacterController 컴포넌트 객체입니다.
    /// </summary>
    private CharacterController _CharacterController;

    /// <summary>
    /// 이 이동 컴포넌트를 가지고 있는 PlayerCharacter 객체입니다.
    /// </summary>
    private PlayerCharacter _OwnerCharacter;

    /// <summary>
    /// ZX 평면 상에서의 정규화된 속력(속력 / 최대 속력)을 나타내는 읽기 전용 프로퍼티입니다.
    /// </summary>
    public float normalizedZXSpeed => Mathf.Sqrt(((_TargetVelocity).x * (_TargetVelocity).x + (_TargetVelocity).z * (_TargetVelocity).z)) / m_Speed;

    /// <summary>
    /// CharacterController 컴포넌트에 대한 읽기 전용 프로퍼티입니다.
    /// </summary>
    public CharacterController characterController => _CharacterController ?? (_CharacterController = GetComponent<CharacterController>());

    private void Awake()
    {
        // 점프 게이지 객체를 생성합니다.
        _JumpReuseTimeGauge = new FloatGauge(m_JumpReuseTime);

        //이 이동 컴포넌트를 가지고 있는 PlayerCharacter 객체를 찾습니다.
        _OwnerCharacter = GetComponent<PlayerCharacter>();
    }

    private void Update()
    {
        // 점프 쿨타임을 갱신합니다.
        UpdateJumpReuseTimeGauge();
    }

    private void FixedUpdate()
    {
        // XZ 평면 상의 속도를 계산합니다.
        CalculateXZVelocity();

        // 목표 회전값을 계산합니다.
        CalculateTargetYawAngle();

        // Y 축 상의 속도를 계산합니다.
        CalculateYVelocity();        

        // 계산한 속도를 토대로 이동시킵니다.
        Move();

        // 목표 회전값을 향해 회전합니다.
        Rotation();

        // 넉백 속도가 있다면 넉백를 적용합니다.
        KnockBack();
    }

    /// <summary>
    /// 목표 회전값을 계산하는 메서드입니다.
    /// </summary>
    private void CalculateTargetYawAngle()
    {
        // 이동 입력을 받지 않은 상태라면 목표 회전값 설정을 건너뜁니다.
        if(_InputDirection == Vector2.zero) return;

        // 목표 회전값을 설정합니다.
        _TargetYawAngle = Mathf.Atan2(_InputDirection.x, _InputDirection.y) * Mathf.Rad2Deg;        
    }

    /// <summary>
    /// XZ 평면 상의 속도를 계산하는 메서드입니다.
    /// </summary>
    private void CalculateXZVelocity()
    {
        // 새로 설정할 속도 벡터
        Vector3 newTargetVelocity = Vector3.zero;

        // 이동할 수 있는 상태라면 속도 벡터를 설정합니다.
        if (_AbleToMove)
        {
            // 입력받은 이동 방향을 새로 설정할 속도 벡터에 저장합니다.
            newTargetVelocity += Vector3.forward * _InputDirection.y;
            newTargetVelocity += Vector3.right * _InputDirection.x;
            newTargetVelocity.Normalize();
            newTargetVelocity *= m_Speed;
        }            

        // 목표 속도 벡터에 저장합니다.
        _TargetVelocity.x = newTargetVelocity.x;
        _TargetVelocity.z = newTargetVelocity.z;
    }

    /// <summary>
    /// Y 축 상의 속도를 계산하는 메서드입니다.
    /// </summary>
    private void CalculateYVelocity()
    {
        // 땅에 닿아있는지 체크합니다.
        _IsGrounded = CheckGrounded();

        // 적 머리 위에 위에 있는지 체크합니다.
        _IsAboveEnemy = CheckAboveEnemy();

        // 중력값
        float gravity = Physics.gravity.y * Time.fixedDeltaTime * 3.0f;

        // 땅에 닿아있다면 y 축 방향에 대한 속도를 0으로 설정합니다.
        if(_IsGrounded)
        {
            _TargetVelocity.y = 0.0f;
        }
        // 땅에 닿아있지 않다면 y 축 방향에 대한 속도에 중력값을 더합니다.
        else
        {
            _TargetVelocity.y += gravity;
        }

        // 만약 점프 트리거가 활성화되어있는 상태라면 y 축 방향에 대한 속도값을 설정해 점프를 실시합니다.
        if (_IsJumpTrigger)
        {
            Jump();
        }
    }

    /// <summary>
    /// 계산한 속도를 토대로 이동시키는 메서드입니다.
    /// </summary>
    private void Move()
    {
        // 이동이 가능한 상태라면 이동시킵니다.
        if(_AbleToMove)
            characterController.Move(_TargetVelocity * Time.fixedDeltaTime);

        // 그렇지 않다면 Y 축 상의 이동만 실행시킵니다.
        else
            characterController.Move(Vector3.up * _TargetVelocity.y * Time.fixedDeltaTime);
    }

    /// <summary>
    /// 목표 회전값을 향해 회전하는 메서드입니다.
    /// </summary>
    private void Rotation()
    {        
        // 목표 회전값을 계산합니다.
        Quaternion targetRotation = Quaternion.Euler(0.0f, _TargetYawAngle, 0.0f);

        // 부드럽게 회전하도록 계산합니다.
        Quaternion newRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 720.0f * Time.fixedDeltaTime);

        // 생성한 회전값을 통해 회전시킵니다.
        transform.rotation = newRotation;
    }   
    
    /// <summary>
    /// Movement 컴포넌트를 리셋하는 동작을 정의한 메서드입니다.
    /// </summary>
    /// <param name="initPosition"></param>
    public void ResetMovementComponent(Vector3 initPosition)
    {
        // 넉백 속도를 영벡터로 초기화합니다.
        _KnockBackVelocity = Vector3.zero;

        // 게임 시작 시의 위치로 이동시킵니다.
        // 한 번의 Move 함수 호출로는 의도한 위치로 가지 않는 경우가 있어, 여러 차례 Move 함수를 호출하였습니다.
        for (int i = 0; i < 5; ++i)
        {
            characterController.Move(initPosition - transform.position);
        }
    }

    /// <summary>
    /// 이동 가능 여부를 설정하는 메서드입니다.
    /// </summary>
    /// <param name="movable"> 이동 가능은 참, 이동 불가능은 거짓</param>
    public void SetMovable(bool movable)
    {
        _AbleToMove = movable;
    }

    /// <summary>
    /// 이동 입력을 받을 시 호출되는 메서드입니다.
    /// 입력받은 이동 방향 값을 저장합니다.
    /// </summary>
    /// <param name="inputDirection"> 입력 방향</param>
    public void OnMoveInput(Vector2 inputDirection)
    {
        _InputDirection = inputDirection;
    }    

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        GUIContent gUIContent = new GUIContent();
        gUIContent.text = $"\n\n\n\n\n\n\n\n방어력 / 최대 방어력: {_Defence} / {_MaxDefence}";
        Handles.Label(transform.position + Vector3.down, gUIContent);
    }
#endif
}


/// <summary>
/// 점프 관련 부분
/// </summary>
public partial class PlayerMovement
{
    private static string SOUNDNAME_GIANTSLIME_LAND = "Effect_Slime_GiantLand";
    private static string SOUNDNAME_SLIME_JUMP = "Effect_Slime_Jump";

    [Header("# 점프 관련")]
    [Header("점프력")]
    public float m_JumpForce = 15.0f;

    [Header("점프 쿨타임")]
    public float m_JumpReuseTime = 1.0f;

    [Header("# 거대화 슬라임일때 착지 시 이펙트")]
    public GameObject m_Effect_GiantSlimeLand;

    [Header("------------------------------------------------------------------------------")]

    /// <summary>
    /// 점프 트리거가 활성화되어있는지를 나타냅니다.
    /// </summary>
    private bool _IsJumpTrigger = false;

    /// <summary>
    /// 땅에 닿아있는지를 나타냅니다.
    /// </summary>
    private bool _IsGrounded = false;

    /// <summary>
    /// 적 머리 위에 있는지를 나타냅니다.
    /// </summary>
    private bool _IsAboveEnemy = false;

    /// <summary>
    /// 현재 점프에 적용해야하는 점프력의 크기입니다.
    /// </summary>
    private float _CurrentJumpForce;

    /// <summary>
    /// 점프 쿨타임 게이지 객체
    /// </summary>
    private FloatGauge _JumpReuseTimeGauge;

    /// <summary>
    /// 점프 버튼 활성화 조건을 나타내는 읽기 전용 프로퍼티입니다.
    /// </summary>
    public bool isJumpable => CheckJumpable();

    /// <summary>
    /// 땅에 닿아있는지에 대한 읽기 전용 프로퍼티입니다.
    /// </summary>
    public bool isGrounded => _IsGrounded;

    /// <summary>
    /// 점프 쿨타임 게이지 객체에 대한 읽기 전용 프로파티입니다.
    /// </summary>
    public FloatGauge jumpResueTimeGauge => _JumpReuseTimeGauge;

    /// <summary>
    /// 땅에 닿아있는지를 체크하는 메서드입니다.
    /// </summary>
    /// <returns> 땅에 닿아있다면 참을 반환합니다.</returns>
    private bool CheckGrounded()
    {
        // Box cast 시작 지점 설정
        Vector3 center = characterController.center + transform.position;

        // Box cast 크기 설정
        Vector3 half = new Vector3(1.0f, 0.0f, 1.0f) * characterController.radius * transform.lossyScale.y;

        // Box cast 최대 길이 설정
        float maxDistance = (characterController.height * 0.5f + characterController.skinWidth) * transform.lossyScale.y;

        // Box cast 실행
        bool result = Physics.BoxCast(
                            center,
                            half,
                            Vector3.down,
                            out RaycastHit hitResult,
                            Quaternion.identity,
                            maxDistance,
                            1 << LayerMask.NameToLayer("Ground"));

        // 땅을 감지하면 땅에 박혀있는 길이만큼 y축 방향으로 이동시켜줍니다.
        if (result)
        {
            characterController.Move(Vector3.up * (maxDistance - hitResult.distance));
        }

        return result;
    }

    /// <summary>
    /// 적 머리 위에 있는지를 체크하는 메서드입니다.
    /// </summary>
    /// <returns> 적 머리 위에 있다면 참을 반환합니다.</returns>
    private bool CheckAboveEnemy()
    {
        // 만약 CharacterController 의 감지 제외 레이어에 Enemy 레이어가 있다면 이 체크 과정을 건너뜁니다.
        if ( ((LayerMask.GetMask("Enemy")) & characterController.excludeLayers.value) > 0) return false;

        // Box cast 시작 지점 설정
        Vector3 center = characterController.center + transform.position;

        // Box cast 크기 설정
        Vector3 half = new Vector3(1.0f, 0.0f, 1.0f) * characterController.radius * transform.lossyScale.y * 0.35f;

        // Box cast 최대 길이 설정 + 캐릭터의 맨 밑부분까지만 검사합니다.
        float maxDistance = (characterController.height * 0.5f + characterController.skinWidth) * transform.lossyScale.y;

        // Box cast 실행
        bool result = Physics.BoxCast(
                        center,
                        half,
                        Vector3.down,
                        out RaycastHit hitResult,
                        Quaternion.identity,
                        maxDistance,
                        1 << LayerMask.NameToLayer("Enemy"));

        // 만약 적이 감지되었다면, 강도가 작은 강제 점프를 하도록 설정합니다.
        if (result)
        {
            // 점프력 설정
            _CurrentJumpForce = m_JumpForce * 0.6f;

            // 검프 프리거 강제 설정
            _IsJumpTrigger = true;            
        }

        // 결과 반환
        return result;
    }

    /// <summary>
    /// 점프 쿨타임을 갱신하는 메서드입니다.
    /// </summary>
    private void UpdateJumpReuseTimeGauge()
    {
        // 이미 쿨타임이 다 돌았다면 함수 호출 종료
        if (_JumpReuseTimeGauge.currentValue == _JumpReuseTimeGauge.min) return;

        // 쿨타임 감소
        _JumpReuseTimeGauge.currentValue -= Time.deltaTime;
    }

    /// <summary>
    /// 점프 입력인 가능한 조건인지 체크하는 메서드입니다.
    /// </summary>
    /// <returns></returns>
    private bool CheckJumpable()
    {
        return !isKnockBack && !CheckStunned() && !CheckDead();
    }

    /// <summary>
    /// 점프 가능 상태를 확인하고 점프 입력 받았음을 저장하는 메서드입니다.
    /// </summary>
    private void TryJump()
    {
        // 땅에 닿아있지 않거나 쿨타임이 남아있는 상태라면 호출을 종료합니다.
        if (!_IsGrounded || !(_JumpReuseTimeGauge.ratio == 0.0f)) return;

        // 점프 입력이 불가능한 상황이라면 호출을 종료합니다.
        if (!CheckJumpable()) return;

        // 받았음을 저장합니다.
        _IsJumpTrigger = true;

        // 점프력 설정
        _CurrentJumpForce = m_JumpForce;
    }

    /// <summary>
    /// 점프 시키는 메서드입니다.
    /// </summary>
    private void Jump()
    {
        // 점프 시킵니다.
        _TargetVelocity.y = _CurrentJumpForce;

        // 점프 트리거 해제
        _IsJumpTrigger = false;

        // 점프 애니메이션을 실행
        _OwnerCharacter.animController.TriggerJumpParam();

        // 쿨타임을 설정
        _JumpReuseTimeGauge.currentValue = _JumpReuseTimeGauge.max;

        // 점프 사운드 플레이
        SoundManager.Instance.PlaySound(SOUNDNAME_SLIME_JUMP, SoundType.Effect);
    }

    /// <summary>
    /// 거대화 슬라임의 착지 이펙트를 발생시키는 메서드입니다.
    /// </summary>
    public void InstantiateGiantSlimeLandEffect()
    {
        GameObject effect = Instantiate(m_Effect_GiantSlimeLand);

        effect.transform.position = transform.position + Vector3.down * characterController.height / 2.0f * transform.lossyScale.y;
    }

    /// <summary>
    /// 거대화 슬라임의 착지 사운드를 재생하는 메서드입니다.
    /// </summary>
    public void PlayGiantSlimeLandSound()
    {
        SoundManager.Instance.PlaySound(SOUNDNAME_GIANTSLIME_LAND, SoundType.Effect, 2.0f);
    }

    /// <summary>
    /// 점프 입력을 받을 시 호출되는 메서드입니다.
    /// </summary>
    public void OnJumpInput()
    {
        TryJump();
    }
}


/// <summary>
/// 피격 관련
/// </summary>
public partial class PlayerMovement
{
    private static string SOUNDNAME_DAMAGED = "Effect_Damaged";

    [Header("# 피격 관련")]
    [Header("넉백 계수(넉백 계산식에서 데미지값에 곱해지는 수)")]
    public float m_KnockBackCoefficient = 5.0f;

    /// <summary>
    /// 방어력
    /// </summary>
    private float _Defence;

    /// <summary>
    /// 최대 방어력
    /// </summary>
    private float _MaxDefence;

    /// <summary>
    /// 피격 면역 상태를 나타냅니다.
    /// </summary>
    private bool _IsImmune;

    /// <summary>
    /// 넉백 속도
    /// </summary>
    private Vector3 _KnockBackVelocity;

    /// <summary>
    /// 밀려나고 있는지에 대한 읽기 전용 프로퍼티입니다.
    /// </summary>
    public bool isKnockBack => _KnockBackVelocity.sqrMagnitude > 0.0f;

    /// <summary>
    /// 데미지를 계산하는 메서드입니다. (데미지 = 상대 공격력 - 내 방어력)
    /// </summary>
    /// <param name="attack"> 받은 공격력</param>
    /// <returns> 결과 데미지</returns>
    private float CalculateDamage(float attack)
    {
        // 데미지 = 상대 공격력 - 내 방어력
        float damage = attack - _Defence;

        return (damage >= 0.0f) ? damage : 0.0f;
    }

    /// <summary>
    /// 넉백 속도를 계산하여 넉백을 적용하는 메서드입니다.
    /// </summary>
    private void KnockBack()
    {
        // 넉백 상태가 아니라면 호출 종료합니다.
        if (!isKnockBack) return;

        // 넉백 속도를 적용하여 객체를 이동시킵니다.
        _CharacterController.Move(_KnockBackVelocity * Time.fixedDeltaTime);

        // 넉백 속도를 점차 줄입니다.
        _KnockBackVelocity = Vector3.MoveTowards(_KnockBackVelocity, Vector3.zero, 100.0f * Time.fixedDeltaTime);
        if (_KnockBackVelocity.magnitude < 0.1f) _KnockBackVelocity = Vector3.zero;
    }

    /// <summary>
    /// 행동 불가 상태인지 체크하는 메서드입니다.
    /// </summary>
    /// <returns></returns>
    private bool CheckStunned()
    {
        return _OwnerCharacter.isStunned;
    }

    /// <summary>
    /// 죽은 상태인지 체크하는 메서드입니다.
    /// </summary>
    /// <returns></returns>
    private bool CheckDead()
    {
        return _OwnerCharacter.isDead;
    }    

    /// <summary>
    /// 방어력을 갱신하는 메서드입니다.
    /// </summary>
    /// <param name="ratio"> 적용할 최대 방어력의 비율</param>
    public void UpdateDefence(float ratio)
    {
        _Defence = _MaxDefence * ratio;
    }
    
    /// <summary>
    /// 면역 상태를 설정하는 메서드입니다.
    /// </summary>
    /// <param name="immune"> 설정할 면역 상태</param>
    public void SetImmuneState(bool immune)
    {
        _IsImmune = immune;
    }

    /// <summary>
    /// 레벨업 시 호출될 메서드입니다.
    /// </summary>
    /// <param name="level"> 달성 레벨</param>
    public void OnLevelUp(int level)
    {
        // 최대 방어력 설정
        _MaxDefence = 10.0f + level * 2.0f;
    }

    /// <summary>
    /// 몬스터에게 맞았을 때 호출되는 메서드입니다.
    /// </summary>
    /// <param name="distance"> 밀려날 거리</param>
    /// <param name="direction"> 밀려날 방향</param>
    public void OnHit(float distance, Vector3 direction)
    {
        // 면역 상태일 경우 함수 종료
        if (_IsImmune) return;

        // 데미지 계산
        float damage = CalculateDamage(distance);        

        // 넉백 속도 계산 ( 최종 넉백량 = 데미지량 * 넉백 계수 )
        _KnockBackVelocity = (damage * m_KnockBackCoefficient) * direction;

        // 피격 애니메이션을 실행합니다.
        if(damage > 0.0f)
        {
            _OwnerCharacter.animController.TriggerDamagedParam();
            SoundManager.Instance.PlaySound(SOUNDNAME_DAMAGED, SoundType.Effect);
        }
    }
}



/// <summary>
/// 아이템 효과 관련 부분
/// </summary>
public partial class PlayerMovement
{
    /// <summary>
    /// 거대화 사용 시작시 Movement 컴포넌트에서 수행되어야하는 내용
    /// </summary>
    public void OnStartGiant()
    {
        // 면역 상태 설정
        SetImmuneState(true);

        // 적과의 충돌을 무시합니다.
        characterController.excludeLayers += LayerMask.GetMask("Enemy");
    }

    /// <summary>
    /// 거대화 사용 종료시 Movement 컴포넌트에서 수행되어야하는 내용
    /// </summary>
    public void OnFinishGiant()
    {
        // 면역 상태 해제
        SetImmuneState(false);

        // 적 충돌 무시 해제
        characterController.excludeLayers -= LayerMask.GetMask("Enemy");
    }

    /// <summary>
    /// 기관총 사용 시작시 Movement 컴포넌트에서 수행되어야하는 내용
    /// </summary>
    public void OnStartMachineGun()
    {
        // 적과의 충돌을 무시합니다.
        characterController.excludeLayers += LayerMask.GetMask("Enemy");
    }

    /// <summary>
    /// 기관총 아이템 사용 중 프레임마다 Movement 컴포넌트에서 수행되어야하는 내용
    /// </summary>
    public void OnUpdateMachineGun()
    {
        // 움직임 제한
        SetMovable(false);

        // 면역 상태 설정
        SetImmuneState(true);
    }

    /// <summary>
    /// 기관총 사용 종료시 Movement 컴포넌트에서 수행되어야하는 내용
    /// </summary>
    public void OnFinishMachineGun()
    {
        // 움직임 제한 해제
        SetMovable(true);

        // 면역 상태 해제
        SetImmuneState(false);

        // 적 충돌 무시 해제
        characterController.excludeLayers -= LayerMask.GetMask("Enemy");
    }
}


