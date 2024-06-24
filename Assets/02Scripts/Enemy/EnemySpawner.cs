using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemySpawnTurn
{
    mushroomTurn = 1,
    cactusTurn,
}

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

    [Header("버섯쿤 데이터")]
    public EnemyInfoData[] mushroomData;
    [Header("버섯쿤 스폰 정보")]
    public EnemySpawnInfoData[] mushroomSpawnInfo;

    [Header("선인장쿤 데이터")]
    public EnemyInfoData[] cactusData;
    [Header("선인장쿤 스폰 정보")]
    public EnemySpawnInfoData[] cactusSpawnInfo;

    private EnemySpawnTurn _turnPivot;

    private int _mushroomSpawnedCount;
    private int _cactusSpawnedCount;
    private int _totalCount;

    private Coroutine spawnLoopCoroutine;


    // 버섯쿤 생성 코루틴입니다.
    private IEnumerator C_EnemySpawn_Mushroom()
    {
        while (true)
        {
            //int waveIndex = (int)wave - 1;

            EnemyInfoData enemydata;
            EnemySpawnInfoData spawndata;

            enemydata = mushroomData[0];
            spawndata = mushroomSpawnInfo[0];

            Vector3 randomPosition = GetRandomPositionOnCircleEdge(mushroomSpawnAxis.position, spawndata.SpawnRadius);

            yield return new WaitForSeconds(spawndata.SpawnTime);

            GameObject newEnemy = Instantiate(spawndata.EnemyPrefab, randomPosition, Quaternion.identity);

            EnemyInit(newEnemy, enemydata);
            _mushroomSpawnedCount++;
            EnemyManager.Instance.TotalCount++;

            yield return null;
        }
    }

    // 선인장쿤 생성 코루틴입니다.
    private IEnumerator C_EnemySpawn_Cactus()
    {
        while (true)
        {
            //while (isPaused)
            //{
            //    yield return null;
            //}

            EnemySpawnInfoData spawndata;
            EnemyInfoData enemydata;

            enemydata = cactusData[0];
            spawndata = cactusSpawnInfo[0];

            Vector3 randomPosition = GetRandomPositionOnCircleEdge(cactusSpawnAxis.position, spawndata.SpawnRadius);

            yield return new WaitForSeconds(spawndata.SpawnTime);

            GameObject newEnemy = Instantiate(spawndata.EnemyPrefab, randomPosition, Quaternion.identity);

            EnemyInit(newEnemy, enemydata);
            _cactusSpawnedCount++;
            EnemyManager.Instance.TotalCount++;

            yield return null;
        }
    }

    /// <summary>
    /// 적 생성 후 데이터를 초기화하는 메서드입니다.
    /// </summary>
    /// <param name="enemy">생성된 적 객체</param>
    /// <param name="data">적에게 넣을 데이터 scriptable object</param>
    private void EnemyInit(GameObject enemy, EnemyInfoData data)
    {
        Enemy e = enemy.GetComponent<Enemy>();

        //e.agent.speed = data.MoveSpeed;
        e.MoveSpeed = data.MoveSpeed;
        e.AttackForce = data.AttackForce;
        e.AttackTime = data.AttackTime;
        e.AttackSpeed = data.AttackSpeed;
        e.DetectPlayerDistance = data.AttackRange;
        e.onDead += () => onEnemyDead?.Invoke(1);
    }


    // 적 스폰 일시정지 껐다켰다
    public void PauseSwitchEnemySpawn()
    {
        isPaused = !isPaused;
    }


    /// <summary>
    /// 레벨업 시 호출할 적 스폰 메서드입니다.
    /// 스폰을 멈췄다가 레벨에 맞게 추가 스폰합니다.
    /// </summary>
    /// <param name="level"></param>
    public void StartResetEnemy(int level)
    {
        if (level == 1)
        {
            return;
        }
        StartCoroutine(C_ResetForLevelUp(level));
    }

    private IEnumerator C_ResetForLevelUp(int level)
    {
        // 일시정지하기
        PauseSwitchEnemySpawn();
        Debug.Log("정지");

        yield return new WaitForSecondsRealtime(2f);

        Debug.Log("스폰 시작");

        SpawnByWave((WaveName)level);

    }

    private bool IsFull()
    {
        return EnemyManager.Instance.TotalCount >= _totalCount;
    }

    private void MoveNextTurn()
    {
        if ((int)_turnPivot == Enum.GetValues(typeof(EnemySpawnTurn)).Length)
        {
            Debug.Log(_turnPivot);
            _turnPivot = EnemySpawnTurn.mushroomTurn;
        }
        else
        {
            _turnPivot++;
        }
    }

    private void StopMushroomSpawn()
    {
        StopCoroutine(mushroomCoroutine);
        mushroomCoroutine = null;
    }

    private void StopCactusSpawn()
    {
        StopCoroutine(cactusCoroutine);
        cactusCoroutine = null;
    }

    private IEnumerator C_SpawnLoop()
    {

        while (true)
        {
            while (Time.timeScale == 0)
            {
                yield return null;
            }

            while (IsFull())
            {
                Debug.Log("꽉참" + _totalCount + " " + EnemyManager.Instance.TotalCount);
                yield return null;
            }

            if (_mushroomSpawnedCount >= mushroomSpawnInfo[0].MaxEnemyCount)
            {
                MoveNextTurn();
                _mushroomSpawnedCount = 0;
            }
            else if (_cactusSpawnedCount >= cactusSpawnInfo[0].MaxEnemyCount)
            {
                MoveNextTurn();
                _cactusSpawnedCount = 0;
            }

            //Debug.Log(_mushroomSpawnedCount + "버섯 나온 개수");
            //Debug.Log(_cactusSpawnedCount + "선인장 나온 개수");
            //Debug.Log("지금 턴 " + _turnPivot);
            //Debug.Log(EnemyManager.Instance.TotalCount);

            switch (_turnPivot)
            {
                case EnemySpawnTurn.mushroomTurn:
                    {
                        EnemyInfoData enemydata;
                        EnemySpawnInfoData spawndata;

                        enemydata = mushroomData[0];
                        spawndata = mushroomSpawnInfo[0];

                        Vector3 randomPosition = GetRandomPositionOnCircleEdge(mushroomSpawnAxis.position, spawndata.SpawnRadius);

                        yield return new WaitForSeconds(spawndata.SpawnTime);

                        GameObject newEnemy = Instantiate(spawndata.EnemyPrefab, randomPosition, Quaternion.identity);

                        EnemyInit(newEnemy, enemydata);
                        ++_mushroomSpawnedCount;
                        ++EnemyManager.Instance.TotalCount;
                        //Debug.Log(EnemyManager.Instance.TotalCount + " 총");

                        //Debug.Log("버섯 스폰");

                    }
                    break;
                case EnemySpawnTurn.cactusTurn:
                    {
                        EnemySpawnInfoData spawndata;
                        EnemyInfoData enemydata;

                        enemydata = cactusData[0];
                        spawndata = cactusSpawnInfo[0];

                        Vector3 randomPosition = GetRandomPositionOnCircleEdge(cactusSpawnAxis.position, spawndata.SpawnRadius);

                        yield return new WaitForSeconds(spawndata.SpawnTime);

                        GameObject newEnemy = Instantiate(spawndata.EnemyPrefab, randomPosition, Quaternion.identity);

                        EnemyInit(newEnemy, enemydata);
                        ++_cactusSpawnedCount;
                        ++EnemyManager.Instance.TotalCount;

                        //Debug.Log(EnemyManager.Instance.TotalCount + " 총");

                        //Debug.Log("선인장");

                    }
                    break;
                default:
                    break;
            }

            yield return null;


        }
    }

 
    /// <summary>
    /// 버섯쿤 스폰 시작 메서드입니다.
    /// </summary>
    /// <param name="wave">현재 레벨, WaveName으로 형변환</param>
    private void StartMushroomSpawn()
    {
        if (mushroomCoroutine == null)
        {
            mushroomCoroutine = StartCoroutine(C_EnemySpawn_Mushroom());
        }
    }

    /// <summary>
    /// 선인장쿤 스폰 시작 메서드입니다.
    /// </summary>
    private void StartCactusSpawn()
    {
        if (cactusCoroutine == null)
        {
            cactusCoroutine = StartCoroutine(C_EnemySpawn_Cactus());
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
                    StartCactusSpawn();
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
        if (spawnLoopCoroutine != null)
        {
            StopCoroutine(spawnLoopCoroutine);
            spawnLoopCoroutine = null;
        }

        // 맵 내에 있는 적들 모두 파괴
        GameObject[] removeList = isEnemyExistInMap();
        if (removeList != null)
        {
            foreach (var item in removeList)
            {
                //Destroy(item);
            }
        }

        _mushroomSpawnedCount = 0;
        _cactusSpawnedCount = 0;

        _totalCount = mushroomSpawnInfo[0].MaxEnemyCount + cactusSpawnInfo[0].MaxEnemyCount;
        EnemyManager.Instance.TotalCount = 0;

        _turnPivot = EnemySpawnTurn.cactusTurn;
        spawnLoopCoroutine = StartCoroutine(C_SpawnLoop());
    }

    private void StartSpawnLoop()
    {
        if (spawnLoopCoroutine == null)
        {
            StartCoroutine(C_SpawnLoop());
        }
    }


    /// <summary>
    /// 맵 내에 적들이 있다면 배열로 받아오는 메서드입니다. 
    /// </summary>
    /// <returns></returns>
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
