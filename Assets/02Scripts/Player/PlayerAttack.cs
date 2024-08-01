using UnityEditor;
using UnityEngine;

/// <summary>
/// �������� �߻��ϴ� ����ü�� ������ ��Ÿ���� ������ Ÿ���Դϴ�.
/// </summary>
public enum BulletType
{
    Basic,
    Shell,
}

/// <summary>
/// �÷��̾��� ĳ������ ������ �����ϴ� ������Ʈ�Դϴ�. 
/// �߻�, źȯ ������, Ÿ���� �ý��� ���� �κ��� �ۼ��Ǿ��ֽ��ϴ�.
/// </summary>
/// źȯ �߻� ���� �κ�
public partial class PlayerAttack : MonoBehaviour
{
    private static string SOUNDNAME_FIRE_BASIC = "Effect_FIre_ver2";
    private static string SOUNDNAME_FIRE_SHELL = "Effect_FireShell";

    [Header("# źȯ �߻� ����")]
    [Header("��ź ������")]
    public Bullet m_Shell;

    [Header("�߻� ����Ʈ")]
    public GameObject m_Effect_Fire;

    [Header("�߻� �ӷ�")]
    public float m_BulletSpeed = 20.0f;

    [Header("�о�� �� ���")]
    public float m_PushPowerMultiplier = 1.0f;

    [Header("���� ��Ÿ��")]
    public float m_AttackReuseTime = 0.2f;

    [Header("------------------------------------------------------------------------------")]

    /// <summary>
    /// ���ݷ� ( �̴� ��. ������ ���� ���� )
    /// </summary>
    private float _AttackForce;

    /// <summary>
    /// �߻縦 ������ �����ϴ����� ��Ÿ���ϴ�.
    /// </summary>
    private bool _ProhibitFire;

    /// <summary>
    /// ���� �߻� ������ �̸��� ��Ÿ���ϴ�.
    /// </summary>
    private string _CurrentFireSoundName;

    /// <summary>
    /// �߻� ���� ��ġ
    /// </summary>
    private Transform _StartPosition;

    /// <summary>
    /// ��Ÿ�� ������ ��ü
    /// </summary>
    private FloatGauge _ReuseTimeGuage;    

    /// <summary>
    /// Ÿ���� �ý��� ��ü
    /// </summary>
    private TargetingSystem _TargetingSystem;

    /// <summary>
    /// �� ���� ������Ʈ�� ������ �ִ� PlayerCharacter ��ü�Դϴ�.
    /// </summary>
    private PlayerCharacter _OwnerCharacter;

    /// <summary>
    /// ���� ����ϴ� źȯ ����
    /// </summary>
    private BulletType _CurrentBullet;

    /// <summary>
    /// ���� ������ ��Ȳ������ ��Ÿ���� �б����� ������Ƽ�Դϴ�.
    /// </summary>
    public bool isAttacktable => CheckFire();

    /// <summary>
    /// ��Ÿ�� �������� ���� �б� ���� ������Ƽ�Դϴ�.
    /// </summary>
    public FloatGauge reuseTimeGauge => _ReuseTimeGuage;
    

    private void Awake()
    {
        // ��Ÿ�� ������ ��ü�� �����մϴ�.
        _ReuseTimeGuage = new FloatGauge(m_AttackReuseTime);

        // źȯ �������� �����մϴ�.
        _BulletGauge = new BulletGauge(m_BulletGaugeRecoverAmount, m_BulletGaugeRecoverCycle, m_BulletGaugeStartRecoverTime, 10);
                
        // Ÿ���� �ý����� �����մϴ�.
        _TargetingSystem = new TargetingSystem(m_SenceLayer, m_SencerDistance, m_SencerWidth, m_SencerHeight);               
        
        // �� ���� ������Ʈ�� ������ �ִ� PlayerCharacter ��ü�� ã���ϴ�.
        _OwnerCharacter = GetComponent<PlayerCharacter>();

        // ���� źȯ�� �⺻ źȯ���� �����մϴ�.
        ChangeBullet(BulletType.Basic);
    }

    private void Update()
    {
        // ��Ÿ���� �����մϴ�.
        UpdateResueTimeGauge();

        // źȯ �������� �����մϴ�.
        UpdateBulletGauge();
    }

    private void FixedUpdate()
    {
        // Ÿ���� ����� ������Ʈ�մϴ�.
        UpdateTargeting();
    }

