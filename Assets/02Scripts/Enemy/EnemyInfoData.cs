using UnityEngine;

// 적의 기본 정보, 수치 데이터
// 파일을 받아서 저장합니다.
[CreateAssetMenu(fileName = "Enemy Spawn Info", menuName = "New Enemy/enemyInfo")]

public class EnemyInfoData : ScriptableObject
{
    // 정렬용
    public EnemyType Type => _enemyType;

    // enemy에 적용할 데이터
    public string EnemyID => _enemyID;
    public float AttackForce => _attackForce;
    public float DefenseForce => _defenseForce;
    public float MoveSpeed => _moveSpeed;
    public bool IsAttackTargetExist => _isAttackTargetExist;
    public bool AttackWay => _attackWay;
    public float AttackRange => _attackRange;
    public float AttackSpeed => _attackSpeed;
    public float AttackTime => _attackTime;

    [SerializeField] private EnemyType _enemyType;
    [SerializeField] private string _enemyID;
    [SerializeField] private float _attackForce;
    [SerializeField] private float _defenseForce;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private bool _isAttackTargetExist;
    [SerializeField] private bool _attackWay;
    [SerializeField] private float _attackRange;
    [SerializeField] private float _attackSpeed;
    [SerializeField] private float _attackTime;
}
