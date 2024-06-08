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

    [Header("# ���׹̳� ����")]
    [Header("�߻� �� �Ҹ� ���׹̳�")]
    public float m_CostStamina = 1.0f;

    [Header("���׹̳� �ڿ� ȸ�� �ӵ� /s")]
    public float m_StaminaRecoverSpeed = 0.5f;

    /// <summary>
    /// ���ݷ� ( �̴� ��. ������ ���� ���� )
    /// </summary>
    private float _AttackForce;
    public float attackForce => _AttackForce;

    /// <summary>
    /// ���¹̳� ������ ��ü
    /// </summary>
    private FloatGauge _StaminaGauge;

    /// <summary>
    /// �� ���� ������Ʈ�� ������ �ִ� PlayerCharacter ��ü�Դϴ�.
    /// </summary>
    private PlayerCharacter _OwnerCharacter;

    public FloatGauge staminaGauge => _StaminaGauge;

    private void Awake()
    {
        // ���¹̳� �������� �ʱ�ȭ�մϴ�.
        _StaminaGauge = new FloatGauge(10.0f, 10.0f);

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
    /// ������ ���� ���¹̳� �ִ뷮�� �����ϰ�, �������� 100% ä��ϴ�.
    /// </summary>
    /// <param name="level"> �޼� ����</param>
    private void SetStaminaMaxValueByLevel(int level)
    {
        _StaminaGauge.max = 10.0f + level * 2.0f;
        _StaminaGauge.currentValue = _StaminaGauge.max;
    }

    /// <summary>
    /// ������ �� ȣ��� �޼����Դϴ�.
    /// </summary>
    /// <param name="level"> �޼� ����</param>
    public void OnlevelUp(int level)
    {
        SetAttackForceByLevel(level);
        SetStaminaMaxValueByLevel(level);

        // �߻� ������ġ�� ã���ϴ�.
        m_StartPosition = _OwnerCharacter.modelComponent.currentModel.transform.Find("FirePoint");
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
        if (_StaminaGauge == null) return;

        GUIContent gUIContent = new GUIContent();
        gUIContent.text = $"\n\n\n\n���ݷ� : {_AttackForce}\n���׹̳� : {_StaminaGauge.currentValue} / {_StaminaGauge.max}";
        Handles.Label(transform.position + Vector3.down, gUIContent);
    }
#endif
}
