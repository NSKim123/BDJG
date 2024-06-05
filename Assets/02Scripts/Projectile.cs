using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 투사체에 대한 컴포넌트입니다.
/// </summary>
public class Projectile : MonoBehaviour
{
    /// <summary>
    /// 이 투사체의 주인
    /// </summary>
    protected GameObject _Owner;

    /// <summary>
    /// 투사체의 이동방향
    /// </summary>
    protected Vector3 _Direction = Vector3.forward;

    /// <summary>
    /// 투사체의 이동속력
    /// </summary>
    protected float _Speed;

    protected void FixedUpdate()
    {
        // 이동합니다.
        Move();
    }

    /// <summary>
    /// 이동방향으로 회전시키고 이동시키는 메서드입니다.
    /// </summary>
    protected void Move()
    {        
        transform.forward = _Direction;
        transform.position += _Direction * _Speed * Time.fixedDeltaTime;
    }

    /// <summary>
    /// 이 투사체를 설정하는 메서드입니다.
    /// </summary>
    /// <param name="owner"> 설정할 투사체 주인</param>
    /// <param name="direction"> 설정할 이동 방향</param>
    /// <param name="speed"> 설정할 이동 속력</param>
    public void SetProjectile(GameObject owner, Vector3 direction, float speed)
    {
        _Owner = owner;
        _Direction = direction.normalized;
        _Speed = speed;

        transform.forward = _Direction;
    }
}
