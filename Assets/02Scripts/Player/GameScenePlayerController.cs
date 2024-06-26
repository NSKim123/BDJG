using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 플레이어의 입력을 받아 플레이어의 캐릭터를 조종하는 객체의 컴포넌트입니다.
/// </summary>
public class GameScenePlayerController : PlayerControllerBase
{
    /// <summary>
    /// 조종하는 플레이어의 캐릭터 객체에 대한 읽기 전용 컴포넌트입니다.
    /// </summary>
    public new PlayerCharacter controlledCharacter => base.controlledCharacter as PlayerCharacter;

    public void OnUseItem()
    {
        controlledCharacter?.OnUseItemInput();
    }

    public void OnMove(InputValue inputValue)
    {
        controlledCharacter?.OnMoveInput(inputValue.Get<Vector2>());
    }

    public void OnMove(Vector2 inputValue)
    {
        controlledCharacter?.OnMoveInput(inputValue);
    }

    public void OnJump()
    {
        controlledCharacter?.OnJumpInput();
    }

    public void OnAttack()
    {
        controlledCharacter?.OnAttackInput();
    }
}