    /// <summary>
    /// Attack ������Ʈ�� �����ϴ� �޼����Դϴ�.
    /// </summary>
    public void ResetAttackComponent()
    {
        // ���� ���ð��� �����մϴ�.
        _ReuseTimeGuage.max = m_AttackReuseTime;

        // ȸ�� �Ұ� ���¸� �����մϴ�.
        _BulletGauge.SwitchProhibitRecover(false);

        // ���� źȯ�� �⺻ źȯ���� �����մϴ�.
        ChangeBullet(BulletType.Basic);

        // �߻� ������ �����մϴ�.
        _ProhibitFire = false;
    }

    /// <summary>
    /// ��Ÿ�� �������� �����ϴ� �޼����Դϴ�.
    /// </summary>
    private void UpdateResueTimeGauge()
    {
        // �̹� ��Ÿ���� �� ���Ҵٸ� ȣ���� �����մϴ�.
        if (_ReuseTimeGuage.currentValue == _ReuseTimeGuage.min) return;

        // ��Ÿ���� ���ҽ�ŵ�ϴ�.
        _ReuseTimeGuage.currentValue -= Time.deltaTime;
    }

    /// <summary>
    /// ������ �õ��ϴ� �޼����Դϴ�.
    /// </summary>
    private void TryAttack()
    {
        // ������ �Ұ����� �����̰ų� ��Ÿ���� �����ִٸ� ȣ���� �����մϴ�.
        if (!CheckFire() || _ReuseTimeGuage.currentValue > 0.0f)
            return;

        // �߻��մϴ�.
        Fire();
    }    

    /// <summary>
    /// �˹� �������� üũ�ϴ� �޼����Դϴ�.
    /// </summary>
    /// <returns> �˹� ���¶�� ���� ��ȯ�մϴ�.</returns>
    private bool CheckKnockBack()
    {
        return _OwnerCharacter.movementComponent.isKnockBack;
    }    

    /// <summary>
    /// �ൿ �Ұ� �������� üũ�ϴ� �޼����Դϴ�.
    /// </summary>
    /// <returns> �ൿ �Ұ� ���¶�� ���� ��ȯ�մϴ�.</returns>
    private bool CheckStunned()
    {
        return _OwnerCharacter.isStunned;
    }

    /// <summary>
    /// �߻� ������ �������� Ȯ���ϴ� �޼����Դϴ�.
    /// </summary>
    /// <returns> ���� ������ ���¶�� ���� ��ȯ�մϴ�.</returns>
    private bool CheckFire()
    {
        return CheckRemainedBullet() && !CheckOverburden() && !CheckKnockBack() && !CheckStunned() && !_ProhibitFire;
    }

    /// <summary>
    /// źȯ�� �߻��ϴ� �޼����Դϴ�.
    /// </summary>
    private void Fire()
    {
        // źȯ �������� �Ҹ��մϴ�.
        CostBulletGauge(m_CostBulletGauge);

        // ����ü�� �����մϴ�.
        InstantiateBullet();

        // ���� �ִϸ��̼��� ����մϴ�.
        _OwnerCharacter.animController.TriggerAttackParam();

        // �߻� ����Ʈ ����
        InstantiateEffect();

        // ��Ÿ���� ������ �����մϴ�.
        _ReuseTimeGuage.currentValue = _ReuseTimeGuage.max;

        // �߻� ���带 ����մϴ�.
        PlayFireSound();
    }

    

    /// <summary>
    /// ���� źȯ ������ ���� źȯ�� �����ϴ� �޼����Դϴ�.
    /// </summary>
    private void InstantiateBullet()
    {
        // �߻��� źȯ
        Bullet bullet = null;

        // ���� źȯ�� ������ ���� �߻��� źȯ�� �����մϴ�.
        switch (_CurrentBullet)
        {            
            // �⺻ źȯ
            case BulletType.Basic:
                // ������Ʈ Ǯ���� �⺻ źȯ �ϳ��� �����ɴϴ�.
                GameObject obj = ObjectPoolManager.Instance.GetFromPool(PoolType.Bullet);
                bullet = obj.GetComponent<Bullet>();                
                obj.SetActive(true);
                break;
            // ��ź
            case BulletType.Shell:
                // ��ź �������� ���� �����մϴ�.
                bullet = Instantiate(m_Shell);                
                break;
        }

        // źȯ�� ��ġ, �ӵ�, ���ݷ��� �����մϴ�.
        bullet.transform.position = _StartPosition.position;
        bullet.SetProjectile(this.gameObject, transform.forward, m_BulletSpeed);
        bullet.SetAttackPower(_AttackForce * m_PushPowerMultiplier);

        // źȯ�� ��ǥ Transform �� �������ݴϴ�.
        bullet.SetTarget(_TargetingSystem.currentTargetTransform);                   
    }

