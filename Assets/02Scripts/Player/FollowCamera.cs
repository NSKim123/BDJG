using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// ī�޶� ����� �����ϴ� ������ ����ϴ� ������Ʈ�Դϴ�.
/// </summary>
public class FollowCamera : MonoBehaviour
{
    private static float _CameraShake;

    [Header("# ��ġ ����")]
    [Header("�Ĺ� �Ÿ�")]
    public float m_RearDistance = 6.3f;

    [Header("����")]
    public float m_Height = 4.2f;

    [Header("x�� ȸ�� ��")]
    public float m_PitchRotation = 30.0f;

    /// <summary>
    /// �����ϴ� ���� ������Ʈ
    /// </summary>
    private GameObject _TargetObject;

    private float _Distance = 1.0f;


    private void Start()
    {
        // ������ �÷��̾ ã���ϴ�.
        // TODO : �ܺο��� ���� ����� �������־�� ��.
        _TargetObject = FindAnyObjectByType<PlayerControllerBase>().controlledCharacter.gameObject;

        // ī�޶� Pitch ȸ����ŵ�ϴ�.
        PitchRotate();
    }

    private void FixedUpdate()
    {
        // ����� �����մϴ�.
        FollowTarget();
    }

    /// <summary>
    /// ����� �����ϴ� �޼����Դϴ�.
    /// </summary>
    private void FollowTarget()
    {   
        // ���� ����� ��ġ�� ���� ���� ��ġ�� �����մϴ�.
        //transform.position = _TargetObject.transform.position + (-_TargetObject.transform.forward * m_Distance + Vector3.up * m_Height);
        transform.position = 
            _TargetObject.transform.position + 
            (-Vector3.forward * m_RearDistance + Vector3.up * m_Height) * AdjustDistanceByTargetScale()
            + Vector3.right * Mathf.Sin(Time.time * 90.0f) * _CameraShake;

        _CameraShake -= Time.deltaTime;

        if (_CameraShake < 0.0f)
        {
            _CameraShake = 0.0f;
        }        
    }

    private float AdjustDistanceByTargetScale()
    {
        _Distance = Mathf.Lerp(_Distance, (_TargetObject.transform.lossyScale.y - 1.0f) * 0.1f + 0.75f, 0.1f);
        return _Distance;
    }

    public static void ShakeCamera()
    {
        _CameraShake = 1.0f;
    }

    /// <summary>
    /// ī�޶��� Pitch ȸ����Ű�� �޼����Դϴ�.
    /// </summary>
    private void PitchRotate()
    {
        // ���� ȸ������ �����ɴϴ�.
        Vector3 currentEulerAngles = transform.rotation.eulerAngles;

        // ���Ӱ� ������ ȸ������ ����Ͽ� �����մϴ�.
        transform.rotation = Quaternion.Euler(m_PitchRotation, 0.0f, 0.0f);
    }
}
