using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Mushroom,
    Cactus,
    MushroomSpecial,
    CactusSpecial,
}

// ������ �� �ʿ��� ����
// ���� ��ġ, ���� �ð�(�� �� ��), ���̺� ����, ...

[CreateAssetMenu(fileName = "Enemy Spawn Info", menuName = "New Enemy/spawnInfo")]
public class EnemySpawnInfoData : ScriptableObject
{
    public EnemyType Type => _enemyType;
    public GameObject EnemyPrefab => _enemyPrefab;
    public int MaxEnemyCount => _maxEnemyCount;
    public float SpawnRadius => _spawnRadius;
    public float SpawnTime => _spawnTime;   

    [SerializeField] private EnemyType _enemyType;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private int _maxEnemyCount;
    [SerializeField] private float _spawnRadius;
    [SerializeField] private float _spawnTime;

}