    /// <summary>
    /// �߻� ����Ʈ�� �����ϴ� �޼����Դϴ�.
    /// </summary>
    private void InstantiateEffect()
    {
        GameObject effect = Instantiate(m_Effect_Fire);
        effect.transform.position = _StartPosition.position;
        effect.transform.SetParent(transform);
    }

    /// <summary>
    /// �߻� ���带 ����ϴ� �޼����Դϴ�.
    /// </summary>
    private void PlayFireSound()
    {
        SoundManager.Instance.PlaySound(_CurrentFireSoundName, SoundType.Effect);
    }

    /// <summary>
    /// ������ ���� ���ݷ��� �����մϴ�.
    /// </summary>
    /// <param name="level"> �޼� ����</param>
    private void SetAttackForceByLevel(int level)
    {
        _AttackForce = 10 * (level - 1) + 20.0f;
    }    

    /// <summary>
    /// �߻� ������ġ�� ã���ϴ�.
    /// </summary>
    private void FindStartPoint()
    {
        _StartPosition = _OwnerCharacter.modelComponent.currentModel.transform.Find("FirePoint");
    }

    /// <summary>
    /// �ֺ��� �����ϴ� �޼����Դϴ�.
    /// </summary>
    /// <param name="damageMultiplier"> ���� ���ݷ¿� ������ ��ġ�� ���� ������ ��� </param>
    public void AttackAround(float damageMultiplier)
    {
        // ���� ���� ����
        Collider[] hitResult = Physics.OverlapSphere(transform.position + Vector3.down * 0.35f * transform.localScale.y, 0.3f * transform.localScale.x);

        // ���� ������ ������ ����Ͽ� ���ĳ��� ������ �����մϴ�.
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
    /// ���� źȯ�� ������ �����մϴ�.
    /// </summary>
    /// <param name="newBulletType"> ������ źȯ Ÿ��</param>
    public void ChangeBullet(BulletType newBulletType)
    {
        // źȯ ���� ��ü
        _CurrentBullet = newBulletType;

        // ��ü�� źȯ ������ ���� �߻� ���带 �����մϴ�.
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
    /// ������ �� ȣ��� �޼����Դϴ�.
    /// </summary>
    /// <param name="level"> �޼� ����</param>
    public void OnLevelUp(int level)
    {
        // ������ ���� ���ݷ� ����
        SetAttackForceByLevel(level);

        // ������ ���� źâ ������ �ִ뷮�� ����
        SetBulletGaugeMaxValueByLevel(level);

        // �߻� ���� �缳��
        FindStartPoint();
    }

    /// <summary>
    /// ���� �Է��� �޾��� �� ȣ��Ǵ� �޼����Դϴ�.
    /// ����ü �߻縦 �õ��մϴ�.
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
        gUIContent.text = $"\n\n\n\n\n���ݷ� : {_AttackForce}\nźȯ ������ : {_BulletGauge.currentValue} / {_BulletGauge.max}";
        Handles.Label(transform.position + Vector3.down, gUIContent);
    }
#endif
}

/// <summary>
/// źȯ ������ ���� �κ�
/// </summary>
public partial class PlayerAttack
{
    [Header("# źȯ ������ ����")]
    [Header("�߻� �� �Ҹ� ������")]
    public int m_CostBulletGauge = 1;

    [Header("1ȸ ������ ȸ����")]
    public int m_BulletGaugeRecoverAmount = 2;

    [Header("ȸ�� �ֱ�")]
    public float m_BulletGaugeRecoverCycle = 1.0f;

    [Header("���� �� ������ ȸ���� �����ϴ� �ð�")]
    public float m_BulletGaugeStartRecoverTime = 1.0f;

    [Header("------------------------------------------------------------------------------")]

    /// <summary>
    /// źȯ ������ ��ü
    /// </summary>
    private BulletGauge _BulletGauge;

    /// <summary>
    /// źȯ ������ ��ü�� ���� �б� ���� ������Ƽ�Դϴ�.
    /// </summary>
    public BulletGauge bulletGauge => _BulletGauge;

    /// <summary>
    /// źȯ �������� ������Ʈ�ϴ� �޼����Դϴ�.
    /// </summary>
    private void UpdateBulletGauge()
    {
        _BulletGauge.UpdateBulletGauge();
    }

