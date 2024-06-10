using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// �÷��̾��� ĳ������ �̵��� �����ϴ� ������Ʈ�Դϴ�.
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    [Header("# �̵� ����")]
    [Header("�̵� �ӷ�")]
    public float m_Speed = 5.0f;

    [Header("ȸ�� �ӷ�")]
    public float m_RotateSpeedInDegree = 30.0f;

    [Header("------------------------------------------------------------------------------")]

    [Header("# ���� ����")]
    [Header("������")]
    public float m_JumpForce = 5.0f;

    [Header("------------------------------------------------------------------------------")]

    [Header("# �ǰ� ����")]
    [Header("�˹� ���(�˹� ���Ŀ��� ���������� �������� ��)")]
    public float m_KnockBackCoefficient = 5.0f;


    /// <summary>
    /// ���� �Է��� �޾Ҵ����� ��Ÿ���ϴ�.
    /// </summary>
    private bool _IsJumpInput = false;

    /// <summary>
    /// ���� ����ִ����� ��Ÿ���ϴ�.
    /// </summary>
    private bool _IsGrounded = false;
        
    /// <summary>
    /// ��ǥ Yaw ȸ������ ��Ÿ���ϴ�.
    /// </summary>
    private float _TargetYawAngle;

    /// <summary>
    /// �̵��� ���������� ��Ÿ���ϴ�.
    /// </summary>
    private bool _AbleToMove = true;

    /// <summary>
    /// ����
    /// </summary>
    private float _Defence;

    /// <summary>
    /// �ִ� ����
    /// </summary>
    private float _MaxDefence;

    /// <summary>
    /// �Է¹��� �̵� ������ �����ϴ� ��������Դϴ�.
    /// </summary>
    private Vector2 _InputDirection;

    /// <summary>
    /// ��ǥ �ӵ� ���� �Դϴ�.
    /// </summary>
    private Vector3 _TargetVelocity;

    /// <summary>
    /// �˹� �ӵ�
    /// </summary>
    private Vector3 _KnockBackVelocity;

    /// <summary>
    /// CharacterController ������Ʈ ��ü�Դϴ�.
    /// </summary>
    private CharacterController _CharacterController;

    /// <summary>
    /// �з����� �ִ����� ���� �б� ���� ������Ƽ�Դϴ�.
    /// </summary>
    public bool isKnockBack => _KnockBackVelocity.sqrMagnitude > 0.0f;

    /// <summary>
    /// CharacterController ������Ʈ�� ���� �б� ���� ������Ƽ�Դϴ�.
    /// </summary>
    public CharacterController characterController => _CharacterController ?? (_CharacterController = GetComponent<CharacterController>());


    private void FixedUpdate()
    {
        // �ٶ󺸴� ������ �����մϴ�.
        CalculateLookDirection();

        // XZ ��� ���� �ӵ��� ����մϴ�.
        CalculateXZVelocity();

        // Y �� ���� �ӵ��� ����մϴ�.
        CalculateYVelocity();        

        // ����� �ӵ��� ���� �̵���ŵ�ϴ�.
        Move();

        // �˹� �ӵ��� �ִٸ� �˹鸦 �����մϴ�.
        KnockBack();
    }

    /// <summary>
    /// �ٶ󺸴� ������ �����մϴ�.
    /// </summary>
    private void CalculateLookDirection()
    {
        // ��ǥ Yaw ȸ������ ���� ���ʹϾ��� �����մϴ�.
        Quaternion targetLook = Quaternion.Euler(0.0f, _TargetYawAngle, 0.0f);

        // ������ ���ʹϾ� ���� ���� ȸ����ŵ�ϴ�.
        transform.rotation = targetLook;
    }

    /// <summary>
    /// XZ ��� ���� �ӵ��� ����մϴ�.
    /// </summary>
    private void CalculateXZVelocity()
    {
        // ���� ������ �ӵ� ����
        Vector3 newTargetVelocity = Vector3.zero;

        // �Է¹��� �̵� ������ ĳ������ ���� ���� �������� ��ȯ�Ͽ� ���� ������ �ӵ� ���Ϳ� �����մϴ�.
        newTargetVelocity += transform.forward * _InputDirection.y;
        newTargetVelocity += transform.right * _InputDirection.x;
        newTargetVelocity.Normalize();
        newTargetVelocity *= m_Speed;

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
            _TargetVelocity.y = m_JumpForce;
            _IsJumpInput = false;
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
    /// �˹� �ӵ��� ����Ͽ� �˹��� �����մϴ�.
    /// </summary>
    private void KnockBack()
    {
        // �˹� ���°� �ƴ϶�� ȣ�� �����մϴ�.
        if (!isKnockBack) return;

        // �뽬 �ӵ��� �����Ͽ� ��ü�� �̵���ŵ�ϴ�.
        _CharacterController.Move(_KnockBackVelocity * Time.fixedDeltaTime);

        // �뽬 �ӵ��� ���Դϴ�.
        _KnockBackVelocity = Vector3.Lerp(_KnockBackVelocity, Vector3.zero, 0.8f);
        if (_KnockBackVelocity.magnitude < 0.1f) _KnockBackVelocity = Vector3.zero;
    }

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
        float maxDistance = (characterController.height * 0.5f * transform.lossyScale.y + characterController.skinWidth);

        // Box cast ����
        return Physics.BoxCast(
                            center,
                            half,
                            Vector3.down,
                            Quaternion.identity,
                            maxDistance,
                            1 << LayerMask.NameToLayer("Ground"));
    }

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
    /// �̵� ���� ���θ� �����մϴ�.
    /// </summary>
    /// <param name="movable"> �̵� ������ ��, �̵� �Ұ����� ����</param>
    public void SetMovable(bool movable)
    {
        _AbleToMove = movable;
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
        float damage = CalculateDamage(distance);

        _KnockBackVelocity += (damage * m_KnockBackCoefficient) * direction;
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
        _TargetYawAngle += deltaYawAngle * m_RotateSpeedInDegree * Time.fixedDeltaTime;
    }

    /// <summary>
    /// ���� �Է��� ���� �� ȣ��Ǵ� �޼����Դϴ�.
    /// ������ �� �ִ� ������ ������ �� ���� �Է� �޾����� �����մϴ�.
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
        gUIContent.text = $"\n\n\n\n\n\n\n\n���� / �ִ� ����: {_Defence} / {_MaxDefence}";
        Handles.Label(transform.position + Vector3.down, gUIContent);
    }
#endif
}
