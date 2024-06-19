using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private WaveName currentWave;


    public Wave[] waves;

    public delegate void CoroutineStarter(WaveName wave);
    public event CoroutineStarter OnCoroutineStart;
    
    [SerializeField] private Transform mushroomSpawnAxis;
    [SerializeField] private Transform cactusSpawnAxis;

    private Coroutine mushroomCoroutine;
    private Coroutine cactusCoroutine;

    public System.Action<int> onEnemyDead;

    private bool isPaused = false;

    // **** 코드 정리 필요

    private void Start()
    {
        currentWave = WaveName.General;

        //StartMushroomSpawn(currentWave);
        //StartCoroutine(C_Test());

        /*for (int i = 0; i < waves.Length; i++)
        //{
        //    waves[i].spawn.OrderBy(obj => obj.Type).ToList();
        //    waves[i].enemydata.OrderBy(obj => obj.Type).ToList();
        }*/
    }

    private void Update()
    {
        // wave2에서 선인장쿤 생성 코루틴을 호출합니다.
        //if (currentWave == WaveName.Trainee && cactusCoroutine == null)
        //{
        //    cactusCoroutine = StartCoroutine(C_EnemySpawn_Cactus(currentWave));
        //}
    }

    // 테스트
    private IEnumerator C_Test()
    {
        yield return new WaitForSeconds(5);

        currentWave = WaveName.Trainee;
        //OnChangeWave?.Invoke(currentWave);
        OnCoroutineStart?.Invoke(currentWave);
    }


    // 버섯쿤 생성 코루틴입니다.
    private IEnumerator C_EnemySpawn_Mushroom(WaveName wave)
    {
        while (true)
        {
            // 일시정지일 때 대기
            while (isPaused)
            {
                yield return null;
            }

            int waveIndex = (int)wave-1;

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
    private IEnumerator C_EnemySpawn_Cactus(WaveName wave)
    {
        while (true)
        {
            while (isPaused)
            {
                yield return null;
            }

            int waveIndex = (int)wave-1;

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
    
    // 적 생성 후 데이터를 초기화하는 메서드입니다.
    private void EnemyInit(GameObject enemy, EnemyInfoData data)
    {
        Enemy e = enemy.GetComponent<Enemy>();

        //e.agent.speed = data.MoveSpeed;
        e.MoveSpeed = data.MoveSpeed;
        e.AttackForce = data.AttackForce;
        e.AttackTime = data.AttackTime;
        e.DetectPlayerDistance = data.AttackRange;
        e.onDead += () => onEnemyDead?.Invoke(1);
    }

    // 일시정지 먼저 -> 레벨업 메서드 호출

    // 적 스폰 일시정지 껐다켰다
    public void PauseSwitchEnemySpawn()
    {
        isPaused = !isPaused;
    }

    // 레벨업 시 호출할 메서드
    public void ResetForLevelUp(int level)
    {
        // 일시정지하기
        PauseSwitchEnemySpawn();
        SpawnByWave((WaveName)level);
    }

    /// <summary>
    /// 버섯쿤 스폰 시작 메서드입니다.
    /// </summary>
    /// <param name="wave">현재 레벨, WaveName으로 형변환</param>
    private void StartMushroomSpawn(WaveName wave)
    {
        if (mushroomCoroutine == null)
        {
            mushroomCoroutine = StartCoroutine(C_EnemySpawn_Mushroom(wave));
        }
    }

    /// <summary>
    /// 선인장쿤 스폰 시작 메서드입니다.
    /// </summary>
    /// <param name="wave">현재 레벨, WaveName으로 형변환</param>
    private void StartCactusSpawn(WaveName wave)
    {
        if (cactusCoroutine == null)
        {
            cactusCoroutine = StartCoroutine(C_EnemySpawn_Cactus(wave));
        }
    }

    // 웨이브별로 적 스폰
    private void SpawnByWave(WaveName wave)
    {
        WaveName current = wave;

        // 일시정지를 재개로 변경
        PauseSwitchEnemySpawn();

        switch (current)
        {
            case WaveName.General:
                break;
            case WaveName.Trainee:
                {
                    StartCactusSpawn(wave);
                }
                break;
            case WaveName.Three:
                break;
            case WaveName.Four:
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 생성 코루틴 중단 후 맵에 있는 적을 모두 파괴하고, 버섯쿤(1레벨) 스폰을 시작합니다.
    /// </summary>
    public void ResetForRestart()
    {
        // 생성 코루틴 전부 중단
        StopAllCoroutines();

        // 맵 내에 있는 적들 모두 파괴
        GameObject[] removeList = isEnemyExistInMap();
        if (removeList != null)
        {
            foreach (var item in removeList)
            {
                Destroy(item);
            }
        }

        // 버섯쿤 스폰 시작
        currentWave = WaveName.General;  // 삭제할 수도
        StartMushroomSpawn(currentWave);
    }


    // 맵 내에 적들이 있다면 배열로 받아오는 메서드입니다.
    private GameObject[] isEnemyExistInMap()
    {
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemys.Length > 0)
        {
            return enemys;
        }
        else
        {
            return null;
        }
    }


    // 반지름을 받아서 원형 내부에서 랜덤 위치를 계산하는 메서드들입니다.
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
