using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어의 캐릭터에 대한 컴포넌트입니다.
/// </summary>
public class PlayerCharacter : MonoBehaviour
{
    /// <summary>
    /// 이 캐릭터의 레벨
    /// </summary>
    private int _Level;

    /// <summary>
    /// 이동 컴포넌트
    /// </summary>
    private PlayerMovement _PlayerMovement;

    /// <summary>
    /// 공격 컴포넌트
    /// </summary>
    private PlayerAttack _PlayerAttack;

    /// <summary>
    /// 이동 컴포넌트에 대한 읽기 전용 프로퍼티입니다.
    /// </summary>
    public PlayerMovement movementComponent => _PlayerMovement ?? (_PlayerMovement = GetComponent<PlayerMovement>());

    /// <summary>
    /// 공격 컴포넌트에 대한 읽기 전용 프로퍼티입니다.
    /// </summary>
    public PlayerAttack attackComponent => _PlayerAttack ?? (_PlayerAttack = GetComponent<PlayerAttack>());

    private void Awake()
    {
        _Level = 1;
    }

    /// <summary>
    /// 이동 입력을 받았을 때 호출되는 메서드입니다.
    /// </summary>
    /// <param name="inputDirection"> 입력받은 이동 방향입니다.</param>
    public void OnMoveInput(Vector2 inputDirection)
    {
        movementComponent.OnMoveInput(inputDirection);
    }

    /// <summary>
    /// 회전 입력을 받았을 때 호출되는 메서드입니다.
    /// </summary>
    /// <param name="inputDelta"> 입력받은 회전 값입니다.</param>
    public void OnTurnInput(Vector2 inputDelta)
    {
        movementComponent.OnTurnInput(inputDelta.x);
    }

    /// <summary>
    /// 점프 입력을 받았을 때 호출되는 메서드입니다.
    /// </summary>
    public void OnJumpInput()
    {
        movementComponent?.OnJumpInput();
    }

    /// <summary>
    /// 공격 입력을 받았을 때 호출되는 메서드입니다.
    /// </summary>
    public void OnAttackInput()
    {
        attackComponent?.OnAttackInput();
    }
}
