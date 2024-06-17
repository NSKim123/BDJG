using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

/// <summary>
/// �÷��̾��� ĳ������ �̵�, ����, �ǰ�(�з���)�� �����ϴ� ������Ʈ�Դϴ�.
/// </summary>
/// �̵� �κ�
public partial class PlayerMovement : MonoBehaviour
{
    [Header("# �̵� ����")]
    [Header("�̵� �ӷ�")]
    public float m_Speed = 5.0f;

    [Header("ȸ�� �ӷ�")]
    public float m_RotateSpeedInDegree = 30.0f;

    [Header("------------------------------------------------------------------------------")]
        
    /// <summary>
    /// ��ǥ Yaw ȸ������ ��Ÿ���ϴ�.
    /// </summary>
    private float _TargetYawAngle;

    /// <summary>
    /// �̵��� ���������� ��Ÿ���ϴ�.
    /// </summary>
    private bool _AbleToMove = true;    

    /// <summary>
    /// �Է¹��� �̵� ������ �����ϴ� ��������Դϴ�.
    /// </summary>
    private Vector2 _InputDirection;

    /// <summary>
    /// ��ǥ �ӵ� ���� �Դϴ�.
    /// </summary>
    private Vector3 _TargetVelocity;
    
    /// <summary>
    /// CharacterController ������Ʈ ��ü�Դϴ�.
    /// </summary>
    private CharacterController _CharacterController;

    /// <summary>
    /// �� �̵� ������Ʈ�� ������ �ִ� PlayerCharacter ��ü�Դϴ�.
    /// </summary>
    private PlayerCharacter _OwnerCharacter;

    /// <summary>
    /// ZX ��� �󿡼��� ����ȭ�� �ӷ�(�ӷ� / �ִ� �ӷ�)�� ��Ÿ���� �б� ���� ������Ƽ�Դϴ�.
    /// </summary>
    public float normalizedZXSpeed => Mathf.Sqrt(((_TargetVelocity).x * (_TargetVelocity).x + (_TargetVelocity).z * (_TargetVelocity).z)) / m_Speed;

    /// <summary>
    /// CharacterController ������Ʈ�� ���� �б� ���� ������Ƽ�Դϴ�.
    /// </summary>
    public CharacterController characterController => _CharacterController ?? (_CharacterController = GetComponent<CharacterController>());

    private void Awake()
    {
        // ���� ������ ��ü�� �����մϴ�.
        _JumpReuseTimeGauge = new FloatGauge(m_JumpReuseTime);

        //�� �̵� ������Ʈ�� ������ �ִ� PlayerCharacter ��ü�� ã���ϴ�.
        _OwnerCharacter = GetComponent<PlayerCharacter>();
    }

    private void Update()
    {
        // ��Ÿ���� �����մϴ�.
        UpdateJumpReuseTimeGauge();
    }

    private void FixedUpdate()
    {
        // XZ ��� ���� �ӵ��� ����մϴ�.
        CalculateXZVelocity();

        // ��ǥ ȸ������ ����մϴ�.
        CalculateTargetYawAngle();

        // Y �� ���� �ӵ��� ����մϴ�.
        CalculateYVelocity();        

        // ����� �ӵ��� ���� �̵���ŵ�ϴ�.
        Move();

        // ��ǥ ȸ������ ���� ȸ���մϴ�.
        Rotation();

        // �˹� �ӵ��� �ִٸ� �˹鸦 �����մϴ�.
        KnockBack();
    }

    /// <summary>
    /// ��ǥ ȸ������ ����ϴ� �޼����Դϴ�.
    /// </summary>
    private void CalculateTargetYawAngle()
    {
        // �̵��ϰ� �ִ� ��Ȳ�� �ƴ϶�� ȣ���� �����մϴ�.
        if (normalizedZXSpeed == 0.0f) return;
        
        // ��ǥ ȸ������ �����մϴ�.
        _TargetYawAngle = Mathf.Atan2(_TargetVelocity.x, _TargetVelocity.z) * Mathf.Rad2Deg;        
    }