    /// <summary>
    /// źȯ�� �����ִ��� üũ�ϴ� �޼����Դϴ�.
    /// </summary>
    /// <returns> źȯ�� �����ִٸ� ���� ��ȯ�մϴ�.</returns>
    private bool CheckRemainedBullet()
    {
        return !(_BulletGauge.currentValue - m_CostBulletGauge < _BulletGauge.min);
    }

    /// <summary>
    /// źȯ �������� ������ �������� üũ�ϴ� �޼����Դϴ�.
    /// </summary>
    /// <returns> ������ ���¶�� ���� ��ȯ�մϴ�.</returns>
    private bool CheckOverburden()
    {
        return _BulletGauge.isOverburden;
    }

    /// <summary>
    /// źȯ �������� �Ҹ��ϴ� �޼����Դϴ�.
    /// </summary>
    /// <param name="cost"> �Ҹ�</param>
    private void CostBulletGauge(int cost)
    {
        _BulletGauge.CostBullet(cost);
    }

    /// <summary>
    /// ������ ���� źȯ ������ �ִ뷮�� �����ϰ�, �������� 100% ä��ϴ�.
    /// </summary>
    /// <param name="level"> �޼� ����</param>
    private void SetBulletGaugeMaxValueByLevel(int level)
    {
        // �ִ뷮 ��� ��
        _BulletGauge.max = 10 + level * 2;
        _BulletGauge.currentValue = _BulletGauge.max;
    }
}

/// <summary>
/// Ÿ���� �ý��� ���� �κ�
/// </summary>
public partial class PlayerAttack
{
    [Header("# Ÿ���� ����")]
    [Header("���� �ִ� ����")]
    public float m_SencerDistance = 20.0f;

    [Header("���� ��")]
    public float m_SencerWidth = 3.0f;

    [Header("���� ����")]
    public float m_SencerHeight = 3.0f;

    [Header("���� ���̾�")]
    public LayerMask m_SenceLayer;

    /// <summary>
    /// Ÿ���� ����� ������Ʈ�ϴ� �޼����Դϴ�.
    /// </summary>
    private void UpdateTargeting()
    {
        _TargetingSystem.Targeting(transform);
    }
}


/// <summary>
/// ������ ȿ�� ���� �κ�
/// </summary>
public partial class PlayerAttack
{
    /// <summary>
    /// �Ŵ�ȭ �������� ���� ���� �޼����Դϴ�.
    /// </summary>
    public void GiantSlimeLandAttack()
    {
        AttackAround(3.0f);
    }

    /// <summary>
    /// �Ŵ�ȭ ������ ȿ�� ���� ���� ������ ��Ÿ�� �޼����Դϴ�.
    /// </summary>
    public void OnStartGiant()
    {
        SetBulletGaugeMaxValueByLevel(50);
        FindStartPoint();
        _ProhibitFire = true;
    }

    /// <summary>
    /// �Ŵ�ȭ ������ ȿ�� ���� ���� ������ ��Ÿ�� �޼����Դϴ�.
    /// </summary>
    public void OnFinishGiant()
    {
        OnLevelUp(_OwnerCharacter.levelSystem.level);
        _ProhibitFire = false;
    }

    /// <summary>
    /// ����� ������ ȿ�� ���� ���� ������ ��Ÿ�� �޼����Դϴ�.
    /// </summary>
    public void OnStartMachineGun()
    {
        m_PushPowerMultiplier *= 1.5f;

        bulletGauge.currentValue = bulletGauge.max;
        m_CostBulletGauge -= 1;
        _ReuseTimeGuage.max = 0.05f;
    }

    /// <summary>
    /// ����� ������ ȿ�� ���� ���� ������ ��Ÿ�� �޼����Դϴ�.
    /// </summary>
    public void OnFinishMachineGun()
    {
        m_PushPowerMultiplier /= 1.5f;

        CostBulletGauge(bulletGauge.currentValue);
        m_CostBulletGauge += 1;
        _ReuseTimeGuage.max = m_AttackReuseTime;
    }

    /// <summary>
    /// ��ź ������ ȿ�� ���� ���� ������ ��Ÿ�� �޼����Դϴ�.
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
    /// ��ź ������ ȿ�� ������Ʈ ���� ������ ��Ÿ�� �޼����Դϴ�.
    /// </summary>
    public void OnUpdateShell()
    {
        bulletGauge.SwitchProhibitRecover(true);
    }

    /// <summary>
    /// ��ź ������ ȿ�� ���� ���� ������ ��Ÿ�� �޼����Դϴ�.
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
