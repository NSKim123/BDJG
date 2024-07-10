using System;
using System.Collections;
using UnityEngine;

public enum EnemySpawnTurn
{
    mushroomTurn = 1,
    cactusTurn,
}


public class EnemySpawner : MonoBehaviour
{
    // 스폰될 반경의 중심점
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

    // 적 개수/총 개수 세기
    private int _mushroomSpawnedCount;
    private int _cactusSpawnedCount;
    private int _totalCount;

    public static int TotalEnemyCount;

    private float _specialSpawnTime;

    private Coroutine normalSpawnCoroutine;
    private Coroutine specialSpawnCoroutine;

    private int _currentLevel = 0;

    public void ChangeLevelOfSpawn(int level)
    {
       _currentLevel = level;
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
        e.AttackRange = data.AttackRange;
        e.Type = data.Type;
        e.onDead += () => onEnemyDead?.Invoke(1);
        e.OnRequestSpawnItem += EnemyManager.Instance.itemSpawner.ItemSpawnByPercentage;
    }


    private void EnemyInit_Special(GameObject enemy, EnemyInfoData data)
    {
        Enemy e = enemy.GetComponent<Enemy>();

        e.MoveSpeed = data.MoveSpeed;
        e.AttackForce = data.AttackForce;
        e.AttackTime = data.AttackTime;
        e.AttackSpeed = data.AttackSpeed;
        e.AttackRange = data.AttackRange;
        e.SpecialAttackCoolTime = data.SpecialAttackCoolTime;
        e.SpecialAttackRange = data.SpecialAttackRange;
        e.SpecialAttackTime = data.SpecialAttackTime;
        e.Type = data.Type;
        e.onDead += () => onEnemyDead?.Invoke(1);
        e.OnRequestSpawnItem += EnemyManager.Instance.itemSpawner.ItemSpawnByPercentage;
    }


    private bool IsFull()
    {
        int maxCount = mushroomSpawnInfo[0].MaxEnemyCount + cactusSpawnInfo[0].MaxEnemyCount;
        return TotalEnemyCount >= maxCount;
    }


    /// <summary>
    /// 스폰될 몬스터의 종류를 바꿉니다.
    /// </summary>
    private void MoveNextTurn()
    {
        if ((int)_turnPivot == Enum.GetValues(typeof(EnemySpawnTurn)).Length)
        {
            //Debug.Log(_turnPivot);
            _turnPivot = EnemySpawnTurn.mushroomTurn;
        }
        else
        {
            _turnPivot++;
        }
    }