    /// <summary>
    /// XZ ��� ���� �ӵ��� ����մϴ�.
    /// </summary>
    private void CalculateXZVelocity()
    {
        // ���� ������ �ӵ� ����
        Vector3 newTargetVelocity = Vector3.zero;

        if (_AbleToMove)
        {
            // �Է¹��� �̵� ������ ���� ������ �ӵ� ���Ϳ� �����մϴ�.
            newTargetVelocity += Vector3.forward * _InputDirection.y;
            newTargetVelocity += Vector3.right * _InputDirection.x;
            newTargetVelocity.Normalize();
            newTargetVelocity *= m_Speed;
        }            

        // ��ǥ �ӵ� ���Ϳ� �����մϴ�.
        _TargetVelocity.x = newTargetVelocity.x;
        _TargetVelocity.z = newTargetVelocity.z;
    }

    /// <summary>
    /// Y �� ���� �ӵ��� ����մϴ�.
    /// </summary>
    private void CalculateYVelocity()
    {
        // ���� ����ִ��� üũ�մϴ�.
        _IsGrounded = CheckGrounded();

        // �߷°�
        float gravity = Physics.gravity.y * Time.fixedDeltaTime;

        // ���� ����ִٸ� y �� ���⿡ ���� �ӵ��� 0���� �����մϴ�.
        if(_IsGrounded)
        {
            _TargetVelocity.y = 0.0f;
        }
        // ���� ������� �ʴٸ� y �� ���⿡ ���� �ӵ��� �߷°��� ���մϴ�.
        else
        {
            _TargetVelocity.y += gravity;
        }

        // ���� ���� �Է��� ���� ���¶�� y �� ���⿡ ���� �ӵ����� ������ ������ �ǽ��մϴ�.
        if (_IsJumpInput)
        {
            Jump();
        }
    }

    /// <summary>
    /// ����� �ӵ��� ���� �̵���ŵ�ϴ�.
    /// </summary>
    private void Move()
    {
        // �̵��� ������ ���¶�� �̵���ŵ�ϴ�.
        if(_AbleToMove)
            characterController.Move(_TargetVelocity * Time.fixedDeltaTime);        
    }

    /// <summary>
    /// ��ǥ ȸ������ ���� ȸ���ϴ� �޼����Դϴ�.
    /// </summary>
    private void Rotation()
    {        
        // ��ǥ ȸ������ ����մϴ�.
        Quaternion targetRotation = Quaternion.Euler(0.0f, _TargetYawAngle, 0.0f);

        // �ε巴�� ȸ���ϵ��� ����մϴ�.
        Quaternion newRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 360.0f * Time.fixedDeltaTime);

        // ������ ȸ������ ���� ȸ����ŵ�ϴ�.
        transform.rotation = newRotation;
    }

    /// <summary>
    /// �̵� ���� ���θ� �����մϴ�.
    /// </summary>
    /// <param name="movable"> �̵� ������ ��, �̵� �Ұ����� ����</param>
    public void SetMovable(bool movable)
    {
        _AbleToMove = movable;
    }

    /// <summary>
    /// �̵� �Է��� ���� �� ȣ��Ǵ� �޼����Դϴ�.
    /// �Է¹��� �̵� ���� ���� �����մϴ�.
    /// </summary>
    /// <param name="inputDirection"></param>
    public void OnMoveInput(Vector2 inputDirection)
    {
        _InputDirection = inputDirection;
    }

    /// <summary>
    /// ȸ�� �Է��� ���� �� ȣ��Ǵ� �޼����Դϴ�.
    /// ��ǥ Yaw ȸ������ �����մϴ�.
    /// </summary>
    /// <param name="deltaYawAngle"></param>
    public void OnTurnInput(float deltaYawAngle)
    {
        //_TargetYawAngle += deltaYawAngle * m_RotateSpeedInDegree * Time.fixedDeltaTime;
    }    

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        GUIContent gUIContent = new GUIContent();
        gUIContent.text = $"\n\n\n\n\n\n\n\n���� / �ִ� ����: {_Defence} / {_MaxDefence}";
        Handles.Label(transform.position + Vector3.down, gUIContent);
    }
#endif
}





/// <summary>
/// ���� ����
/// </summary>
public partial class PlayerMovement
{
    [Header("# ���� ����")]
    [Header("������")]
    public float m_JumpForce = 5.0f;

