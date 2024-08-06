using UnityEditor;
using UnityEngine;

/// <summary>
/// 슬라임이 발사하는 투사체의 종류를 나타내는 열겨형 타입입니다.
/// </summary>
public enum BulletType
{
    Basic,
    Shell,
}

/// <summary>
/// 플레이어의 캐릭터의 공격을 수행하는 컴포넌트입니다. 
/// 발사, 탄환 게이지, 타겟팅 시스템 관련 부분이 작성되어있습니다.
/// </summary>
/// 탄환 발사 관련 부분
public partial class PlayerAttack : MonoBehaviour
{
    private static string SOUNDNAME_FIRE_BASIC = "Effect_FIre_ver2";
    private static string SOUNDNAME_FIRE_SHELL = "Effect_FireShell";

    [Header("# 탄환 발사 관련")]
    [Header("포탄 프리팹")]
    public Bullet m_Shell;

    [Header("발사 이펙트")]
    public GameObject m_Effect_Fire;

    [Header("발사 속력")]
    public float m_BulletSpeed = 20.0f;

    [Header("밀어내는 힘 계수")]
    public float m_PushPowerMultiplier = 1.0f;

    [Header("공격 쿨타임")]
    public float m_AttackReuseTime = 0.2f;

    [Header("------------------------------------------------------------------------------")]

    /// <summary>
    /// 공격력 ( 미는 힘. 레벨에 따라 성장 )
    /// </summary>
    private float _AttackForce;

    /// <summary>
    /// 발사를 강제로 제한하는지를 나타냅니다.
    /// </summary>
    private bool _ProhibitFire;

    /// <summary>
    /// 현재 발사 사운드의 이름을 나타냅니다.
    /// </summary>
    private string _CurrentFireSoundName;

    /// <summary>
    /// 발사 시작 위치
    /// </summary>
    private Transform _StartPosition;

    /// <summary>
    /// 쿨타임 게이지 객체
    /// </summary>
    private FloatGauge _ReuseTimeGuage;    

    /// <summary>
    /// 타겟팅 시스템 객체
    /// </summary>
    private TargetingSystem _TargetingSystem;

    /// <summary>
    /// 이 공격 컴포넌트를 가지고 있는 PlayerCharacter 객체입니다.
    /// </summary>
    private PlayerCharacter _OwnerCharacter;

    /// <summary>
    /// 현재 사용하는 탄환 종류
    /// </summary>
    private BulletType _CurrentBullet;

    /// <summary>
    /// 공격 가능한 상황인지를 나타내는 읽기전용 프로퍼티입니다.
    /// </summary>
    public bool isAttacktable => CheckFire();

    /// <summary>
    /// 쿨타임 게이지에 대한 읽기 전용 프로퍼티입니다.
    /// </summary>
    public FloatGauge reuseTimeGauge => _ReuseTimeGuage;
    

    private void Awake()
    {
        // 쿨타임 게이지 객체를 생성합니다.
        _ReuseTimeGuage = new FloatGauge(m_AttackReuseTime);

        // 탄환 게이지를 생성합니다.
        _BulletGauge = new BulletGauge(m_BulletGaugeRecoverAmount, m_BulletGaugeRecoverCycle, m_BulletGaugeStartRecoverTime, 10);
                
        // 타겟팅 시스템을 생성합니다.
        _TargetingSystem = new TargetingSystem(m_SenceLayer, m_SencerDistance, m_SencerWidth, m_SencerHeight);               
        
        // 이 공격 컴포넌트를 가지고 있는 PlayerCharacter 객체를 찾습니다.
        _OwnerCharacter = GetComponent<PlayerCharacter>();

        // 현재 탄환을 기본 탄환으로 설정합니다.
        ChangeBullet(BulletType.Basic);
    }

    private void Update()
    {
        // 쿨타임을 갱신합니다.
        UpdateResueTimeGauge();

        // 탄환 게이지를 갱신합니다.
        UpdateBulletGauge();
    }

    private void FixedUpdate()
    {
        // 타켓팅 대상을 업데이트합니다.
        UpdateTargeting();
    }

