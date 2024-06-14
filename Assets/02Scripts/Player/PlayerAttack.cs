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
    public float m_SencerWidth = 2.0f;

    [Header("���� ����")]
    public float m_SencerHeight = 2.0f;

    [Header("���� ���̾�")]
    public LayerMask m_SenceLayer;

    /// <summary>
    /// ���ݷ� ( �̴� ��. ������ ���� ���� )
    /// </summary>
    private float _AttackForce;

    /// <summary>
    /// �߻� ���� ��ġ
    /// </summary>
    private Transform _StartPosition;

    /// <summary>
    /// ��Ÿ�� 
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

    public bool isAttacktable => CheckAttackable();

    public FloatGauge reuseTimeGauge => _ReuseTimeGuage;

    /// <summary>
    /// źȯ ������ ��ü�� ���� �б� ���� ������Ƽ�Դϴ�.
    /// </summary>
    public BulletGauge bulletGauge => _BulletGauge;

    private void Awake()
    {
        _ReuseTimeGuage = new FloatGauge(m_AttackReuseTime);

        // źȯ �������� �����մϴ�.
        _BulletGauge = new BulletGauge(m_BulletGaugeRecoverAmount, m_BulletGaugeRecoverCycle, m_BulletGaugeStartRecoverTime, 10);
                
        // Ÿ���� �ý����� �����մϴ�.
        _TargetingSystem = new TargetingSystem(LayerMask.GetMask("Enemy"), m_SencerDistance, m_SencerWidth, m_SencerHeight);               
        
        // �� ���� ������Ʈ�� ������ �ִ� PlayerCharacter ��ü�� ã���ϴ�.
        _OwnerCharacter = GetComponent<PlayerCharacter>();        
    }

    private void Update()
    {
        UpdateResueTimeGauge();

        // źȯ �������� ������Ʈ�մϴ�.
        _BulletGauge.UpdateBulletGauge();
    }

    private void FixedUpdate()
    {
        // Ÿ���� ����� �����մϴ�.
        _TargetingSystem.Targeting(transform);        
    }

    private void UpdateResueTimeGauge()
    {
        if (_ReuseTimeGuage.currentValue == _ReuseTimeGuage.min) return;

        _ReuseTimeGuage.currentValue -= Time.deltaTime;
    }

    /// <summary>
    /// ������ �õ��մϴ�.
    /// </summary>
    private void TryAttack()
    {
        // ������ �Ұ����� ���¶�� ȣ���� �ߴ��մϴ�.
        if (!CheckAttackable())
            return;

        if (_ReuseTimeGuage.currentValue > 0.0f) 
            return;

        // �߻��մϴ�.
        Fire();
    }

    /// <summary>
    /// źȯ�� �����ִ��� üũ�մϴ�.
    /// </summary>
    /// <returns> źȯ�� �����ִٸ� ���� ��ȯ�մϴ�.</returns>
    private bool CheckRemainedBullet()
    {
        return !(_BulletGauge.currentValue - m_CostBulletGauge < _BulletGauge.min);
    }

    /// <summary>
    /// źȯ �������� ������ �������� üũ�մϴ�.
    /// </summary>
    /// <returns> ������ ���¶�� ���� ��ȯ�մϴ�.</returns>
    private bool CheckOverburden()
    {
        return _BulletGauge.isOverburden;
    }

    /// <summary>
    /// �˹� �������� Ȯ���մϴ�.
    /// </summary>
    /// <returns> �˹� ���¶�� ���� ��ȯ�մϴ�.</returns>
    private bool CheckKnockBack()
    {
        return _OwnerCharacter.movementComponent.isKnockBack;
    }    

    private bool CheckStunned()
    {
        return _OwnerCharacter.isStunned;
    }

    /// <summary>
    /// ���� ������ �������� Ȯ���մϴ�.
    /// </summary>
    /// <returns> ���� ������ ���¶�� ���� ��ȯ�մϴ�.</returns>
    private bool CheckAttackable()
    {
        return CheckRemainedBullet() && !CheckOverburden() && !CheckKnockBack() && !CheckStunned();
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

        _ReuseTimeGuage.currentValue = _ReuseTimeGuage.max;
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

        if(_TargetingSystem.currentTargetTransform != null)
            bullet.SetTarget(_TargetingSystem.currentTargetTransform);
    }

    /// <summary>
    /// ������ ���� ���ݷ��� �����մϴ�.
    /// </summary>
    /// <param name="level"> �޼� ����</param>
    private void SetAttackForceByLevel(int level)
    {        
        _AttackForce = level;
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
