using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// SpawnScriptableObject 값을 적용, 객체에 붙음
public class EnemySpawnInfo : MonoBehaviour
{
    public EnemySpawnInfoScriptableObject SpawnInfo => _spawnInfo;

    [SerializeField] private EnemySpawnInfoScriptableObject _spawnInfo;

}
