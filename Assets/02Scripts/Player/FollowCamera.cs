using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ī�޶� ����� �����ϴ� ������ ����ϴ� ������Ʈ�Դϴ�.
/// </summary>
public class FollowCamera : MonoBehaviour
{
    [Header("# ��ġ ����")]
    [Header("�Ĺ� �Ÿ�")]
    public float m_Distance = 12.6f;

    [Header("����")]
    public float m_Height = 8.4f;

    [Header("x�� ȸ�� ��")]
    public float m_PitchRotation = 30.0f;

    /// <summary>
    /// �����ϴ� ���� ������Ʈ
    /// </summary>
    private GameObject _TargetObject;

    private void Start()
    {
        // ������ �÷��̾ ã���ϴ�.
        // TODO : �ܺο��� ���� ����� �������־�� ��.
        _TargetObject = FindAnyObjectByType<PlayerCharacter>().gameObject;
    }

    private void FixedUpdate()
    {
        // ����� �����մϴ�.
        FollowTarget();

        // ī�޶��� Pitch ȸ����ŵ�ϴ�.
        PitchRotate();
    }

    /// <summary>
    /// ����� �����ϴ� �޼����Դϴ�.
    /// </summary>
    private void FollowTarget()
    {   
        // ���� ����� ��ġ�� ���� ���� ��ġ�� �����մϴ�.
        //transform.position = _TargetObject.transform.position + (-_TargetObject.transform.forward * m_Distance + Vector3.up * m_Height);
        transform.position = _TargetObject.transform.position + (-Vector3.forward * m_Distance + Vector3.up * m_Height);
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
