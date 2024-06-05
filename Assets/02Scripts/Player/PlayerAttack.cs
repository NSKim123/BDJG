using System.Collections;
using System.Collections.Generic;
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

    [Header("�о�� ��")]
    public float m_PushPower = 5.0f;

    [Header("�߻� ���� ��ġ")]
    public Transform m_StartPosition;

    [Header("---------------------------------------")]

    [Header("# ���׹̳� ����")]
    [Header("�ִ� ���׹̳�")]
    public float m_MaxStamina = 100.0f;

    [Header("�߻� �� �Ҹ� ���׹̳�")]
    public float m_CostStamina = 10.0f;

    [Header("���׹̳� �ڿ� ȸ�� �ӵ� /s")]
    public float m_StaminaRecoverSpeed = 5.0f;

    /// <summary>
    /// ���¹̳� ������ ��ü
    /// </summary>
    private Gauge _StaminaGauge;

    /// <summary>
    /// �� ���� ������Ʈ�� ������ �ִ� PlayerCharacter ��ü�Դϴ�.
    /// </summary>
    private PlayerCharacter _OwnerCharacter;

    private void Awake()
    {
        // ���¹̳� �������� �ʱ�ȭ�մϴ�.
        _StaminaGauge = new Gauge(m_MaxStamina, m_MaxStamina);

        // �� ���� ������Ʈ�� ������ �ִ� PlayerCharacter ��ü�� ã���ϴ�.
        _OwnerCharacter = GetComponent<PlayerCharacter>();        
    }

    private void Update()
    {
        // ���¹̳��� �ڿ� ȸ���մϴ�.
        RecoverStamina(m_StaminaRecoverSpeed * Time.deltaTime);
    }

    /// <summary>
    /// ���¹̳��� ȸ���մϴ�.
    /// </summary>
    /// <param name="recover"> ȸ�����Դϴ�.</param>
    private void RecoverStamina(float recover)
    {
        _StaminaGauge.currentValue += recover;
    }

    /// <summary>
    /// ������ �õ��մϴ�.
    /// </summary>
    private void TryAttack()
    {
        // ���¹̳��� �����ϸ� �߻����� �ʰ� �����մϴ�.
        if (_StaminaGauge.currentValue - m_CostStamina < _StaminaGauge.min)
        {
            Debug.Log("���׹̳��� ������ �߻��� �� �����ϴ�!");
            return;
        }

        // �߻��մϴ�.
        Fire();
    }

    /// <summary>
    /// �߻��ϴ� �޼����Դϴ�.
    /// </summary>
    private void Fire()
    {
        // ���¹̳��� �Ҹ��մϴ�.
        CostStamina(m_CostStamina);

        // ����ü�� �����մϴ�.
        InstantiateBullet();
    }

    /// <summary>
    /// ���¹̳��� �Ҹ��մϴ�.
    /// </summary>
    /// <param name="cost"> �Ҹ��Դϴ�.</param>
    private void CostStamina(float cost)
    {
        _StaminaGauge.currentValue -= cost;
    }

    /// <summary>
    /// ����ü�� �߻��մϴ�.
    /// </summary>
    private void InstantiateBullet()
    {
        Bullet bullet = Instantiate(m_Bullet);
        bullet.transform.position = m_StartPosition.position;
        bullet.SetProjectile(this.gameObject, transform.forward, m_BulletSpeed);
    }

    /// <summary>
    /// ���� �Է��� �޾��� �� ȣ��Ǵ� �޼����Դϴ�.
    /// ����ü �߻縦 �õ��մϴ�.
    /// </summary>
    public void OnAttackInput()
    {
        TryAttack();        
    }
}
