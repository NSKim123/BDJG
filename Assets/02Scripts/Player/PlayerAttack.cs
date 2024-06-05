using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어의 캐릭터의 공격을 수행하는 컴포넌트입니다.
/// </summary>
public class PlayerAttack : MonoBehaviour
{
    [Header("# 탄환 발사 관련")]
    [Header("탄환 프래팹")]
    public Bullet m_Bullet;

    [Header("발사 속력")]
    public float m_BulletSpeed = 20.0f;

    [Header("밀어내는 힘")]
    public float m_PushPower = 5.0f;

    [Header("발사 시작 위치")]
    public Transform m_StartPosition;

    [Header("---------------------------------------")]

    [Header("# 스테미나 관련")]
    [Header("최대 스테미나")]
    public float m_MaxStamina = 100.0f;

    [Header("발사 시 소모 스테미나")]
    public float m_CostStamina = 10.0f;

    [Header("스테미나 자연 회복 속도 /s")]
    public float m_StaminaRecoverSpeed = 5.0f;

    /// <summary>
    /// 스태미나 게이지 객체
    /// </summary>
    private Gauge _StaminaGauge;

    /// <summary>
    /// 이 공격 컴포넌트를 가지고 있는 PlayerCharacter 객체입니다.
    /// </summary>
    private PlayerCharacter _OwnerCharacter;

    private void Awake()
    {
        // 스태미나 게이지를 초기화합니다.
        _StaminaGauge = new Gauge(m_MaxStamina, m_MaxStamina);

        // 이 공격 컴포넌트를 가지고 있는 PlayerCharacter 객체를 찾습니다.
        _OwnerCharacter = GetComponent<PlayerCharacter>();        
    }

    private void Update()
    {
        // 스태미나를 자연 회복합니다.
        RecoverStamina(m_StaminaRecoverSpeed * Time.deltaTime);
    }

    /// <summary>
    /// 스태미나를 회복합니다.
    /// </summary>
    /// <param name="recover"> 회복량입니다.</param>
    private void RecoverStamina(float recover)
    {
        _StaminaGauge.currentValue += recover;
    }

    /// <summary>
    /// 공격을 시도합니다.
    /// </summary>
    private void TryAttack()
    {
        // 스태미나가 부족하면 발사하지 않고 리턴합니다.
        if (_StaminaGauge.currentValue - m_CostStamina < _StaminaGauge.min)
        {
            Debug.Log("스테미나가 부족해 발사할 수 없습니다!");
            return;
        }

        // 발사합니다.
        Fire();
    }

    /// <summary>
    /// 발사하는 메서드입니다.
    /// </summary>
    private void Fire()
    {
        // 스태미나를 소모합니다.
        CostStamina(m_CostStamina);

        // 투사체를 생성합니다.
        InstantiateBullet();
    }

    /// <summary>
    /// 스태미나를 소모합니다.
    /// </summary>
    /// <param name="cost"> 소모량입니다.</param>
    private void CostStamina(float cost)
    {
        _StaminaGauge.currentValue -= cost;
    }

    /// <summary>
    /// 투사체를 발사합니다.
    /// </summary>
    private void InstantiateBullet()
    {
        Bullet bullet = Instantiate(m_Bullet);
        bullet.transform.position = m_StartPosition.position;
        bullet.SetProjectile(this.gameObject, transform.forward, m_BulletSpeed);
    }

    /// <summary>
    /// 공격 입력을 받았을 때 호출되는 메서드입니다.
    /// 투사체 발사를 시도합니다.
    /// </summary>
    public void OnAttackInput()
    {
        TryAttack();        
    }
}
