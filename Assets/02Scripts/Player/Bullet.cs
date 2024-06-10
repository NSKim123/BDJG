using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 슬라임이 발사하는 투사체입니다.
/// </summary>
public class Bullet : Projectile
{
    private void OnCollisionEnter(Collision collision)
    {
        // 이 투사체가 적에게 닿으면
        if(collision.gameObject.TryGetComponent<Enemy>(out Enemy hitEnemy))
        {
            // TO DO : 적 밀려남 처리 

            // 이 투사체를 제거합니다.
            Destroy(this.gameObject);
        }
    }
}
