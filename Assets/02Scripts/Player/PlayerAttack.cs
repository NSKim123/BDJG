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

    [Header("�߻� ���� ��ġ")]
    public Transform m_StartPosition;

    [Header("---------------------------------------")]

    [Header("# źȯ ������ ����")]
    [Header("�߻� �� �Ҹ� ������")]
    public int m_CostBulletGauge = 1;

    /// <summary>
    /// źȯ ������ ��ü
    /// </summary>
    [SerializeField]
    private BulletGauge _BulletGauge;

    /// <summary>
    /// ���ݷ� ( �̴� ��. ������ ���� ���� )
    /// </summary>
    private float _AttackForce;

    /// <summary>
    /// �� ���� ������Ʈ�� ������ �ִ� PlayerCharacter ��ü�Դϴ�.
    /// </summary>
    private PlayerCharacter _OwnerCharacter;

    /// <summary>
    /// źȯ ������ ��ü�� ���� �б� ���� ������Ƽ�Դϴ�.
    /// </summary>
    public BulletGauge bulletGauge => _BulletGauge;

    private void Awake()
    {
        // ���¹̳� �������� �ʱ�ȭ�մϴ�.
        _BulletGauge = new BulletGauge(10, 10);

        // �� ���� ������Ʈ�� ������ �ִ� PlayerCharacter ��ü�� ã���ϴ�.
        _OwnerCharacter = GetComponent<PlayerCharacter>();        
    }

    private void Update()
    {
        // źȯ �������� ������Ʈ�մϴ�.
        _BulletGauge.UpdateBulletGauge();
    }   

    /// <summary>
    /// ������ �õ��մϴ�.
    /// </summary>
    private void TryAttack()
    {
        // ������ �Ұ����� ���¶�� ȣ���� �ߴ��մϴ�.
        if (!CheckAttackable())
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
    /// źȯ �������� ����ȭ �������� üũ�մϴ�.
    /// </summary>
    /// <returns> ����ȭ ���¶�� ���� ��ȯ�մϴ�.</returns>
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

    /// <summary>
    /// ���� ������ �������� Ȯ���մϴ�.
    /// </summary>
    /// <returns> ���� ������ ���¶�� ���� ��ȯ�մϴ�.</returns>
    private bool CheckAttackable()
    {
        return CheckRemainedBullet() && !CheckOverburden() && !CheckKnockBack();
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
    /// ����ü�� �߻��մϴ�.
    /// </summary>
    private void InstantiateBullet()
    {
        Bullet bullet = Instantiate(m_Bullet);
        bullet.transform.position = m_StartPosition.position;
        bullet.SetProjectile(this.gameObject, transform.forward, m_BulletSpeed);
        bullet.SetAttackPower(_AttackForce * m_PushPowerMultiplier);
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
        m_StartPosition = _OwnerCharacter.modelComponent.currentModel.transform.Find("FirePoint");
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