    private IEnumerator C_SpecailEnemySpawn()
    {
        while (true)
        {
            yield return new WaitUntil(() => isPaused == false);

            while (Time.timeScale == 0)
            {
                yield return null;
            }

            //Debug.Log(_currentLevel);

            switch (_currentLevel)
            {
                case 0:
                    {
                        _specialSpawnTime = 3.0f;

                        EnemySpawnInfoData spawndata;
                        EnemyInfoData enemydata;

                        //enemydata = mushroomData[1];
                        //spawndata = mushroomSpawnInfo[1];

                        enemydata = cactusData[1];
                        spawndata = cactusSpawnInfo[1];

                        Vector3 randomPosition = GetRandomPositionOnCircleEdge(mushroomSpawnAxis.position, spawndata.SpawnRadius);

                        yield return new WaitForSeconds(_specialSpawnTime);

                        GameObject newEnemy = Instantiate(spawndata.EnemyPrefab, randomPosition, Quaternion.identity);
                        //GameObject newEnemy = ObjectPoolManager.Instance.GetFromPool(PoolType.SpecialMushroom);
                        //newEnemy.transform.position = randomPosition;
                        //newEnemy.transform.rotation = Quaternion.identity;
                        //newEnemy.SetActive(true);


                        EnemyInit_Special(newEnemy, enemydata);
                    }
                    break;
                case 1:
                    {
                        _specialSpawnTime = 10.0f;

                        EnemySpawnInfoData[] spawndata = new EnemySpawnInfoData[2];
                        EnemyInfoData[] enemydata = new EnemyInfoData[2];

                        enemydata[0] = mushroomData[1];
                        spawndata[0] = mushroomSpawnInfo[1];

                        enemydata[1] = cactusData[1];
                        spawndata[1] = cactusSpawnInfo[1];

                        Vector3 randomPosition1 = GetRandomPositionOnCircleEdge(mushroomSpawnAxis.position, spawndata[0].SpawnRadius);
                        Vector3 randomPosition2 = GetRandomPositionOnCircleEdge(mushroomSpawnAxis.position, spawndata[1].SpawnRadius);

                        yield return new WaitForSeconds(_specialSpawnTime);

                        GameObject newEnemy_mush = Instantiate(spawndata[0].EnemyPrefab, randomPosition1, Quaternion.identity);
                        GameObject newEnemy_cac = Instantiate(spawndata[1].EnemyPrefab, randomPosition2, Quaternion.identity);

                        //GameObject newEnemy_mush = ObjectPoolManager.Instance.GetFromPool(PoolType.SpecialMushroom);
                        //newEnemy_mush.transform.position = randomPosition1;
                        //newEnemy_mush.transform.rotation = Quaternion.identity;
                        //newEnemy_mush.SetActive(true);


                        //GameObject newEnemy_cac = ObjectPoolManager.Instance.GetFromPool(PoolType.SpecialCactus);
                        //newEnemy_cac.transform.position = randomPosition2;
                        //newEnemy_cac.transform.rotation = Quaternion.identity;
                        //newEnemy_cac.SetActive(true);


                        EnemyInit_Special(newEnemy_mush, enemydata[0]);
                        EnemyInit_Special(newEnemy_cac, enemydata[1]);
                    }
                    break;
                case 2:
                    {
                        _specialSpawnTime = 6.0f;

                        EnemySpawnInfoData[] spawndata = new EnemySpawnInfoData[2];
                        EnemyInfoData[] enemydata = new EnemyInfoData[2];

                        enemydata[0] = mushroomData[1];
                        spawndata[0] = mushroomSpawnInfo[1];

                        enemydata[1] = cactusData[1];
                        spawndata[1] = cactusSpawnInfo[1];

                        Vector3 randomPosition1 = GetRandomPositionOnCircleEdge(mushroomSpawnAxis.position, spawndata[0].SpawnRadius);
                        Vector3 randomPosition2 = GetRandomPositionOnCircleEdge(mushroomSpawnAxis.position, spawndata[1].SpawnRadius);

                        yield return new WaitForSeconds(_specialSpawnTime);

                        GameObject newEnemy_mush = Instantiate(spawndata[0].EnemyPrefab, randomPosition1, Quaternion.identity);
                        GameObject newEnemy_cac = Instantiate(spawndata[1].EnemyPrefab, randomPosition2, Quaternion.identity);

                        //GameObject newEnemy_mush = ObjectPoolManager.Instance.GetFromPool(PoolType.SpecialMushroom);
                        //newEnemy_mush.transform.position = randomPosition1;
                        //newEnemy_mush.transform.rotation = Quaternion.identity;
                        //newEnemy_mush.SetActive(true);


                        //GameObject newEnemy_cac = ObjectPoolManager.Instance.GetFromPool(PoolType.SpecialCactus);
                        //newEnemy_cac.transform.position = randomPosition2;
                        //newEnemy_cac.transform.rotation = Quaternion.identity;
                        //newEnemy_cac.SetActive(true);

                        EnemyInit_Special(newEnemy_mush, enemydata[0]);
                        EnemyInit_Special(newEnemy_cac, enemydata[1]);
                    }
                    break;
                case 3:
                    {
                        _specialSpawnTime = 5.0f;

                        EnemySpawnInfoData[] spawndata = new EnemySpawnInfoData[2];
                        EnemyInfoData[] enemydata = new EnemyInfoData[2];

                        enemydata[0] = mushroomData[1];
                        spawndata[0] = mushroomSpawnInfo[1];

                        enemydata[1] = cactusData[1];
                        spawndata[1] = cactusSpawnInfo[1];

                        Vector3 randomPosition1 = GetRandomPositionOnCircleEdge(mushroomSpawnAxis.position, spawndata[0].SpawnRadius);
                        Vector3 randomPosition2 = GetRandomPositionOnCircleEdge(mushroomSpawnAxis.position, spawndata[1].SpawnRadius);

                        yield return new WaitForSeconds(_specialSpawnTime);


                        GameObject newEnemy_mush = Instantiate(spawndata[0].EnemyPrefab, randomPosition1, Quaternion.identity);
                        GameObject newEnemy_cac = Instantiate(spawndata[1].EnemyPrefab, randomPosition2, Quaternion.identity);

                        //GameObject newEnemy_mush = ObjectPoolManager.Instance.GetFromPool(PoolType.SpecialMushroom);
                        //newEnemy_mush.transform.position = randomPosition1;
                        //newEnemy_mush.transform.rotation = Quaternion.identity;
                        //newEnemy_mush.SetActive(true);


                        //GameObject newEnemy_cac = ObjectPoolManager.Instance.GetFromPool(PoolType.SpecialCactus);
                        //newEnemy_cac.transform.position = randomPosition2;
                        //newEnemy_cac.transform.rotation = Quaternion.identity;
                        //newEnemy_cac.SetActive(true);

                        EnemyInit_Special(newEnemy_mush, enemydata[0]);
                        EnemyInit_Special(newEnemy_cac, enemydata[1]);
                    }
                    break;
                default:
                    break;
            }

            yield return null;
        }
        
    }

