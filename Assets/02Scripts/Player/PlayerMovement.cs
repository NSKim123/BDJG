using System;
using UnityEditor;
using UnityEngine;

/// <summary>
/// �÷��̾��� ĳ������ �̵�, ����, �ǰ�(�з���)�� �����ϴ� ������Ʈ�Դϴ�.
/// </summary>
/// �̵� �κ� + ����Ƽ �̺�Ʈ �Լ� �κ�
public partial class PlayerMovement : MonoBehaviour
{
    [Header("# �̵� ����")]
    [Header("�̵� �ӷ�")]
    public float m_Speed = 3.5f;

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
        // ���� ��Ÿ���� �����մϴ�.
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
        // �̵� �Է��� ���� ���� ���¶�� ��ǥ ȸ���� ������ �ǳʶݴϴ�.
        if(_InputDirection == Vector2.zero) return;

        // ��ǥ ȸ������ �����մϴ�.
        _TargetYawAngle = Mathf.Atan2(_InputDirection.x, _InputDirection.y) * Mathf.Rad2Deg;        
    }

    /// <summary>
    /// XZ ��� ���� �ӵ��� ����ϴ� �޼����Դϴ�.
    /// </summary>
    private void CalculateXZVelocity()
    {
        // ���� ������ �ӵ� ����
        Vector3 newTargetVelocity = Vector3.zero;

        // �̵��� �� �ִ� ���¶�� �ӵ� ���͸� �����մϴ�.
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
    /// Y �� ���� �ӵ��� ����ϴ� �޼����Դϴ�.
    /// </summary>
    private void CalculateYVelocity()
    {
        // ���� ����ִ��� üũ�մϴ�.
        _IsGrounded = CheckGrounded();

        // �� �Ӹ� ���� ���� �ִ��� üũ�մϴ�.
        _IsAboveEnemy = CheckAboveEnemy();

        // �߷°�
        float gravity = Physics.gravity.y * Time.fixedDeltaTime * 3.0f;

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

        // ���� ���� Ʈ���Ű� Ȱ��ȭ�Ǿ��ִ� ���¶�� y �� ���⿡ ���� �ӵ����� ������ ������ �ǽ��մϴ�.
        if (_IsJumpTrigger)
        {
            Jump();
        }
    }

    /// <summary>
    /// ����� �ӵ��� ���� �̵���Ű�� �޼����Դϴ�.
    /// </summary>
    private void Move()
    {
        // �̵��� ������ ���¶�� �̵���ŵ�ϴ�.
        if(_AbleToMove)
            characterController.Move(_TargetVelocity * Time.fixedDeltaTime);

        // �׷��� �ʴٸ� Y �� ���� �̵��� �����ŵ�ϴ�.
        else
            characterController.Move(Vector3.up * _TargetVelocity.y * Time.fixedDeltaTime);
    }

    /// <summary>
    /// ��ǥ ȸ������ ���� ȸ���ϴ� �޼����Դϴ�.
    /// </summary>
    private void Rotation()
    {        
        // ��ǥ ȸ������ ����մϴ�.
        Quaternion targetRotation = Quaternion.Euler(0.0f, _TargetYawAngle, 0.0f);

        // �ε巴�� ȸ���ϵ��� ����մϴ�.
        Quaternion newRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 720.0f * Time.fixedDeltaTime);

        // ������ ȸ������ ���� ȸ����ŵ�ϴ�.
        transform.rotation = newRotation;
    }   
    
    /// <summary>
    /// Movement ������Ʈ�� �����ϴ� ������ ������ �޼����Դϴ�.
    /// </summary>
    /// <param name="initPosition"></param>
    public void ResetMovementComponent(Vector3 initPosition)
    {
        // �˹� �ӵ��� �����ͷ� �ʱ�ȭ�մϴ�.
        _KnockBackVelocity = Vector3.zero;

        // ���� ���� ���� ��ġ�� �̵���ŵ�ϴ�.
        // �� ���� Move �Լ� ȣ��δ� �ǵ��� ��ġ�� ���� �ʴ� ��찡 �־�, ���� ���� Move �Լ��� ȣ���Ͽ����ϴ�.
        for (int i = 0; i < 5; ++i)
        {
            characterController.Move(initPosition - transform.position);
        }
    }

    /// <summary>
    /// �̵� ���� ���θ� �����ϴ� �޼����Դϴ�.
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
    /// <param name="inputDirection"> �Է� ����</param>
    public void OnMoveInput(Vector2 inputDirection)
    {
        _InputDirection = inputDirection;
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
/// ���� ���� �κ�
/// </summary>
public partial class PlayerMovement
{
    private static string SOUNDNAME_GIANTSLIME_LAND = "Effect_Slime_GiantLand";
    private static string SOUNDNAME_SLIME_JUMP = "Effect_Slime_Jump";

    [Header("# ���� ����")]
    [Header("������")]
    public float m_JumpForce = 15.0f;

    [Header("���� ��Ÿ��")]
    public float m_JumpReuseTime = 1.0f;

    [Header("# �Ŵ�ȭ �������϶� ���� �� ����Ʈ")]
    public GameObject m_Effect_GiantSlimeLand;

    [Header("------------------------------------------------------------------------------")]

    /// <summary>
    /// ���� Ʈ���Ű� Ȱ��ȭ�Ǿ��ִ����� ��Ÿ���ϴ�.
    /// </summary>
    private bool _IsJumpTrigger = false;

    /// <summary>
    /// ���� ����ִ����� ��Ÿ���ϴ�.
    /// </summary>
    private bool _IsGrounded = false;

    /// <summary>
    /// �� �Ӹ� ���� �ִ����� ��Ÿ���ϴ�.
    /// </summary>
    private bool _IsAboveEnemy = false;

    /// <summary>
    /// ���� ������ �����ؾ��ϴ� �������� ũ���Դϴ�.
    /// </summary>
    private float _CurrentJumpForce;

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
    /// ���� ����ִ����� üũ�ϴ� �޼����Դϴ�.
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
    /// �� �Ӹ� ���� �ִ����� üũ�ϴ� �޼����Դϴ�.
    /// </summary>
    /// <returns> �� �Ӹ� ���� �ִٸ� ���� ��ȯ�մϴ�.</returns>
    private bool CheckAboveEnemy()
    {
        // ���� CharacterController �� ���� ���� ���̾ Enemy ���̾ �ִٸ� �� üũ ������ �ǳʶݴϴ�.
        if ( ((LayerMask.GetMask("Enemy")) & characterController.excludeLayers.value) > 0) return false;

        // Box cast ���� ���� ����
        Vector3 center = characterController.center + transform.position;

        // Box cast ũ�� ����
        Vector3 half = new Vector3(1.0f, 0.0f, 1.0f) * characterController.radius * transform.lossyScale.y * 0.35f;

        // Box cast �ִ� ���� ���� + ĳ������ �� �غκб����� �˻��մϴ�.
        float maxDistance = (characterController.height * 0.5f + characterController.skinWidth) * transform.lossyScale.y;

        // Box cast ����
        bool result = Physics.BoxCast(
                        center,
                        half,
                        Vector3.down,
                        out RaycastHit hitResult,
                        Quaternion.identity,
                        maxDistance,
                        1 << LayerMask.NameToLayer("Enemy"));

        // ���� ���� �����Ǿ��ٸ�, ������ ���� ���� ������ �ϵ��� �����մϴ�.
        if (result)
        {
            // ������ ����
            _CurrentJumpForce = m_JumpForce * 0.6f;

            // ���� ������ ���� ����
            _IsJumpTrigger = true;            
        }

        // ��� ��ȯ
        return result;
    }

    /// <summary>
    /// ���� ��Ÿ���� �����ϴ� �޼����Դϴ�.
    /// </summary>
    private void UpdateJumpReuseTimeGauge()
    {
        // �̹� ��Ÿ���� �� ���Ҵٸ� �Լ� ȣ�� ����
        if (_JumpReuseTimeGauge.currentValue == _JumpReuseTimeGauge.min) return;

        // ��Ÿ�� ����
        _JumpReuseTimeGauge.currentValue -= Time.deltaTime;
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
    /// ���� ���� ���¸� Ȯ���ϰ� ���� �Է� �޾����� �����ϴ� �޼����Դϴ�.
    /// </summary>
    private void TryJump()
    {
        // ���� ������� �ʰų� ��Ÿ���� �����ִ� ���¶�� ȣ���� �����մϴ�.
        if (!_IsGrounded || !(_JumpReuseTimeGauge.ratio == 0.0f)) return;

        // ���� �Է��� �Ұ����� ��Ȳ�̶�� ȣ���� �����մϴ�.
        if (!CheckJumpable()) return;

        // �޾����� �����մϴ�.
        _IsJumpTrigger = true;

        // ������ ����
        _CurrentJumpForce = m_JumpForce;
    }

    /// <summary>
    /// ���� ��Ű�� �޼����Դϴ�.
    /// </summary>
    private void Jump()
    {
        // ���� ��ŵ�ϴ�.
        _TargetVelocity.y = _CurrentJumpForce;

        // ���� Ʈ���� ����
        _IsJumpTrigger = false;

        // ���� �ִϸ��̼��� ����
        _OwnerCharacter.animController.TriggerJumpParam();

        // ��Ÿ���� ����
        _JumpReuseTimeGauge.currentValue = _JumpReuseTimeGauge.max;

        // ���� ���� �÷���
        SoundManager.Instance.PlaySound(SOUNDNAME_SLIME_JUMP, SoundType.Effect);
    }

    /// <summary>
    /// �Ŵ�ȭ �������� ���� ����Ʈ�� �߻���Ű�� �޼����Դϴ�.
    /// </summary>
    public void InstantiateGiantSlimeLandEffect()
    {
        GameObject effect = Instantiate(m_Effect_GiantSlimeLand);

        effect.transform.position = transform.position + Vector3.down * characterController.height / 2.0f * transform.lossyScale.y;
    }

    /// <summary>
    /// �Ŵ�ȭ �������� ���� ���带 ����ϴ� �޼����Դϴ�.
    /// </summary>
    public void PlayGiantSlimeLandSound()
    {
        SoundManager.Instance.PlaySound(SOUNDNAME_GIANTSLIME_LAND, SoundType.Effect, 2.0f);
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
    private static string SOUNDNAME_DAMAGED = "Effect_Damaged";

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
    /// �ǰ� �鿪 ���¸� ��Ÿ���ϴ�.
    /// </summary>
    private bool _IsImmune;

    /// <summary>
    /// �˹� �ӵ�
    /// </summary>
    private Vector3 _KnockBackVelocity;

    /// <summary>
    /// �з����� �ִ����� ���� �б� ���� ������Ƽ�Դϴ�.
    /// </summary>
    public bool isKnockBack => _KnockBackVelocity.sqrMagnitude > 0.0f;

    /// <summary>
    /// �������� ����ϴ� �޼����Դϴ�. (������ = ��� ���ݷ� - �� ����)
    /// </summary>
    /// <param name="attack"> ���� ���ݷ�</param>
    /// <returns> ��� ������</returns>
    private float CalculateDamage(float attack)
    {
        // ������ = ��� ���ݷ� - �� ����
        float damage = attack - _Defence;

        return (damage >= 0.0f) ? damage : 0.0f;
    }

    /// <summary>
    /// �˹� �ӵ��� ����Ͽ� �˹��� �����ϴ� �޼����Դϴ�.
    /// </summary>
    private void KnockBack()
    {
        // �˹� ���°� �ƴ϶�� ȣ�� �����մϴ�.
        if (!isKnockBack) return;

        // �˹� �ӵ��� �����Ͽ� ��ü�� �̵���ŵ�ϴ�.
        _CharacterController.Move(_KnockBackVelocity * Time.fixedDeltaTime);

        // �˹� �ӵ��� ���� ���Դϴ�.
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
    /// ������ �����ϴ� �޼����Դϴ�.
    /// </summary>
    /// <param name="ratio"> ������ �ִ� ������ ����</param>
    public void UpdateDefence(float ratio)
    {
        _Defence = _MaxDefence * ratio;
    }
    
    /// <summary>
    /// �鿪 ���¸� �����ϴ� �޼����Դϴ�.
    /// </summary>
    /// <param name="immune"> ������ �鿪 ����</param>
    public void SetImmuneState(bool immune)
    {
        _IsImmune = immune;
    }

    /// <summary>
    /// ������ �� ȣ��� �޼����Դϴ�.
    /// </summary>
    /// <param name="level"> �޼� ����</param>
    public void OnLevelUp(int level)
    {
        // �ִ� ���� ����
        _MaxDefence = 10.0f + level * 2.0f;
    }

    /// <summary>
    /// ���Ϳ��� �¾��� �� ȣ��Ǵ� �޼����Դϴ�.
    /// </summary>
    /// <param name="distance"> �з��� �Ÿ�</param>
    /// <param name="direction"> �з��� ����</param>
    public void OnHit(float distance, Vector3 direction)
    {
        // �鿪 ������ ��� �Լ� ����
        if (_IsImmune) return;

        // ������ ���
        float damage = CalculateDamage(distance);        

        // �˹� �ӵ� ��� ( ���� �˹鷮 = �������� * �˹� ��� )
        _KnockBackVelocity = (damage * m_KnockBackCoefficient) * direction;

        // �ǰ� �ִϸ��̼��� �����մϴ�.
        if(damage > 0.0f)
        {
            _OwnerCharacter.animController.TriggerDamagedParam();
            SoundManager.Instance.PlaySound(SOUNDNAME_DAMAGED, SoundType.Effect);
        }
    }
}



/// <summary>
/// ������ ȿ�� ���� �κ�
/// </summary>
public partial class PlayerMovement
{
    /// <summary>
    /// �Ŵ�ȭ ��� ���۽� Movement ������Ʈ���� ����Ǿ���ϴ� ����
    /// </summary>
    public void OnStartGiant()
    {
        // �鿪 ���� ����
        SetImmuneState(true);

        // ������ �浹�� �����մϴ�.
        characterController.excludeLayers += LayerMask.GetMask("Enemy");
    }

    /// <summary>
    /// �Ŵ�ȭ ��� ����� Movement ������Ʈ���� ����Ǿ���ϴ� ����
    /// </summary>
    public void OnFinishGiant()
    {
        // �鿪 ���� ����
        SetImmuneState(false);

        // �� �浹 ���� ����
        characterController.excludeLayers -= LayerMask.GetMask("Enemy");
    }

    /// <summary>
    /// ����� ��� ���۽� Movement ������Ʈ���� ����Ǿ���ϴ� ����
    /// </summary>
    public void OnStartMachineGun()
    {
        // ������ �浹�� �����մϴ�.
        characterController.excludeLayers += LayerMask.GetMask("Enemy");
    }

    /// <summary>
    /// ����� ������ ��� �� �����Ӹ��� Movement ������Ʈ���� ����Ǿ���ϴ� ����
    /// </summary>
    public void OnUpdateMachineGun()
    {
        // ������ ����
        SetMovable(false);

        // �鿪 ���� ����
        SetImmuneState(true);
    }

    /// <summary>
    /// ����� ��� ����� Movement ������Ʈ���� ����Ǿ���ϴ� ����
    /// </summary>
    public void OnFinishMachineGun()
    {
        // ������ ���� ����
        SetMovable(true);

        // �鿪 ���� ����
        SetImmuneState(false);

        // �� �浹 ���� ����
        characterController.excludeLayers -= LayerMask.GetMask("Enemy");
    }
}


