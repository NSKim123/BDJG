using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 슬라임이 발사하는 투사체입니다.
/// </summary>
public class Bullet : GuidedProjectile
{   
    /// <summary>
    /// 이 투사체가 가질 공격력
    /// </summary>
    private float _AttackPower;

    private void Awake()
    {
        Destroy(this.gameObject, 5.0f);
    }

    private void OnTriggerEnter(Collider collider)
    {
        // 플레이어와의 충돌은 무시합니다.
        if (collider.gameObject.tag == "Player") return;

        // 이 투사체가 적에게 닿으면
        if (collider.gameObject.TryGetComponent<IHit>(out IHit hitEnemy))
        {
            hitEnemy.OnDamaged(_AttackPower, _Direction);            
        }

        // 이 투사체를 제거합니다.
        Destroy(this.gameObject);
    }


    /// <summary>
    /// 공격력 설정 메서드
    /// </summary>
    /// <param name="power"> 설정할 공격력</param>
    public void SetAttackPower(float power)
    {
        _AttackPower = power;
    }    
}
