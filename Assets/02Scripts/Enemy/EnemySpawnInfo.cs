using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// SpawnScriptableObject ���� ����, ��ü�� ���� - ������ ���� �ֽ��ϴ�.
public class EnemySpawnInfo : MonoBehaviour
{
    public EnemySpawnInfoData SpawnInfo => _spawnInfo;

    [SerializeField] private EnemySpawnInfoData _spawnInfo;

}
