using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 카메라가 대상을 추적하는 역할을 담당하는 컴포넌트입니다.
/// </summary>
public class FollowCamera : MonoBehaviour
{
    [Header("# 위치 관련")]
    [Header("후방 거리")]
    public float m_Distance = 12.6f;

    [Header("높이")]
    public float m_Height = 8.4f;

    [Header("x축 회전 값")]
    public float m_PitchRotation = 30.0f;

    /// <summary>
    /// 추적하는 게임 오브젝트
    /// </summary>
    private GameObject _TargetObject;

    private void Start()
    {
        // 추적할 플레이어를 찾습니다.
        // TODO : 외부에서 추적 대상을 설정해주어야 함.
        _TargetObject = FindAnyObjectByType<PlayerCharacter>().gameObject;
    }

    private void FixedUpdate()
    {
        // 대상을 추적합니다.
        FollowTarget();

        // 카메라의 Pitch 회전시킵니다.
        PitchRotate();
    }

    /// <summary>
    /// 대상을 추적하는 메서드입니다.
    /// </summary>
    private void FollowTarget()
    {   
        // 추적 대상의 위치에 따라 다음 위치를 설정합니다.
        //transform.position = _TargetObject.transform.position + (-_TargetObject.transform.forward * m_Distance + Vector3.up * m_Height);
        transform.position = _TargetObject.transform.position + (-Vector3.forward * m_Distance + Vector3.up * m_Height);
    }

    /// <summary>
    /// 카메라의 Pitch 회전시키는 메서드입니다.
    /// </summary>
    private void PitchRotate()
    {
        // 현재 회전값을 가져옵니다.
        Vector3 currentEulerAngles = transform.rotation.eulerAngles;

        // 새롭게 설정할 회전값을 계산하여 적용합니다.
        transform.rotation = Quaternion.Euler(m_PitchRotation, 0.0f, 0.0f);
    }
}