    /// <summary>
    /// Attack 컴포넌트를 리셋하는 메서드입니다.
    /// </summary>
    public void ResetAttackComponent()
    {
        // 재사용 대기시간을 리셋합니다.
        _ReuseTimeGuage.max = m_AttackReuseTime;

        // 회복 불가 상태를 해제합니다.
        _BulletGauge.SwitchProhibitRecover(false);

        // 현재 탄환을 기본 탄환으로 변경합니다.
        ChangeBullet(BulletType.Basic);

        // 발사 제한을 해제합니다.
        _ProhibitFire = false;
    }

    /// <summary>
    /// 쿨타임 게이지를 갱신하는 메서드입니다.
    /// </summary>
    private void UpdateResueTimeGauge()
    {
        // 이미 쿨타임이 다 돌았다면 호출을 종료합니다.
        if (_ReuseTimeGuage.currentValue == _ReuseTimeGuage.min) return;

        // 쿨타임을 감소시킵니다.
        _ReuseTimeGuage.currentValue -= Time.deltaTime;
    }

    /// <summary>
    /// 공격을 시도하는 메서드입니다.
    /// </summary>
    private void TryAttack()
    {
        // 공격이 불가능한 상태이거나 쿨타임이 남아있다면 호출을 종료합니다.
        if (!CheckFire() || _ReuseTimeGuage.currentValue > 0.0f)
            return;

        // 발사합니다.
        Fire();
    }    

    /// <summary>
    /// 넉백 상태인지 체크하는 메서드입니다.
    /// </summary>
    /// <returns> 넉백 상태라면 참을 반환합니다.</returns>
    private bool CheckKnockBack()
    {
        return _OwnerCharacter.movementComponent.isKnockBack;
    }    

    /// <summary>
    /// 행동 불가 상태인지 체크하는 메서드입니다.
    /// </summary>
    /// <returns> 행동 불가 상태라면 참을 반환합니다.</returns>
    private bool CheckStunned()
    {
        return _OwnerCharacter.isStunned;
    }

    /// <summary>
    /// 발사 가능한 상태인지 확인하는 메서드입니다.
    /// </summary>
    /// <returns> 공격 가능한 상태라면 참을 반환합니다.</returns>
    private bool CheckFire()
    {
        return CheckRemainedBullet() && !CheckOverburden() && !CheckKnockBack() && !CheckStunned() && !_ProhibitFire;
    }

    /// <summary>
    /// 탄환을 발사하는 메서드입니다.
    /// </summary>
    private void Fire()
    {
        // 탄환 게이지를 소모합니다.
        CostBulletGauge(m_CostBulletGauge);

        // 투사체를 생성합니다.
        InstantiateBullet();

        // 공격 애니메이션을 재생합니다.
        _OwnerCharacter.animController.TriggerAttackParam();

        // 발사 이펙트 생성
        InstantiateEffect();

        // 쿨타임을 돌리기 시작합니다.
        _ReuseTimeGuage.currentValue = _ReuseTimeGuage.max;

        // 발사 사운드를 재생합니다.
        PlayFireSound();
    }

    

    /// <summary>
    /// 현재 탄환 종류에 따라 탄환을 생성하는 메서드입니다.
    /// </summary>
    private void InstantiateBullet()
    {
        // 발사할 탄환
        Bullet bullet = null;

        // 현재 탄환의 종류에 따라 발사할 탄환을 결정합니다.
        switch (_CurrentBullet)
        {            
            // 기본 탄환
            case BulletType.Basic:
                // 오브젝트 풀에서 기본 탄환 하나를 가져옵니다.
                GameObject obj = ObjectPoolManager.Instance.GetFromPool(EPoolType.Bullet);
                bullet = obj.GetComponent<Bullet>();                
                obj.SetActive(true);
                break;
            // 포탄
            case BulletType.Shell:
                // 포탄 프리팹을 복사 생성합니다.
                bullet = Instantiate(m_Shell);                
                break;
        }

        // 탄환의 위치, 속도, 공격력을 설정합니다.
        bullet.transform.position = _StartPosition.position;
        bullet.SetProjectile(this.gameObject, transform.forward, m_BulletSpeed);
        bullet.SetAttackPower(_AttackForce * m_PushPowerMultiplier);

        // 탄환의 목표 Transform 을 설정해줍니다.
        bullet.SetTarget(_TargetingSystem.currentTargetTransform);                   
    }

