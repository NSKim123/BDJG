using UnityEngine;

// ���� �⺻ ����, ��ġ ������
// ������ �޾Ƽ� �����մϴ�.
[CreateAssetMenu(fileName = "Enemy Spawn Info", menuName = "New Enemy/enemyInfo")]

public class EnemyInfoData : ScriptableObject
{
    // ���Ŀ�
    public EnemyType Type => _enemyType;

    // enemy�� ������ ������
    public string EnemyID => _enemyID;
    public float AttackForce => _attackForce;
    public float DefenseForce => _defenseForce;
    public float MoveSpeed => _moveSpeed;
    public float AttackRange => _attackRange;
    public float AttackSpeed => _attackSpeed;
    public float AttackTime => _attackTime;

    public float SpecialAttackCoolTime => _specialAttackCoolTime;
    public float SpecialAttackRange => _specialAttackRange;
    public float SpecialAttackTime => _specialAttackTime;



    [SerializeField] private EnemyType _enemyType;
    [SerializeField] private string _enemyID;
    [SerializeField] private float _attackForce;
    [SerializeField] private float _defenseForce;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _attackRange;
    [SerializeField] private float _attackSpeed;
    [SerializeField] private float _attackTime;

    [Header("Ư����ü�� ������")]
    [SerializeField] private float _specialAttackCoolTime;
    [SerializeField] private float _specialAttackRange;
    [SerializeField] private float _specialAttackTime;
}
