using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 유도탄을 나타내는 컴포넌트입니다.
/// </summary>
public class GuidedProjectile : Projectile
{
    /// <summary>
    /// 따라갈 목표 Transform 입니다.
    /// </summary>
    private Transform _TargetTransform;

    /// <summary>
    /// 이동하는 메서드입니다.
    /// </summary>
    protected override void Move()
    {
        // 목표 Transform 을 향해 방향을 설정합니다.
        if(_TargetTransform != null)
            _Direction = (_TargetTransform.position - transform.position).normalized;

        // 이동 방향으로 회전시키고 이동시킵니다.
        base.Move();
    }

    /// <summary>
    /// 목표를 설정하는 메서드입니다.
    /// </summary>
    /// <param name="target"> 목표 Transform </param>
    public void SetTarget(Transform target)
    {
        _TargetTransform = target;
    }
}