    [Header("���� ��Ÿ��")]
    public float m_JumpReuseTime = 1.0f;

    [Header("------------------------------------------------------------------------------")]

    /// <summary>
    /// ���� �Է��� �޾Ҵ����� ��Ÿ���ϴ�.
    /// </summary>
    private bool _IsJumpInput = false;

    /// <summary>
    /// ���� ����ִ����� ��Ÿ���ϴ�.
    /// </summary>
    private bool _IsGrounded = false;

    /// <summary>
    /// ���� ��Ÿ�� ������ ��ü
    /// </summary>
    private FloatGauge _JumpReuseTimeGauge;

    /// <summary>
    /// ���� ��ư Ȱ��ȭ ������ ��Ÿ���� �б� ���� ������Ƽ�Դϴ�.
    /// </summary>
    public bool isJumpable => CheckJumpable();

    /// <summary>
    /// ���� ����ִ����� ���� �б� ���� ������Ƽ�Դϴ�.
    /// </summary>
    public bool isGrounded => _IsGrounded;

    /// <summary>
    /// ���� ��Ÿ�� ������ ��ü�� ���� �б� ���� ������Ƽ�Դϴ�.
    /// </summary>
    public FloatGauge jumpResueTimeGauge => _JumpReuseTimeGauge;

    /// <summary>
    /// ���� ����ִ����� üũ�մϴ�.
    /// </summary>
    /// <returns> ���� ����ִٸ� ���� ��ȯ�մϴ�.</returns>
    private bool CheckGrounded()
    {
        // Box cast ���� ���� ����
        Vector3 center = characterController.center + transform.position;

        // Box cast ũ�� ����
        Vector3 half = new Vector3(1.0f, 0.0f, 1.0f) * characterController.radius * transform.lossyScale.y;

        // Box cast �ִ� ���� ����
        float maxDistance = (characterController.height * 0.5f + characterController.skinWidth) * transform.lossyScale.y;

        // Box cast ����
        bool result = Physics.BoxCast(
                            center,
                            half,
                            Vector3.down,
                            out RaycastHit hitResult,
                            Quaternion.identity,
                            maxDistance,
                            1 << LayerMask.NameToLayer("Ground"));

        // ���� �����ϸ� ���� �����ִ� ���̸�ŭ y�� �������� �̵������ݴϴ�.
        if (result)
        {
            characterController.Move(Vector3.up * (maxDistance - hitResult.distance));
        }

        return result;
    }

    /// <summary>
    /// ���� ��Ÿ���� �����մϴ�.
    /// </summary>
    private void UpdateJumpReuseTimeGauge()
    {
        // �̹� ��Ÿ���� �� ���Ҵٸ� �Լ� ȣ�� ����
        if (_JumpReuseTimeGauge.currentValue == _JumpReuseTimeGauge.min) return;

        // ��Ÿ�� ����
        _JumpReuseTimeGauge.currentValue -= Time.deltaTime;
    }

    /// <summary>
    /// ���� ���� ���¸� Ȯ���ϰ� ���� �Է� �޾����� �����ϴ� �޼����Դϴ�.
    /// </summary>
    private void TryJump()
    {
        // ���� ������� �ʰų� ��Ÿ���� �����ִ� ���¶�� ȣ���� �����մϴ�.
        if (!_IsGrounded || !(_JumpReuseTimeGauge.ratio == 0.0f)) return;

        // ���� �Է��� �Ұ����� ��Ȳ�̶�� ȣ���� �����մϴ�.
        if (!CheckJumpable()) return;

        // �޾����� �����մϴ�.
        _IsJumpInput = true;
    }

    /// <summary>
    /// ���� ��Ű�� �޼����Դϴ�.
    /// </summary>
    private void Jump()
    {
        // ���� ��ŵ�ϴ�.
        _TargetVelocity.y = m_JumpForce;
        _IsJumpInput = false;

        // ���� �ִϸ��̼��� �����մϴ�.
        _OwnerCharacter.animController.TriggerJumpParam();

        // ��Ÿ���� ������ �����մϴ�.
        _JumpReuseTimeGauge.currentValue = _JumpReuseTimeGauge.max;
    }


