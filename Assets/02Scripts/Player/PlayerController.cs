using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// �÷��̾��� �Է��� �޾� �÷��̾��� ĳ���͸� �����ϴ� ��ü�� ������Ʈ�Դϴ�.
/// </summary>
public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// �����ϴ� �÷��̾��� ĳ���� ��ü
    /// </summary>
    private PlayerCharacter _ControlledCharacter;

    /// <summary>
    /// �����ϴ� �÷��̾��� ĳ���� ��ü�� ���� �б� ���� ������Ʈ�Դϴ�.
    /// </summary>
    public PlayerCharacter controlledCharacter => _ControlledCharacter;

    private void Awake()
    {
        // ������ ĳ���͸� ã���ϴ�.
        _ControlledCharacter = FindAnyObjectByType<PlayerCharacter>();
    }

    private void OnTurn(InputValue inputValue)
    {
        controlledCharacter?.OnTurnInput(inputValue.Get<Vector2>());
    }

    private void OnMove(InputValue inputValue)
    {
        controlledCharacter?.OnMoveInput(inputValue.Get<Vector2>());
    }

    private void OnJump()
    {
        controlledCharacter?.OnJumpInput();
    }

    private void OnAttack()
    {
        controlledCharacter?.OnAttackInput();
    }
}
