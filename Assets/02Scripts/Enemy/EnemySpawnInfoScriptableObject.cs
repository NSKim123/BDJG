using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Mushroom,
}

// 스폰할 때 필요한 정보
// 스폰 위치(플레이어와의 거리), 스폰 시간(몇 초 뒤), 적 타입 또는 이름(빠질수도?) ...  

[CreateAssetMenu(fileName = "Enemy Spawn Info", menuName = "New Enemy/spawnInfo")]
public class EnemySpawnInfoScriptableObject : ScriptableObject
{
    
    public float SpawnDistance => _spawnDistance;
    public float SpawnTime => _spawnTime;

    [SerializeField] private float _spawnDistance;
    [SerializeField] private float _spawnTime;

}
