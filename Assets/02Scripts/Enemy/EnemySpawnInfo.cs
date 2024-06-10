using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// SpawnScriptableObject 값을 적용, 객체에 붙음 - 삭제할 수도 있습니다.
public class EnemySpawnInfo : MonoBehaviour
{
    public EnemySpawnInfoData SpawnInfo => _spawnInfo;

    [SerializeField] private EnemySpawnInfoData _spawnInfo;

}
