using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EEnemyType
{
    Mushroom,
    Cactus,
    MushroomSpecial,
    CactusSpecial,
}

// 스폰할 때 필요한 정보
[CreateAssetMenu(fileName = "Enemy Spawn Info", menuName = "New Enemy/spawnInfo")]
public class EnemySpawnInfoData : ScriptableObject
{
    public EEnemyType Type => _enemyType;
    public GameObject EnemyPrefab => _enemyPrefab;
    public int MaxEnemyCount => _maxEnemyCount;
    public float SpawnRadius => _spawnRadius;
    public float SpawnTime => _spawnTime;   

    [SerializeField] private EEnemyType _enemyType;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private int _maxEnemyCount;
    [SerializeField] private float _spawnRadius;
    [SerializeField] private float _spawnTime;

}
