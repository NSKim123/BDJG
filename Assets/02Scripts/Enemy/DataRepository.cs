using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataRepository : MonoBehaviour
{
    public static DataRepository Instance
    {
        get
        {
            return _instance;
        }
    }
    private static DataRepository _instance;

    [SerializeField] private EnemySpawnInfoData[] _spawnInfoData;
    [SerializeField] private EnemyInfoData[] _enemyData;

    public EnemySpawnInfoData[] SpawnInfoData => _spawnInfoData;
    public EnemyInfoData[] EnemyData => _enemyData;

    public Dictionary<EnemyType, EnemySpawnInfoData> enemySpawnDatum;
    public Dictionary<EnemyType, EnemyInfoData> enemyInfoDatum;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        
    }
}
