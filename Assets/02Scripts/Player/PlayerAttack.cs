using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// �÷��̾��� ĳ������ ������ �����ϴ� ������Ʈ�Դϴ�.
/// </summary>
public class PlayerAttack : MonoBehaviour
{
    [Header("# źȯ �߻� ����")]
    [Header("źȯ ������")]
    public Bullet m_Bullet;

    [Header("�߻� ����Ʈ")]
    public GameObject m_Effect_Fire;

    [Header("�߻� �ӷ�")]
    public float m_BulletSpeed = 20.0f;

    [Header("�о�� �� ���")]
    public float m_PushPowerMultiplier = 1.0f;

    [Header("���� ��Ÿ��")]
    public float m_AttackReuseTime = 0.75f;

    [Header("------------------------------------------------------------------------------")]

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
    /// ���ݷ� ( �̴� ��. ������ ���� ���� )
    /// </summary>
    private float _AttackForce;

    private bool _ProhibitFire;

    /// <summary>
    /// �߻� ���� ��ġ
    /// </summary>
    private Transform _StartPosition;

    /// <summary>
    /// ��Ÿ�� ������ ��ü
    /// </summary>
    private FloatGauge _ReuseTimeGuage;

    /// <summary>
    /// źȯ ������ ��ü
    /// </summary>
    private BulletGauge _BulletGauge;

    /// <summary>
    /// Ÿ���� �ý��� ��ü
    /// </summary>
    private TargetingSystem _TargetingSystem;

    /// <summary>
    /// �� ���� ������Ʈ�� ������ �ִ� PlayerCharacter ��ü�Դϴ�.
    /// </summary>
    private PlayerCharacter _OwnerCharacter;

    /// <summary>
    /// ���� ������ ��Ȳ������ ��Ÿ���� �б����� ������Ƽ�Դϴ�.
    /// </summary>
    public bool isAttacktable => CheckFire();

    /// <summary>
    /// ��Ÿ�� �������� ���� �б� ���� ������Ƽ�Դϴ�.
    /// </summary>
    public FloatGauge reuseTimeGauge => _ReuseTimeGuage;

    /// <summary>
    /// źȯ ������ ��ü�� ���� �б� ���� ������Ƽ�Դϴ�.
    /// </summary>
    public BulletGauge bulletGauge => _BulletGauge;

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
    }

    private void Update()
    {
        // ��Ÿ���� �����մϴ�.
        UpdateResueTimeGauge();

        // źȯ �������� �����մϴ�.
        _BulletGauge.UpdateBulletGauge();
    }

    private void FixedUpdate()
    {
        // Ÿ���� ����� �����մϴ�.
        _TargetingSystem.Targeting(transform);        
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
    /// ������ �õ��մϴ�.
    /// </summary>
    private void TryAttack()
    {
        // ������ �Ұ����� ���¶�� ȣ���� �����մϴ�.
        if (!CheckFire())
            return;

        // ��Ÿ���� �����ִٸ� ȣ���� �����մϴ�.
        if (_ReuseTimeGuage.currentValue > 0.0f) 
            return;

        // �߻��մϴ�.
        Fire();
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
    /// �߻� ������ �������� Ȯ���մϴ�.
    /// </summary>
    /// <returns> ���� ������ ���¶�� ���� ��ȯ�մϴ�.</returns>
    private bool CheckFire()
    {
        return CheckRemainedBullet() && !CheckOverburden() && !CheckKnockBack() && !CheckStunned() && !_ProhibitFire;
    }

    /// <summary>
    /// �߻��ϴ� �޼����Դϴ�.
    /// </summary>
    private void Fire()
    {
        // źȯ �������� �Ҹ��մϴ�.
        CostStamina(m_CostBulletGauge);

        // ����ü�� �����մϴ�.
        InstantiateBullet();

        // ���� �ִϸ��̼��� ����մϴ�.
        _OwnerCharacter.animController.TriggerAttackParam();

        // �߻� ����Ʈ ����
        GameObject effect = Instantiate(m_Effect_Fire);
        effect.transform.position = _StartPosition.position;
        effect.transform.SetParent(transform);

        // ��Ÿ���� ������ �����մϴ�.
        //_ReuseTimeGuage.currentValue = _ReuseTimeGuage.max;
    }

    /// <summary>
    /// źȯ �������� �Ҹ��մϴ�.
    /// </summary>
    /// <param name="cost"> �Ҹ�</param>
    private void CostStamina(int cost)
    {
        _BulletGauge.CostBullet(cost);
    }

    /// <summary>
    /// źȯ�� �߻��մϴ�.
    /// </summary>
    private void InstantiateBullet()
    {
        Bullet bullet = Instantiate(m_Bullet);
        bullet.transform.position = _StartPosition.position;
        bullet.SetProjectile(this.gameObject, transform.forward, m_BulletSpeed);
        bullet.SetAttackPower(_AttackForce * m_PushPowerMultiplier);

        // Ÿ���� ���� ��ü�� �ִٸ� źȯ�� ��ǥ Transform �� �������ݴϴ�.
        if(_TargetingSystem.currentTargetTransform != null)
            bullet.SetTarget(_TargetingSystem.currentTargetTransform);
    }

    /// <summary>
    /// ������ ���� ���ݷ��� �����մϴ�.
    /// </summary>
    /// <param name="level"> �޼� ����</param>
    private void SetAttackForceByLevel(int level)
    {        
        _AttackForce = level * 10;
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

    /// <summary>
    /// �߻� ������ġ�� ã���ϴ�.
    /// </summary>
    private void FindStartPoint()
    {
        _StartPosition = _OwnerCharacter.modelComponent.currentModel.transform.Find("FirePoint");
    }

    public void OnGiantStart()
    {
        SetBulletGaugeMaxValueByLevel(50);
        FindStartPoint();
        _ProhibitFire = true;
    }

    public void OnGiantFinish()
    {
        _ProhibitFire = false;
    }

    public void AttackAround()
    {
        Collider[] hitResult = Physics.OverlapSphere(transform.position + Vector3.down * 0.35f * transform.localScale.y, 0.3f * transform.localScale.x);

        foreach (Collider collider in hitResult)
        {
            if (collider.TryGetComponent<IHit>(out IHit iHit))
            {
                iHit.OnDamaged(_AttackForce, (collider.transform.position - transform.position).normalized);
            }
        }
    }

    /// <summary>
    /// ������ �� ȣ��� �޼����Դϴ�.
    /// </summary>
    /// <param name="level"> �޼� ����</param>
    public void OnLevelUp(int level)
    {
        SetAttackForceByLevel(level);
        SetBulletGaugeMaxValueByLevel(level);
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
