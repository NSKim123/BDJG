using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����ź�� ��Ÿ���� ������Ʈ�Դϴ�.
/// </summary>
public class GuidedProjectile : Projectile
{
    /// <summary>
    /// ���� ��ǥ Transform �Դϴ�.
    /// </summary>
    private Transform _TargetTransform;

    /// <summary>
    /// �̵��ϴ� �޼����Դϴ�.
    /// </summary>
    protected override void Move()
    {
        // ��ǥ Transform �� ���� ������ �����մϴ�.
        if(_TargetTransform != null)
            _Direction = (_TargetTransform.position - transform.position).normalized;

        // �̵� �������� ȸ����Ű�� �̵���ŵ�ϴ�.
        base.Move();
    }

    /// <summary>
    /// ��ǥ�� �����ϴ� �޼����Դϴ�.
    /// </summary>
    /// <param name="target"> ��ǥ Transform </param>
    public void SetTarget(Transform target)
    {
        _TargetTransform = target;
    }
}
