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
    // ������ �ݰ��� �߽���
    [SerializeField] private Transform mushroomSpawnAxis;
    [SerializeField] private Transform cactusSpawnAxis;

    private Coroutine mushroomCoroutine;
    private Coroutine cactusCoroutine;

    public System.Action<int> onEnemyDead;

    private bool isPaused = false;

    [Header("������ ������")]
    public EnemyInfoData[] mushroomData;
    [Header("������ ���� ����")]
    public EnemySpawnInfoData[] mushroomSpawnInfo;

    [Header("�������� ������")]
    public EnemyInfoData[] cactusData;
    [Header("�������� ���� ����")]
    public EnemySpawnInfoData[] cactusSpawnInfo;

    private EnemySpawnTurn _turnPivot;

    // �� ����/�� ���� ����
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
    /// �� ���� �� �����͸� �ʱ�ȭ�ϴ� �޼����Դϴ�.
    /// </summary>
    /// <param name="enemy">������ �� ��ü</param>
    /// <param name="data">������ ���� ������ scriptable object</param>
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
    /// ������ ������ ������ �ٲߴϴ�.
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
    /// �� ���� ������ ���� �ڷ�ƾ�Դϴ�. ������ ���� ���ʴ�� �����մϴ�.
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
    /// ���� �ڷ�ƾ �ߴ� �� �ʿ� �ִ� ���� ��� �ı��ϰ�, ó������ �ٽ� ������ �����մϴ�.
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

        // �� ���� �ִ� ���� ��� �ı�
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

        // �������� ���� ����
        _turnPivot = EnemySpawnTurn.mushroomTurn;
        _currentLevel = 0;
        normalSpawnCoroutine = StartCoroutine(C_NormalEnemySpawn());
        specialSpawnCoroutine = StartCoroutine(C_SpecailEnemySpawn());
    }

    /// <summary>
    /// �� ���� ������ �ִٸ� �迭�� �޾ƿ��� �޼����Դϴ�. 
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


    // �������� �޾Ƽ� ���� ���ο��� ���� ��ġ�� ����ϴ� �޼����Դϴ�.

    Vector3 GetRandomPositionOnCircleEdge(Vector3 center, float radius)
    {
        float angle = UnityEngine.Random.Range(0, 360) * Mathf.Deg2Rad; // ������ �������� ��ȯ
        float x = center.x + radius * Mathf.Cos(angle);
        float z = center.z + radius * Mathf.Sin(angle);
        return new Vector3(x, center.y, z); // �߽��� y���� �״�� ����
    }


    // �� ���� �Ͻ����� �����״�
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