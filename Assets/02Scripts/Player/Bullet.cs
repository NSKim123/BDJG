using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �������� �߻��ϴ� ����ü�Դϴ�.
/// </summary>
public class Bullet : GuidedProjectile
{
    /// <summary>
    /// �� ����ü�� ���� ���ݷ�
    /// </summary>
    private float _AttackPower;
    
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player") return;

        // �� ����ü�� ������ ������
        if (collision.gameObject.TryGetComponent<IHit>(out IHit hitEnemy))
        {
            // TO DO : �� �з��� ó�� 
            // ���߿� ������Ʈ Merge �� �۾�!
        }

        // �� ����ü�� �����մϴ�.
        Destroy(this.gameObject);
    }

    /// <summary>
    /// ���ݷ� ���� �޼���
    /// </summary>
    /// <param name="power"> ������ ���ݷ�</param>
    public void SetAttackPower(float power)
    {
        _AttackPower = power;
    }    
}
