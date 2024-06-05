using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Mushroom,
}

// ������ �� �ʿ��� ����
// ���� ��ġ(�÷��̾���� �Ÿ�), ���� �ð�(�� �� ��), �� Ÿ�� �Ǵ� �̸�(��������?) ...  

[CreateAssetMenu(fileName = "Enemy Spawn Info", menuName = "New Enemy/spawnInfo")]
public class EnemySpawnInfoScriptableObject : ScriptableObject
{
    
    public float SpawnDistance => _spawnDistance;
    public float SpawnTime => _spawnTime;

    [SerializeField] private float _spawnDistance;
    [SerializeField] private float _spawnTime;

}
