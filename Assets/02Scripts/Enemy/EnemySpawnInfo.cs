using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// SpawnScriptableObject ���� ����, ��ü�� ����
public class EnemySpawnInfo : MonoBehaviour
{
    public EnemySpawnInfoScriptableObject SpawnInfo => _spawnInfo;

    [SerializeField] private EnemySpawnInfoScriptableObject _spawnInfo;

}
