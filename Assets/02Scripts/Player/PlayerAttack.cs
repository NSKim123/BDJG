using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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

    [Header("밀어내는 힘 계수")]
    public float m_PushPowerMultiplier = 1.0f;

    [Header("공격 쿨타임")]
    public float m_AttackReuseTime = 0.75f;

    [Header("------------------------------------------------------------------------------")]

    [Header("# 탄환 게이지 관련")]
    [Header("발사 시 소모 게이지")]
    public int m_CostBulletGauge = 1;

    [Header("1회 게이지 회복량")]
    public int m_BulletGaugeRecoverAmount = 2;

    [Header("회복 주기")]
    public float m_BulletGaugeRecoverCycle = 1.0f;

    [Header("공격 후 게이지 회복을 시작하는 시간")]
    public float m_BulletGaugeStartRecoverTime = 1.0f;    
    

    [Header("------------------------------------------------------------------------------")]

    [Header("# 타겟팅 관련")]
    [Header("감지 최대 길이")]
    public float m_SencerDistance = 20.0f;

    [Header("감지 폭")]
    public float m_SencerWidth = 2.0f;

    [Header("감지 높이")]
    public float m_SencerHeight = 2.0f;

    [Header("감지 레이어")]
    public LayerMask m_SenceLayer;

    /// <summary>
    /// 공격력 ( 미는 힘. 레벨에 따라 성장 )
    /// </summary>
    private float _AttackForce;

    /// <summary>
    /// 발사 시작 위치
    /// </summary>
    private Transform _StartPosition;

    /// <summary>
    /// 쿨타임 
    /// </summary>
    private FloatGauge _ReuseTimeGuage;

    /// <summary>
    /// 탄환 게이지 객체
    /// </summary>
    private BulletGauge _BulletGauge;

    /// <summary>
    /// 타겟팅 시스템 객체
    /// </summary>
    private TargetingSystem _TargetingSystem;

    /// <summary>
    /// 이 공격 컴포넌트를 가지고 있는 PlayerCharacter 객체입니다.
    /// </summary>
    private PlayerCharacter _OwnerCharacter;

    public bool isAttacktable => CheckAttackable();

    public FloatGauge reuseTimeGauge => _ReuseTimeGuage;

    /// <summary>
    /// 탄환 게이지 객체에 대한 읽기 전용 프로퍼티입니다.
    /// </summary>
    public BulletGauge bulletGauge => _BulletGauge;

    private void Awake()
    {
        _ReuseTimeGuage = new FloatGauge(m_AttackReuseTime);

        // 탄환 게이지를 생성합니다.
        _BulletGauge = new BulletGauge(m_BulletGaugeRecoverAmount, m_BulletGaugeRecoverCycle, m_BulletGaugeStartRecoverTime, 10);
                
        // 타겟팅 시스템을 생성합니다.
        _TargetingSystem = new TargetingSystem(LayerMask.GetMask("Enemy"), m_SencerDistance, m_SencerWidth, m_SencerHeight);               
        
        // 이 공격 컴포넌트를 가지고 있는 PlayerCharacter 객체를 찾습니다.
        _OwnerCharacter = GetComponent<PlayerCharacter>();        
    }

    private void Update()
    {
        UpdateResueTimeGauge();

        // 탄환 게이지를 업데이트합니다.
        _BulletGauge.UpdateBulletGauge();
    }

    private void FixedUpdate()
    {
        // 타켓팅 대상을 지정합니다.
        _TargetingSystem.Targeting(transform);        
    }

    private void UpdateResueTimeGauge()
    {
        if (_ReuseTimeGuage.currentValue == _ReuseTimeGuage.min) return;

        _ReuseTimeGuage.currentValue -= Time.deltaTime;
    }

    /// <summary>
    /// 공격을 시도합니다.
    /// </summary>
    private void TryAttack()
    {
        // 공격이 불가능한 상태라면 호출을 중단합니다.
        if (!CheckAttackable())
            return;

        if (_ReuseTimeGuage.currentValue > 0.0f) 
            return;

        // 발사합니다.
        Fire();
    }

    /// <summary>
    /// 탄환이 남아있는지 체크합니다.
    /// </summary>
    /// <returns> 탄환이 남아있다면 참을 반환합니다.</returns>
    private bool CheckRemainedBullet()
    {
        return !(_BulletGauge.currentValue - m_CostBulletGauge < _BulletGauge.min);
    }

    /// <summary>
    /// 탄환 게이지가 과부하 상태인지 체크합니다.
    /// </summary>
    /// <returns> 과부하 상태라면 참을 반환합니다.</returns>
    private bool CheckOverburden()
    {
        return _BulletGauge.isOverburden;
    }

    /// <summary>
    /// 넉백 상태인지 확인합니다.
    /// </summary>
    /// <returns> 넉백 상태라면 참을 반환합니다.</returns>
    private bool CheckKnockBack()
    {
        return _OwnerCharacter.movementComponent.isKnockBack;
    }    

    private bool CheckStunned()
    {
        return _OwnerCharacter.isStunned;
    }

    /// <summary>
    /// 공격 가능한 상태인지 확인합니다.
    /// </summary>
    /// <returns> 공격 가능한 상태라면 참을 반환합니다.</returns>
    private bool CheckAttackable()
    {
        return CheckRemainedBullet() && !CheckOverburden() && !CheckKnockBack() && !CheckStunned();
    }

    /// <summary>
    /// 발사하는 메서드입니다.
    /// </summary>
    private void Fire()
    {
        // 탄환 게이지를 소모합니다.
        CostStamina(m_CostBulletGauge);

        // 투사체를 생성합니다.
        InstantiateBullet();

        // 공격 애니메이션을 재생합니다.
        _OwnerCharacter.animController.TriggerAttackParam();

        _ReuseTimeGuage.currentValue = _ReuseTimeGuage.max;
    }

    /// <summary>
    /// 탄환 게이지를 소모합니다.
    /// </summary>
    /// <param name="cost"> 소모량</param>
    private void CostStamina(int cost)
    {
        _BulletGauge.CostBullet(cost);
    }

    /// <summary>
    /// 탄환를 발사합니다.
    /// </summary>
    private void InstantiateBullet()
    {
        Bullet bullet = Instantiate(m_Bullet);
        bullet.transform.position = _StartPosition.position;
        bullet.SetProjectile(this.gameObject, transform.forward, m_BulletSpeed);
        bullet.SetAttackPower(_AttackForce * m_PushPowerMultiplier);

        if(_TargetingSystem.currentTargetTransform != null)
            bullet.SetTarget(_TargetingSystem.currentTargetTransform);
    }

    /// <summary>
    /// 레벨에 따른 공격력을 설정합니다.
    /// </summary>
    /// <param name="level"> 달성 레벨</param>
    private void SetAttackForceByLevel(int level)
    {        
        _AttackForce = level;
    }

    /// <summary>
    /// 레벨에 따른 탄환 게이지 최대량을 설정하고, 게이지를 100% 채웁니다.
    /// </summary>
    /// <param name="level"> 달성 레벨</param>
    private void SetBulletGaugeMaxValueByLevel(int level)
    {
        // 최대량 계산 식
        _BulletGauge.max = 10 + level * 2;
        _BulletGauge.currentValue = _BulletGauge.max;
    }

    /// <summary>
    /// 발사 시작위치를 찾습니다.
    /// </summary>
    private void FindStartPoint()
    {
        _StartPosition = _OwnerCharacter.modelComponent.currentModel.transform.Find("FirePoint");
    }

    /// <summary>
    /// 레벨업 시 호출될 메서드입니다.
    /// </summary>
    /// <param name="level"> 달성 레벨</param>
    public void OnLevelUp(int level)
    {
        SetAttackForceByLevel(level);
        SetBulletGaugeMaxValueByLevel(level);
        FindStartPoint();
    }

    /// <summary>
    /// 공격 입력을 받았을 때 호출되는 메서드입니다.
    /// 투사체 발사를 시도합니다.
    /// </summary>
    public void OnAttackInput()
    {
        TryAttack();        
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_BulletGauge == null) return;

        GUIContent gUIContent = new GUIContent();
        gUIContent.text = $"\n\n\n\n\n공격력 : {_AttackForce}\n탄환 게이지 : {_BulletGauge.currentValue} / {_BulletGauge.max}";
        Handles.Label(transform.position + Vector3.down, gUIContent);

        
    }
#endif
}
