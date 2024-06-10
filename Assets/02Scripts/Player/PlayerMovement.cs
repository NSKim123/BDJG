using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 플레이어의 캐릭터의 이동을 수행하는 컴포넌트입니다.
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    [Header("# 이동 관련")]
    [Header("이동 속력")]
    public float m_Speed = 5.0f;

    [Header("회전 속력")]
    public float m_RotateSpeedInDegree = 30.0f;

    [Header("------------------------------------------------------------------------------")]

    [Header("# 점프 관련")]
    [Header("점프력")]
    public float m_JumpForce = 5.0f;

    [Header("------------------------------------------------------------------------------")]

    [Header("# 피격 관련")]
    [Header("넉백 계수(넉백 계산식에서 데미지값에 곱해지는 수)")]
    public float m_KnockBackCoefficient = 5.0f;


    /// <summary>
    /// 점프 입력을 받았는지를 나타냅니다.
    /// </summary>
    private bool _IsJumpInput = false;

    /// <summary>
    /// 땅에 닿아있는지를 나타냅니다.
    /// </summary>
    private bool _IsGrounded = false;
        
    /// <summary>
    /// 목표 Yaw 회전값을 나타냅니다.
    /// </summary>
    private float _TargetYawAngle;

    /// <summary>
    /// 이동이 가능한지를 나타냅니다.
    /// </summary>
    private bool _AbleToMove = true;

    /// <summary>
    /// 방어력
    /// </summary>
    private float _Defence;

    /// <summary>
    /// 최대 방어력
    /// </summary>
    private float _MaxDefence;

    /// <summary>
    /// 입력받은 이동 방향을 저장하는 멤버변수입니다.
    /// </summary>
    private Vector2 _InputDirection;

    /// <summary>
    /// 목표 속도 벡터 입니다.
    /// </summary>
    private Vector3 _TargetVelocity;

    /// <summary>
    /// 넉백 속도
    /// </summary>
    private Vector3 _KnockBackVelocity;

    /// <summary>
    /// CharacterController 컴포넌트 객체입니다.
    /// </summary>
    private CharacterController _CharacterController;

    /// <summary>
    /// 밀려나고 있는지에 대한 읽기 전용 프로퍼티입니다.
    /// </summary>
    public bool isKnockBack => _KnockBackVelocity.sqrMagnitude > 0.0f;

    /// <summary>
    /// CharacterController 컴포넌트에 대한 읽기 전용 프로퍼티입니다.
    /// </summary>
    public CharacterController characterController => _CharacterController ?? (_CharacterController = GetComponent<CharacterController>());


    private void FixedUpdate()
    {
        // 바라보는 방향을 설정합니다.
        CalculateLookDirection();

        // XZ 평면 상의 속도를 계산합니다.
        CalculateXZVelocity();

        // Y 축 상의 속도를 계산합니다.
        CalculateYVelocity();        

        // 계산한 속도를 토대로 이동시킵니다.
        Move();

        // 넉백 속도가 있다면 넉백를 적용합니다.
        KnockBack();
    }

    /// <summary>
    /// 바라보는 방향을 설정합니다.
    /// </summary>
    private void CalculateLookDirection()
    {
        // 목표 Yaw 회전값을 통해 쿼터니언을 생성합니다.
        Quaternion targetLook = Quaternion.Euler(0.0f, _TargetYawAngle, 0.0f);

        // 생성한 쿼터니언 값을 통해 회전시킵니다.
        transform.rotation = targetLook;
    }

    /// <summary>
    /// XZ 평면 상의 속도를 계산합니다.
    /// </summary>
    private void CalculateXZVelocity()
    {
        // 새로 설정할 속도 벡터
        Vector3 newTargetVelocity = Vector3.zero;

        // 입력받은 이동 방향을 캐릭터의 전방 방향 기준으로 변환하여 새로 설정할 속도 벡터에 저장합니다.
        newTargetVelocity += transform.forward * _InputDirection.y;
        newTargetVelocity += transform.right * _InputDirection.x;
        newTargetVelocity.Normalize();
        newTargetVelocity *= m_Speed;

        // 목표 속도 벡터에 저장합니다.
        _TargetVelocity.x = newTargetVelocity.x;
        _TargetVelocity.z = newTargetVelocity.z;
    }

    /// <summary>
    /// Y 축 상의 속도를 계산합니다.
    /// </summary>
    private void CalculateYVelocity()
    {
        // 땅에 닿아있는지 체크합니다.
        _IsGrounded = CheckGrounded();

        // 중력값
        float gravity = Physics.gravity.y * Time.fixedDeltaTime;

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

        // 만약 점프 입력을 받은 상태라면 y 축 방향에 대한 속도값을 설정해 점프를 실시합니다.
        if (_IsJumpInput)
        {
            _TargetVelocity.y = m_JumpForce;
            _IsJumpInput = false;
        }
    }

    /// <summary>
    /// 계산한 속도를 토대로 이동시킵니다.
    /// </summary>
    private void Move()
    {
        // 이동이 가능한 상태라면 이동시킵니다.
        if(_AbleToMove)
            characterController.Move(_TargetVelocity * Time.fixedDeltaTime);        
    }

    /// <summary>
    /// 넉백 속도를 계산하여 넉백을 적용합니다.
    /// </summary>
    private void KnockBack()
    {
        // 넉백 상태가 아니라면 호출 종료합니다.
        if (!isKnockBack) return;

        // 대쉬 속도를 적용하여 객체를 이동시킵니다.
        _CharacterController.Move(_KnockBackVelocity * Time.fixedDeltaTime);

        // 대쉬 속도를 줄입니다.
        _KnockBackVelocity = Vector3.Lerp(_KnockBackVelocity, Vector3.zero, 0.8f);
        if (_KnockBackVelocity.magnitude < 0.1f) _KnockBackVelocity = Vector3.zero;
    }

    /// <summary>
    /// 땅에 닿아있는지를 체크합니다.
    /// </summary>
    /// <returns> 땅에 닿아있다면 참을 반환합니다.</returns>
    private bool CheckGrounded()
    {
        // Box cast 시작 지점 설정
        Vector3 center = characterController.center + transform.position;

        // Box cast 크기 설정
        Vector3 half = new Vector3(1.0f, 0.0f, 1.0f) * characterController.radius * transform.lossyScale.y;

        // Box cast 최대 길이 설정
        float maxDistance = (characterController.height * 0.5f * transform.lossyScale.y + characterController.skinWidth);

        // Box cast 실행
        return Physics.BoxCast(
                            center,
                            half,
                            Vector3.down,
                            Quaternion.identity,
                            maxDistance,
                            1 << LayerMask.NameToLayer("Ground"));
    }

    /// <summary>
    /// 데미지를 계산합니다. (데미지 = 상대 공격력 - 내 방어력)
    /// </summary>
    /// <param name="attack"> 받은 공격력</param>
    /// <returns></returns>
    private float CalculateDamage(float attack)
    {
        float damage = attack - _Defence;

        return (damage >= 0.0f) ? damage : 0.0f;
    }

    /// <summary>
    /// 이동 가능 여부를 설정합니다.
    /// </summary>
    /// <param name="movable"> 이동 가능은 참, 이동 불가능은 거짓</param>
    public void SetMovable(bool movable)
    {
        _AbleToMove = movable;
    }

    /// <summary>
    /// 방어력을 갱신합니다.
    /// </summary>
    /// <param name="ratio"> 적용할 최대 방어력의 비율</param>
    public void UpdateDefence(float ratio)
    {
        _Defence = _MaxDefence * ratio;
    }

    /// <summary>
    /// 레벨업 시 호출될 메서드입니다.
    /// </summary>
    /// <param name="level"> 달성 레벨</param>
    public void OnLevelUp(int level)
    {
        _MaxDefence = 10.0f + level * 2.0f;
    }

    /// <summary>
    /// 몬스터에게 맞았을 때 호출되는 메서드입니다.
    /// </summary>
    /// <param name="distance"> 밀려날 거리</param>
    /// <param name="direction"> 밀려날 방향</param>
    public void OnHit(float distance, Vector3 direction)
    {
        float damage = CalculateDamage(distance);

        _KnockBackVelocity += (damage * m_KnockBackCoefficient) * direction;
    }

    /// <summary>
    /// 이동 입력을 받을 시 호출되는 메서드입니다.
    /// 입력받은 이동 방향 값을 저장합니다.
    /// </summary>
    /// <param name="inputDirection"></param>
    public void OnMoveInput(Vector2 inputDirection)
    {
        _InputDirection = inputDirection;
    }

    /// <summary>
    /// 회전 입력을 받을 시 호출되는 메서드입니다.
    /// 목표 Yaw 회전값을 설정합니다.
    /// </summary>
    /// <param name="deltaYawAngle"></param>
    public void OnTurnInput(float deltaYawAngle)
    {
        _TargetYawAngle += deltaYawAngle * m_RotateSpeedInDegree * Time.fixedDeltaTime;
    }

    /// <summary>
    /// 점프 입력을 받을 시 호출되는 메서드입니다.
    /// 점프할 수 있는 조건을 만족할 때 점프 입력 받았음을 저장합니다.
    /// </summary>
    public void OnJumpInput()
    {
        if (_IsGrounded)
            _IsJumpInput = true;
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
