using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어의 애니메이터를 관리하는 컴포넌트
/// </summary>
public class PlayerAnimController : AnimController
{
    // 애니메이션 파라미터들
    private const string PARAMNAME_MOVE = "_Move";
    private const string PARAMNAME_JUMP = "_Jump";
    private const string PARAMNAME_GROUNDED = "_Grounded";
    private const string PARAMNAME_DAMAGED = "_Damaged";
    private const string PARAMNAME_ATTACK = "_Attack";

    public event System.Action onLand;

    /// <summary>
    /// 이동 애니메이션 파라미터를 설정하는 메서드입니다.
    /// </summary>
    /// <param name="normalizedMoveSpeed"> 정규화된 속력</param>
    public void UpdateMoveParam(float normalizedMoveSpeed)
    {
        SetFloat(PARAMNAME_MOVE, normalizedMoveSpeed);
    }
    
    /// <summary>
    /// 점프 애니메이션을 실행하는 메서드입니다.
    /// </summary>
    public void TriggerJumpParam()
    {
        SetTrigger(PARAMNAME_JUMP);
    }

    /// <summary>
    /// 땅에 닿아있는지를 나타내는 파라미터를 설정하는 메서드입니다.
    /// </summary>
    /// <param name="isGrounded"> 땅에 닿아있는지를 전달</param>
    public void UpdateGroundedParam(bool isGrounded)
    {
        SetBool(PARAMNAME_GROUNDED, isGrounded);
    }

    /// <summary>
    /// 피격 애니메이션을 실행하는 메서드입니다.
    /// </summary>
    public void TriggerDamagedParam()
    {
        SetTrigger(PARAMNAME_DAMAGED);
    }

    /// <summary>
    /// 공격 애니메이션을 실행하는 메서드입니다.
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
