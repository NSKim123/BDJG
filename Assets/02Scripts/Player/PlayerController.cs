using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 플레이어의 입력을 받아 플레이어의 캐릭터를 조종하는 객체의 컴포넌트입니다.
/// </summary>
public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// 조종하는 플레이어의 캐릭터 객체
    /// </summary>
    private PlayerCharacter _ControlledCharacter;

    /// <summary>
    /// 조종하는 플레이어의 캐릭터 객체에 대한 읽기 전용 컴포넌트입니다.
    /// </summary>
    public PlayerCharacter controlledCharacter => _ControlledCharacter;

    private void Awake()
    {
        // 조종할 캐릭터를 찾습니다.
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
