using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����ü�� ���� ������Ʈ�Դϴ�.
/// </summary>
public class Projectile : MonoBehaviour
{
    /// <summary>
    /// �� ����ü�� ����
    /// </summary>
    protected GameObject _Owner;

    /// <summary>
    /// ����ü�� �̵�����
    /// </summary>
    protected Vector3 _Direction = Vector3.forward;

    /// <summary>
    /// ����ü�� �̵��ӷ�
    /// </summary>
    protected float _Speed;

    protected void FixedUpdate()
    {
        // �̵��մϴ�.
        Move();
    }

    /// <summary>
    /// �̵��������� ȸ����Ű�� �̵���Ű�� �޼����Դϴ�.
    /// </summary>
    protected void Move()
    {        
        transform.forward = _Direction;
        transform.position += _Direction * _Speed * Time.fixedDeltaTime;
    }

    /// <summary>
    /// �� ����ü�� �����ϴ� �޼����Դϴ�.
    /// </summary>
    /// <param name="owner"> ������ ����ü ����</param>
    /// <param name="direction"> ������ �̵� ����</param>
    /// <param name="speed"> ������ �̵� �ӷ�</param>
    public void SetProjectile(GameObject owner, Vector3 direction, float speed)
    {
        _Owner = owner;
        _Direction = direction.normalized;
        _Speed = speed;

        transform.forward = _Direction;
    }
}
