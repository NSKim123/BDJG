using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �÷��̾��� �ִϸ����͸� �����ϴ� ������Ʈ
/// </summary>
public class PlayerAnimController : AnimController
{
    // �ִϸ��̼� �Ķ���͵�
    private const string PARAMNAME_MOVE = "_Move";
    private const string PARAMNAME_JUMP = "_Jump";
    private const string PARAMNAME_GROUNDED = "_Grounded";
    private const string PARAMNAME_DAMAGED = "_Damaged";
    private const string PARAMNAME_ATTACK = "_Attack";

    public event System.Action onLand;

    /// <summary>
    /// �̵� �ִϸ��̼� �Ķ���͸� �����ϴ� �޼����Դϴ�.
    /// </summary>
    /// <param name="normalizedMoveSpeed"> ����ȭ�� �ӷ�</param>
    public void UpdateMoveParam(float normalizedMoveSpeed)
    {
        SetFloat(PARAMNAME_MOVE, normalizedMoveSpeed);
    }
    
    /// <summary>
    /// ���� �ִϸ��̼��� �����ϴ� �޼����Դϴ�.
    /// </summary>
    public void TriggerJumpParam()
    {
        SetTrigger(PARAMNAME_JUMP);
    }

    /// <summary>
    /// ���� ����ִ����� ��Ÿ���� �Ķ���͸� �����ϴ� �޼����Դϴ�.
    /// </summary>
    /// <param name="isGrounded"> ���� ����ִ����� ����</param>
    public void UpdateGroundedParam(bool isGrounded)
    {
        SetBool(PARAMNAME_GROUNDED, isGrounded);
    }

    /// <summary>
    /// �ǰ� �ִϸ��̼��� �����ϴ� �޼����Դϴ�.
    /// </summary>
    public void TriggerDamagedParam()
    {
        SetTrigger(PARAMNAME_DAMAGED);
    }

    /// <summary>
    /// ���� �ִϸ��̼��� �����ϴ� �޼����Դϴ�.
    /// </summary>
    public void TriggerAttackParam()
    {
        SetTrigger(PARAMNAME_ATTACK);
    }

    public void AnimEvent_LandAttack()
    {
        onLand?.Invoke();
    }
}
