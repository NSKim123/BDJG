using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 슬라임이 발사하는 투사체입니다.
/// </summary>
public class Bullet : GuidedProjectile
{
    protected static string SOUNDNAME_DAMAGED = "Effect_Damaged";

    /// <summary>
    /// 이 투사체가 가질 공격력
    /// </summary>
    protected float _AttackPower;

    protected virtual void Awake()
    {
        Destroy(this.gameObject, 5.0f);
        //Invoke("DestroyBullet", 5.0f);
    }

    //private void OnEnable()
    //{
    //    Invoke("DestroyBullet", 5.0f);

    //}


    protected virtual void OnTriggerEnter(Collider collider)
    {
        // 플레이어와의 충돌은 무시합니다.
        if (collider.gameObject.tag == "Player") return;

        // 지형과의 충돌도 무시합니다,
        if (collider.gameObject.layer == LayerMask.GetMask("Ground")) return;

        // 이 투사체가 적에게 닿으면
        if (collider.gameObject.TryGetComponent<IHit>(out IHit hitEnemy))
        {
            _Direction.y = 0.0f;
            _Direction.Normalize();
            hitEnemy.OnDamaged(_AttackPower, _Direction);

            PlaySound(collider);

            // 이 투사체를 제거합니다.
            Destroy(this.gameObject);
            //DestroyBullet();
        }
    }

    private void DestroyBullet()
    {
        Debug.Log("삭제", this);
        ObjectPoolManager.Instance.ReturnToPool(this.gameObject);
    }

    protected virtual float CalculateSoundPitch(Collider other)
    {
        return Mathf.Pow(2.0f, -(_Owner.transform.position - other.transform.position).magnitude);
    }

    protected virtual void PlaySound(Collider other)
    {
        SoundManager.Instance.PlaySound("Effect_Damaged", SoundType.Effect);
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
