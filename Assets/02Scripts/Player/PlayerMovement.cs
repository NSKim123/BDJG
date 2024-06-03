using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("# 이동 관련")]
    public float m_Speed = 5.0f;
    public float m_RotateSpeedInDegree = 180.0f;

    private Vector3 _LookDirection;

    private Vector3 _InputDirection;

    private CharacterController _CharacterController;

    public CharacterController characterController => _CharacterController ?? (_CharacterController = GetComponent<CharacterController>());


    private void FixedUpdate()
    {
        CalculateXZVelocity();

        CalculateYVelocity();

        CalculateLookDirection();
    }

    private void CalculateXZVelocity()
    {

    }

    private void CalculateYVelocity()
    {

    }

    private void CalculateLookDirection()
    {

    }

    public void OnMoveInput(Vector2 inputDirection)
    {
        _InputDirection = inputDirection;
    }

    public void OnRotateInput()
    {

    }
}
