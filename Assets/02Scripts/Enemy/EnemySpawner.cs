using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WaveName
{
    General,
    Trainee,
    Three,
    Four,
}

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private WaveName currentWave;

    // 슬라임 성장 상태 받아와서 currentWave에 넣기

    private bool _isCactusCorouting;

    public Wave[] waves;

    public event Action<WaveName> OnChangeWave;
    public delegate void CoroutineStarter(WaveName wave);
    public event CoroutineStarter OnCoroutineStart;
    
    [SerializeField] private Transform mushroomSpawnAxis;
    [SerializeField] private Transform cactusSpawnAxis;

    // 임시 변수
    public bool isGameOver = false;


    private void Start()
    {
        currentWave = WaveName.General;
        OnChangeWave += EnemyManager.Instance.ChangeMap;
        OnCoroutineStart += EnemyManager.Instance.StartWaterCoroutine;
        //for (int i = 0; i < waves.Length; i++)
        //{
        //    waves[i].spawn.OrderBy(obj => obj.Type).ToList();
        //    waves[i].enemydata.OrderBy(obj => obj.Type).ToList();
        //}

        StartCoroutine(C_EnemySpawn_Mushroom());
        //StartCoroutine(C_Test());
    }

    private void Update()
    {

        // wave2에서 선인장쿤 생성 코루틴을 호출합니다.
        if (currentWave == WaveName.Trainee && !_isCactusCorouting)
        {
            StartCoroutine(C_EnemySpawn_Cactus());
        }
    }

    private IEnumerator C_Test()
    {
        yield return new WaitForSeconds(5);


        currentWave = WaveName.Trainee;
        //OnChangeWave?.Invoke(currentWave);
        OnCoroutineStart?.Invoke(currentWave);
    }


    // 버섯쿤 생성 코루틴입니다.
    private IEnumerator C_EnemySpawn_Mushroom()
    {
        // 스폰할 객체, 위치, 쿨타임 필요
        // 스폰할 객체에 infoData 넣어주기

        while (!isGameOver)
        {
            int waveIndex = (int)currentWave;

            EnemySpawnInfoData spawndata;
            EnemyInfoData enemydata;

            spawndata = waves[waveIndex].spawn[(int)EnemyType.Mushroom];
            enemydata = waves[waveIndex].enemydata[(int)EnemyType.Mushroom];

            if (EnemyManager.Instance.MushroomCount < spawndata.MaxEnemyCount)
            {
                //Vector3 randomAxis = GetRandomPositionInCircle(20 - spawndata.SpawnRadius);
                
                Vector3 randomPosition = GetRandomPositionOnCircleEdge(mushroomSpawnAxis.position, spawndata.SpawnRadius);

                yield return new WaitForSeconds(spawndata.SpawnTime);

                GameObject newEnemy = Instantiate(spawndata.EnemyPrefab, randomPosition, Quaternion.identity);

                EnemyInit(newEnemy, enemydata);
                EnemyManager.Instance.MushroomCount++;

            }
            yield return null;

        }
        

    }

    // 선인장쿤 생성 코루틴입니다.
    private IEnumerator C_EnemySpawn_Cactus()
    {
        while (!isGameOver)
        {
            _isCactusCorouting = true;

            int waveIndex = (int)currentWave;

            EnemySpawnInfoData spawndata;
            EnemyInfoData enemydata;

            spawndata = waves[waveIndex].spawn[(int)EnemyType.Cactus];
            enemydata = waves[waveIndex].enemydata[(int)EnemyType.Cactus];

            if (EnemyManager.Instance.CactusCount < spawndata.MaxEnemyCount)
            {
                //Vector3 randomAxis = GetRandomPositionInCircle(20 - spawndata.SpawnRadius);
                Vector3 randomPosition = GetRandomPositionOnCircleEdge(cactusSpawnAxis.position, spawndata.SpawnRadius);

                yield return new WaitForSeconds(spawndata.SpawnTime);

                GameObject newEnemy = Instantiate(spawndata.EnemyPrefab, randomPosition, Quaternion.identity);

                EnemyInit(newEnemy, enemydata);
                EnemyManager.Instance.CactusCount++;

            }
            yield return null;


        }
    }

    private void EnemyInit(GameObject enemy, EnemyInfoData data)
    {
        Enemy e = enemy.GetComponent<Enemy>();

        //e.agent.speed = data.MoveSpeed;   // 할당되지않았다고 에러뜸
        e.MoveSpeed = data.MoveSpeed;
        e.AttackForce = data.AttackForce;
        e.AttackTime = data.AttackTime;
        e.DetectPlayerDistance = data.AttackRange;
        
    }



    private Vector3 GetRandomPositionInCircle(float radius)
    {
        float angle = UnityEngine.Random.Range(0, 360) * Mathf.Deg2Rad; // 각도를 라디안으로 변환
        float r = Mathf.Sqrt(UnityEngine.Random.value) * radius; // 거리를 보정
        float x = r * Mathf.Cos(angle);
        float z = r * Mathf.Sin(angle);
        return new Vector3(x, 1.0f, z); // 변환된 좌표를 반환
    }

    Vector3 GetRandomPositionOnCircleEdge(Vector3 center, float radius)
    {
        float angle = UnityEngine.Random.Range(0, 360) * Mathf.Deg2Rad; // 각도를 라디안으로 변환
        float x = center.x + radius * Mathf.Cos(angle);
        float z = center.z + radius * Mathf.Sin(angle);
        return new Vector3(x, center.y, z); // 중심의 y값을 그대로 유지
    }


    // 각 wave마다 스폰 정보와 enemy 정보를 저장해둡니다.
    [Serializable]
    public class Wave
    {
        public List<EnemySpawnInfoData> spawn;
        public List<EnemyInfoData> enemydata;
    }
}