    /// <summary>
    /// 발사 이펙트를 생성하는 메서드입니다.
    /// </summary>
    private void InstantiateEffect()
    {
        GameObject effect = Instantiate(m_Effect_Fire);
        effect.transform.position = _StartPosition.position;
        effect.transform.SetParent(transform);
    }

    /// <summary>
    /// 발사 사운드를 재생하는 메서드입니다.
    /// </summary>
    private void PlayFireSound()
    {
        SoundManager.Instance.PlaySound(_CurrentFireSoundName, SoundType.Effect);
    }

    /// <summary>
    /// 레벨에 따른 공격력을 설정합니다.
    /// </summary>
    /// <param name="level"> 달성 레벨</param>
    private void SetAttackForceByLevel(int level)
    {
        _AttackForce = 10 * (level - 1) + 20.0f;
    }    

    /// <summary>
    /// 발사 시작위치를 찾습니다.
    /// </summary>
    private void FindStartPoint()
    {
        _StartPosition = _OwnerCharacter.modelComponent.currentModel.transform.Find("FirePoint");
    }

    /// <summary>
    /// 주변을 공격하는 메서드입니다.
    /// </summary>
    /// <param name="damageMultiplier"> 현재 공격력에 곱해질 밀치기 공격 데미지 계수 </param>
    public void AttackAround(float damageMultiplier)
    {
        // 공격 범위 감지
        Collider[] hitResult = Physics.OverlapSphere(transform.position + Vector3.down * 0.35f * transform.localScale.y, 0.3f * transform.localScale.x);

        // 공격 대상과의 방향을 계산하여 밀쳐내는 공격을 실행합니다.
        foreach (Collider collider in hitResult)
        {
            if (collider.TryGetComponent<IHit>(out IHit iHit))
            {
                Vector3 direction = collider.transform.position - transform.position;
                direction.y = 1.0f;
                direction.Normalize();

                iHit.OnDamaged(_AttackForce * damageMultiplier, direction);
            }
        }
    }

    /// <summary>
    /// 현재 탄환의 종류를 변경합니다.
    /// </summary>
    /// <param name="newBulletType"> 변경할 탄환 타입</param>
    public void ChangeBullet(BulletType newBulletType)
    {
        // 탄환 종류 교체
        _CurrentBullet = newBulletType;

        // 교체된 탄환 종류에 따른 발사 사운드를 변경합니다.
        switch (newBulletType)
        {
            case BulletType.Basic:                
                _CurrentFireSoundName = SOUNDNAME_FIRE_BASIC;
                break;
            case BulletType.Shell:
                _CurrentFireSoundName = SOUNDNAME_FIRE_SHELL;
                break;
        }

    }

