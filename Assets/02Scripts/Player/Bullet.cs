using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �������� �߻��ϴ� ����ü�Դϴ�.
/// </summary>
public class Bullet : Projectile
{
    private void OnCollisionEnter(Collision collision)
    {
        // �� ����ü�� ������ ������
        if(collision.gameObject.TryGetComponent<Enemy>(out Enemy hitEnemy))
        {
            // TO DO : �� �з��� ó�� 

            // �� ����ü�� �����մϴ�.
            Destroy(this.gameObject);
        }
    }
}