    /// <summary>
    /// ���� �Է��� ���� �� ȣ��Ǵ� �޼����Դϴ�.
    /// </summary>
    public void OnJumpInput()
    {
        TryJump();
    }
}







/// <summary>
/// �ǰ� ����
/// </summary>
public partial class PlayerMovement
{
    [Header("# �ǰ� ����")]
    [Header("�˹� ���(�˹� ���Ŀ��� ���������� �������� ��)")]
    public float m_KnockBackCoefficient = 5.0f;

    /// <summary>
    /// ����
    /// </summary>
    private float _Defence;

    /// <summary>
    /// �ִ� ����
    /// </summary>
    private float _MaxDefence;

    /// <summary>
    /// �˹� �ӵ�
    /// </summary>
    private Vector3 _KnockBackVelocity;

    /// <summary>
    /// �з����� �ִ����� ���� �б� ���� ������Ƽ�Դϴ�.
    /// </summary>
    public bool isKnockBack => _KnockBackVelocity.sqrMagnitude > 0.0f;

    /// <summary>
    /// �������� ����մϴ�. (������ = ��� ���ݷ� - �� ����)
    /// </summary>
    /// <param name="attack"> ���� ���ݷ�</param>
    /// <returns></returns>
    private float CalculateDamage(float attack)
    {
        float damage = attack - _Defence;

        return (damage >= 0.0f) ? damage : 0.0f;
    }

    /// <summary>
    /// �˹� �ӵ��� ����Ͽ� �˹��� �����մϴ�.
    /// </summary>
    private void KnockBack()
    {
        // �˹� ���°� �ƴ϶�� ȣ�� �����մϴ�.
        if (!isKnockBack) return;

        // �뽬 �ӵ��� �����Ͽ� ��ü�� �̵���ŵ�ϴ�.
        _CharacterController.Move(_KnockBackVelocity * Time.fixedDeltaTime);

        // �뽬 �ӵ��� ���Դϴ�.
        _KnockBackVelocity = Vector3.MoveTowards(_KnockBackVelocity, Vector3.zero, 100.0f * Time.fixedDeltaTime);
        if (_KnockBackVelocity.magnitude < 0.1f) _KnockBackVelocity = Vector3.zero;
    }

    /// <summary>
    /// �ൿ �Ұ� �������� üũ�ϴ� �޼����Դϴ�.
    /// </summary>
    /// <returns></returns>
    private bool CheckStunned()
    {
        return _OwnerCharacter.isStunned;
    }

    /// <summary>
    /// ���� �������� üũ�ϴ� �޼����Դϴ�.
    /// </summary>
    /// <returns></returns>
    private bool CheckDead()
    {
        return _OwnerCharacter.isDead;
    }

    /// <summary>
    /// ���� �Է��� ������ �������� üũ�ϴ� �޼����Դϴ�.
    /// </summary>
    /// <returns></returns>
    private bool CheckJumpable()
    {
        return !isKnockBack && !CheckStunned() && !CheckDead();
    }

    /// <summary>
    /// ������ �����մϴ�.
    /// </summary>
    /// <param name="ratio"> ������ �ִ� ������ ����</param>
    public void UpdateDefence(float ratio)
    {
        _Defence = _MaxDefence * ratio;
    }    

    /// <summary>
    /// ������ �� ȣ��� �޼����Դϴ�.
    /// </summary>
    /// <param name="level"> �޼� ����</param>
    public void OnLevelUp(int level)
    {
        _MaxDefence = 10.0f + level * 2.0f;
    }

    /// <summary>
    /// ���Ϳ��� �¾��� �� ȣ��Ǵ� �޼����Դϴ�.
    /// </summary>
    /// <param name="distance"> �з��� �Ÿ�</param>
    /// <param name="direction"> �з��� ����</param>
    public void OnHit(float distance, Vector3 direction)
    {
        // ������ �� �˹� �ӵ� ���
        float damage = CalculateDamage(distance);
        _KnockBackVelocity += (damage * m_KnockBackCoefficient) * direction;

        // �ǰ� �ִϸ��̼��� �����մϴ�.
        _OwnerCharacter.animController.TriggerDamagedParam();
    }
}