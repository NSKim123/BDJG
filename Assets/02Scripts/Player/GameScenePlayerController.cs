using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// �÷��̾��� �Է��� �޾� �÷��̾��� ĳ���͸� �����ϴ� ��ü�� ������Ʈ�Դϴ�.
/// </summary>
public class GameScenePlayerController : PlayerControllerBase
{
    /// <summary>
    /// �����ϴ� �÷��̾��� ĳ���� ��ü�� ���� �б� ���� ������Ʈ�Դϴ�.
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
