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
    [Header("������ �ݰ��� �߽���")]
    [SerializeField] private Transform _enemySpawnAxis;

    private Coroutine _mushroomCoroutine;
    private Coroutine _cactusCoroutine;
    private Coroutine _normalSpawnCoroutine;
    private Coroutine _specialSpawnCoroutine;

    public System.Action<int> onEnemyDead;

    private bool _isPaused = false;

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
    public static int totalEnemyCount;

    private float _specialSpawnTime;

    private int _currentLevel = 0;


    public void ChangeLevelOfSpawn(int level)
    {
       _currentLevel = level;
    }

    private void IncreaseScore()
    {
        onEnemyDead?.Invoke(1);
    }

    /// <summary>
    /// �� ���� �� �����͸� �ʱ�ȭ�ϴ� �޼����Դϴ�.
    /// </summary>
    /// <param name="enemy">������ �� ��ü</param>
    /// <param name="data">������ ���� ������ scriptable object</param>
    private void EnemyInit(GameObject enemy, EnemyInfoData data)
    {
        Enemy newEnemy = enemy.GetComponent<Enemy>();

        //e.agent.speed = data.MoveSpeed;
        newEnemy.MoveSpeed = data.MoveSpeed;
        newEnemy.AttackForce = data.AttackForce;
        newEnemy.AttackTime = data.AttackTime;
        newEnemy.AttackSpeed = data.AttackSpeed;
        newEnemy.AttackRange = data.AttackRange;
        newEnemy.Type = data.Type;

        if (!newEnemy.isReused)
        {
            newEnemy.isReused = true;
            newEnemy.onDead += IncreaseScore;
            newEnemy.OnRequestSpawnItem += EnemyManager.Instance.itemSpawner.SpawnItemByProbability;
        }
        
    }

    /// <summary>
    /// �� Ư����ü ���� �� �����͸� �ʱ�ȭ�ϴ� �޼����Դϴ�.
    /// </summary>
    /// <param name="enemy">������ �� Ư����ü</param>
    /// <param name="data">������ ���� ������ scriptable object</param>
    private void EnemyInit_Special(GameObject enemy, EnemyInfoData data)
    {
        Enemy newEnemy = enemy.GetComponent<Enemy>();

        newEnemy.MoveSpeed = data.MoveSpeed;
        newEnemy.AttackForce = data.AttackForce;
        newEnemy.AttackTime = data.AttackTime;
        newEnemy.AttackSpeed = data.AttackSpeed;
        newEnemy.AttackRange = data.AttackRange;
        newEnemy.SpecialAttackCoolTime = data.SpecialAttackCoolTime;
        newEnemy.SpecialAttackRange = data.SpecialAttackRange;
        newEnemy.SpecialAttackTime = data.SpecialAttackTime;
        newEnemy.Type = data.Type;

        if (!newEnemy.isReused)
        {
            newEnemy.isReused = true;
            newEnemy.onDead += IncreaseScore;
            newEnemy.OnRequestSpawnItem += EnemyManager.Instance.itemSpawner.SpawnItemByProbability;
            newEnemy.OnRequestThornAttack += EnemyManager.Instance.CreateThornArea;
            newEnemy.OnRequestCloudAttack += EnemyManager.Instance.CreateCloud;
        }
        
    }


    private bool IsFull()
    {
        int maxCount = mushroomSpawnInfo[0].MaxEnemyCount + cactusSpawnInfo[0].MaxEnemyCount;
        return totalEnemyCount >= maxCount;
    }


    /// <summary>
    /// ������ ������ ������ �ٲߴϴ�.
    /// </summary>
    private void MoveNextTurn()
    {
        if ((int)_turnPivot == Enum.GetValues(typeof(EnemySpawnTurn)).Length)
        {
            _turnPivot = EnemySpawnTurn.mushroomTurn;
        }
        else
        {
            _turnPivot++;
        }
    }

    /// <summary>
    /// Ư����ü�� �����մϴ�.
    /// </summary>
    /// <returns></returns>
    private IEnumerator C_SpecailEnemySpawn()
    {
        while (true)
        {
            yield return new WaitUntil(() => _isPaused == false);

            while (Time.timeScale == 0)
            {
                yield return null;
            }

            switch (_currentLevel)
            {
                case 0:
                    {
                        _specialSpawnTime = 20.0f;

                        EnemySpawnInfoData spawndata;
                        EnemyInfoData enemydata;

                        enemydata = mushroomData[1];
                        spawndata = mushroomSpawnInfo[1];

                        Vector3 randomPosition = UtilSpawn.GetRandomPositionOnCircle(_enemySpawnAxis.position, spawndata.SpawnRadius);

                        yield return new WaitForSeconds(_specialSpawnTime);

                        GameObject newEnemy = GetNewEnemy(EPoolType.SpecialMushroom, randomPosition);

                        EnemyInit_Special(newEnemy, enemydata);
                    }
                    break;
                case 1:
                    {
                        _specialSpawnTime = 10.0f;
                        yield return new WaitForSeconds(_specialSpawnTime);

                        CreateBothSpecialEnemies();
                    }
                    break;
                case 2:
                    {
                        _specialSpawnTime = 6.0f;
                        yield return new WaitForSeconds(_specialSpawnTime);

                        CreateBothSpecialEnemies();
                    }
                    break;
                case 3:
                    {
                        _specialSpawnTime = 5.0f;
                        yield return new WaitForSeconds(_specialSpawnTime);

                        CreateBothSpecialEnemies();
                    }
                    break;
                default:
                    break;
            }

            yield return null;
        }
        
    }

    /// <summary>
    /// Ư����ü ������ �������� �� �� �Բ� �����մϴ�.
    /// </summary>
    private void CreateBothSpecialEnemies()
    {
        EnemySpawnInfoData[] spawndata = new EnemySpawnInfoData[2];
        EnemyInfoData[] enemydata = new EnemyInfoData[2];

        enemydata[0] = mushroomData[1];
        spawndata[0] = mushroomSpawnInfo[1];

        enemydata[1] = cactusData[1];
        spawndata[1] = cactusSpawnInfo[1];

        Vector3 randomPosition1 = UtilSpawn.GetRandomPositionOnCircle(_enemySpawnAxis.position, spawndata[0].SpawnRadius);
        Vector3 randomPosition2 = UtilSpawn.GetRandomPositionOnCircle(_enemySpawnAxis.position, spawndata[1].SpawnRadius);

        GameObject newEnemy_mush = GetNewEnemy(EPoolType.SpecialMushroom, randomPosition1);
        GameObject newEnemy_cac = GetNewEnemy(EPoolType.SpecialCactus, randomPosition2);

        EnemyInit_Special(newEnemy_mush, enemydata[0]);
        EnemyInit_Special(newEnemy_cac, enemydata[1]);
    }

    /// <summary>
    /// �� ���� ������ ���� �ڷ�ƾ�Դϴ�. ������ ���� ���ʴ�� �����մϴ�.
    /// </summary>
    /// <returns></returns>
    private IEnumerator C_NormalEnemySpawn()
    {
        while (true)
        {
            yield return new WaitUntil(() => _isPaused == false);

            while (Time.timeScale == 0)
            {
                yield return null;
            }

            while (IsFull())
            {
                yield return null;
            }

            // �ִ� ������ŭ �����Ǿ��ٸ� �ٸ� ������ ���� �����ϵ��� ���� ����
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

                        Vector3 randomPosition = UtilSpawn.GetRandomPositionOnCircle(_enemySpawnAxis.position, spawndata.SpawnRadius);

                        yield return new WaitForSeconds(spawndata.SpawnTime);

                        GameObject newEnemy = GetNewEnemy(EPoolType.Mushroom, randomPosition);

                        EnemyInit(newEnemy, enemydata);
                        ++_mushroomSpawnedCount;
                        ++totalEnemyCount;
                    }
                    break;
                case EnemySpawnTurn.cactusTurn:
                    {
                        EnemySpawnInfoData spawndata;
                        EnemyInfoData enemydata;

                        enemydata = cactusData[0];
                        spawndata = cactusSpawnInfo[0];

                        Vector3 randomPosition = UtilSpawn.GetRandomPositionOnCircle(_enemySpawnAxis.position, spawndata.SpawnRadius);

                        yield return new WaitForSeconds(spawndata.SpawnTime);

                        GameObject newEnemy = GetNewEnemy(EPoolType.Cactus, randomPosition);

                        EnemyInit(newEnemy, enemydata);
                        ++_cactusSpawnedCount;
                        ++totalEnemyCount;
                    }
                    break;
                default:
                    break;
            }
            yield return null;

        }
    }

    /// <summary>
    /// ���ο� �� ��ü�� �����մϴ�.
    /// </summary>
    /// <param name="poolType">������ ���� Ÿ��(������Ʈ Ǯ Ÿ��)</param>
    /// <param name="position">���� ��ġ</param>
    /// <returns></returns>
    private GameObject GetNewEnemy(EPoolType poolType, Vector3 position)
    {
        GameObject newEnemy = ObjectPoolManager.Instance.GetFromPool(poolType);
        newEnemy.transform.position = position;
        newEnemy.transform.rotation = Quaternion.identity;
        newEnemy.SetActive(true);

        return newEnemy;
    }

    /// <summary>
    /// ���� �ڷ�ƾ �ߴ� �� ó������ �ٽ� ������ �����մϴ�.
    /// </summary>
    public void InitEnemySpawnSetting()
    {
        if (_normalSpawnCoroutine != null)
        {
            StopCoroutine(_normalSpawnCoroutine);
            _normalSpawnCoroutine = null;
        }

        if (_specialSpawnCoroutine != null)
        {
            StopCoroutine(_specialSpawnCoroutine);
            _specialSpawnCoroutine = null;
        }
        
        _mushroomSpawnedCount = 0;
        _cactusSpawnedCount = 0;

        totalEnemyCount = 0;

        // �������� ���� ����
        _turnPivot = EnemySpawnTurn.mushroomTurn;
        _currentLevel = 0;
        _normalSpawnCoroutine = StartCoroutine(C_NormalEnemySpawn());
        _specialSpawnCoroutine = StartCoroutine(C_SpecailEnemySpawn());
    }

    /// <summary>
    /// �� ���� �Ͻ��������θ� �����մϴ�.
    /// </summary>
    /// <param name="isPaused"></param>
    public void PauseSwitchEnemySpawn(bool isPaused)
    {
        this._isPaused = isPaused;
    }
}