    /// <summary>
    /// 적 스폰 루프를 도는 코루틴입니다. 순서에 맞춰 차례대로 스폰합니다.
    /// </summary>
    /// <returns></returns>
    private IEnumerator C_NormalEnemySpawn()
    {
        while (true)
        {
            yield return new WaitUntil(() => isPaused == false);

            while (Time.timeScale == 0)
            {
                yield return null;
            }

            while (IsFull())
            {
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
                        //GameObject newEnemy = ObjectPoolManager.Instance.GetFromPool(PoolType.Mushroom);
                        //newEnemy.transform.position = randomPosition;
                        //newEnemy.transform.rotation = Quaternion.identity;
                        //newEnemy.SetActive(true);

                        EnemyInit(newEnemy, enemydata);
                        ++_mushroomSpawnedCount;
                        ++TotalEnemyCount;
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
                        //GameObject newEnemy = ObjectPoolManager.Instance.GetFromPool(PoolType.Cactus);
                        //newEnemy.transform.position = randomPosition;
                        //newEnemy.transform.rotation = Quaternion.identity;
                        //newEnemy.SetActive(true);

                        EnemyInit(newEnemy, enemydata);
                        ++_cactusSpawnedCount;
                        ++TotalEnemyCount;
                    }
                    break;
                default:
                    break;
            }
            yield return null;

        }
    }

    /// <summary>
    /// 생성 코루틴 중단 후 맵에 있는 적을 모두 파괴하고, 처음부터 다시 스폰을 시작합니다.
    /// </summary>
    public void ResetForRestart()
    {
        if (normalSpawnCoroutine != null)
        {
            StopCoroutine(normalSpawnCoroutine);
            normalSpawnCoroutine = null;
        }

        if (specialSpawnCoroutine != null)
        {
            StopCoroutine(specialSpawnCoroutine);
            specialSpawnCoroutine = null;
        }

        // 맵 내에 있는 적들 모두 파괴
        GameObject[] removeList = isEnemyExistInMap();
        if (removeList != null)
        {
            foreach (var item in removeList)
            {
                Destroy(item);
            }
        }

        _mushroomSpawnedCount = 0;
        _cactusSpawnedCount = 0;

        TotalEnemyCount = 0;

        // 버섯부터 스폰 시작
        _turnPivot = EnemySpawnTurn.mushroomTurn;
        _currentLevel = 0;
        normalSpawnCoroutine = StartCoroutine(C_NormalEnemySpawn());
        specialSpawnCoroutine = StartCoroutine(C_SpecailEnemySpawn());
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


    // 반지름을 받아서 원형 내부에서 랜덤 위치를 계산하는 메서드입니다.

    Vector3 GetRandomPositionOnCircleEdge(Vector3 center, float radius)
    {
        float angle = UnityEngine.Random.Range(0, 360) * Mathf.Deg2Rad; // 각도를 라디안으로 변환
        float x = center.x + radius * Mathf.Cos(angle);
        float z = center.z + radius * Mathf.Sin(angle);
        return new Vector3(x, center.y, z); // 중심의 y값을 그대로 유지
    }


    // 적 스폰 일시정지 껐다켰다
    public void PauseSwitchEnemySpawn(bool isPaused)
    {
        this.isPaused = isPaused;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawSphere(cactusSpawnAxis.position, 8);

        
    }
#endif



}