    /// <summary>
    /// 레벨업 시 호출될 메서드입니다.
    /// </summary>
    /// <param name="level"> 달성 레벨</param>
    public void OnLevelUp(int level)
    {
        // 레벨에 따른 공격력 설정
        SetAttackForceByLevel(level);

        // 레벨에 따른 탄창 게이지 최대량을 설정
        SetBulletGaugeMaxValueByLevel(level);

        // 발사 지점 재설정
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

/// <summary>
/// 탄환 게이지 관련 부분
/// </summary>
public partial class PlayerAttack
{
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

    /// <summary>
    /// 탄환 게이지 객체
    /// </summary>
    private BulletGauge _BulletGauge;

    /// <summary>
    /// 탄환 게이지 객체에 대한 읽기 전용 프로퍼티입니다.
    /// </summary>
    public BulletGauge bulletGauge => _BulletGauge;

    /// <summary>
    /// 탄환 게이지를 업데이트하는 메서드입니다.
    /// </summary>
    private void UpdateBulletGauge()
    {
        _BulletGauge.UpdateBulletGauge();
    }

    /// <summary>
    /// 탄환이 남아있는지 체크하는 메서드입니다.
    /// </summary>
    /// <returns> 탄환이 남아있다면 참을 반환합니다.</returns>
    private bool CheckRemainedBullet()
    {
        return !(_BulletGauge.currentValue - m_CostBulletGauge < _BulletGauge.min);
    }

    /// <summary>
    /// 탄환 게이지가 과부하 상태인지 체크하는 메서드입니다.
    /// </summary>
    /// <returns> 과부하 상태라면 참을 반환합니다.</returns>
    private bool CheckOverburden()
    {
        return _BulletGauge.isOverburden;
    }

    /// <summary>
    /// 탄환 게이지를 소모하는 메서드입니다.
    /// </summary>
    /// <param name="cost"> 소모량</param>
    private void CostBulletGauge(int cost)
    {
        _BulletGauge.CostBullet(cost);
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
}

/// <summary>
/// 타겟팅 시스템 관련 부분
/// </summary>
public partial class PlayerAttack
{
    [Header("# 타겟팅 관련")]
    [Header("감지 최대 길이")]
    public float m_SencerDistance = 20.0f;

    [Header("감지 폭")]
    public float m_SencerWidth = 3.0f;

    [Header("감지 높이")]
    public float m_SencerHeight = 3.0f;

    [Header("감지 레이어")]
    public LayerMask m_SenceLayer;

    /// <summary>
    /// 타겟팅 대상을 업데이트하는 메서드입니다.
    /// </summary>
    private void UpdateTargeting()
    {
        _TargetingSystem.Targeting(transform);
    }
}


/// <summary>
/// 아이템 효과 관련 부분
/// </summary>
public partial class PlayerAttack
{
    /// <summary>
    /// 거대화 슬라임의 착지 공격 메서드입니다.
    /// </summary>
    public void GiantSlimeLandAttack()
    {
        AttackAround(3.0f);
    }

    /// <summary>
    /// 거대화 아이템 효과 시작 시의 동작을 나타낸 메서드입니다.
    /// </summary>
    public void OnStartGiant()
    {
        SetBulletGaugeMaxValueByLevel(50);
        FindStartPoint();
        _ProhibitFire = true;
    }

    /// <summary>
    /// 거대화 아이템 효과 종료 시의 동작을 나타낸 메서드입니다.
    /// </summary>
    public void OnFinishGiant()
    {
        OnLevelUp(_OwnerCharacter.levelSystem.level);
        _ProhibitFire = false;
    }

    /// <summary>
    /// 기관총 아이템 효과 시작 시의 동작을 나타낸 메서드입니다.
    /// </summary>
    public void OnStartMachineGun()
    {
        m_PushPowerMultiplier *= 1.5f;

        bulletGauge.currentValue = bulletGauge.max;
        m_CostBulletGauge -= 1;
        _ReuseTimeGuage.max = 0.05f;
    }

    /// <summary>
    /// 기관총 아이템 효과 종료 시의 동작을 나타낸 메서드입니다.
    /// </summary>
    public void OnFinishMachineGun()
    {
        m_PushPowerMultiplier /= 1.5f;

        CostBulletGauge(bulletGauge.currentValue);
        m_CostBulletGauge += 1;
        _ReuseTimeGuage.max = m_AttackReuseTime;
    }

    /// <summary>
    /// 포탄 아이템 효과 시작 시의 동작을 나타낸 메서드입니다.
    /// </summary>
    public void OnStartShell()
    {   
        m_PushPowerMultiplier *= 3.0f;        
        ChangeBullet(BulletType.Shell);

        bulletGauge.currentValue = bulletGauge.max;
        m_CostBulletGauge = (bulletGauge.max - bulletGauge.min) / 4;
        _ReuseTimeGuage.max = 2.0f;
    }

    /// <summary>
    /// 포탄 아이템 효과 업데이트 시의 동작을 나타낸 메서드입니다.
    /// </summary>
    public void OnUpdateShell()
    {
        bulletGauge.SwitchProhibitRecover(true);
    }

    /// <summary>
    /// 포탄 아이템 효과 종료 시의 동작을 나타낸 메서드입니다.
    /// </summary>
    public void OnFinishShell()
    {
        m_PushPowerMultiplier /= 3.0f;
        ChangeBullet(BulletType.Basic);

        CostBulletGauge(bulletGauge.currentValue);
        m_CostBulletGauge = 1;
        bulletGauge.SwitchProhibitRecover(false);

        _ReuseTimeGuage.max = m_AttackReuseTime;
    }
}
