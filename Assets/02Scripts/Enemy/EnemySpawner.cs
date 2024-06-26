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

    private Coroutine spawnLoopCoroutine;


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
        e.onDead += () => onEnemyDead?.Invoke(1);
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


    /// <summary>
    /// �� ���� ������ ���� �ڷ�ƾ�Դϴ�. ������ ���� ���ʴ�� �����մϴ�.
    /// </summary>
    /// <returns></returns>
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
                //Debug.Log("����" + _totalCount + " " + EnemyManager.Instance.TotalCount);
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

            //Debug.Log(_mushroomSpawnedCount + "���� ���� ����");
            //Debug.Log(_cactusSpawnedCount + "������ ���� ����");
            //Debug.Log("���� �� " + _turnPivot);
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

                        //Debug.Log("���� ����");

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

                        //Vector3 cactusRotation = cactusSpawnAxis.position - randomPosition;
                        GameObject newEnemy = Instantiate(spawndata.EnemyPrefab, randomPosition, Quaternion.identity);

                        EnemyInit(newEnemy, enemydata);
                        ++_cactusSpawnedCount;
                        ++EnemyManager.Instance.TotalCount;

                        //Debug.Log("������");
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
        if (spawnLoopCoroutine != null)
        {
            StopCoroutine(spawnLoopCoroutine);
            spawnLoopCoroutine = null;
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

        _totalCount = mushroomSpawnInfo[0].MaxEnemyCount + cactusSpawnInfo[0].MaxEnemyCount;
        EnemyManager.Instance.TotalCount = 0;

        // �������� ���� ����
        _turnPivot = EnemySpawnTurn.mushroomTurn;
        spawnLoopCoroutine = StartCoroutine(C_SpawnLoop());
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


    // �������� �޾Ƽ� ���� ���ο��� ���� ��ġ�� ����ϴ� �޼�����Դϴ�.
    private Vector3 GetRandomPositionInCircle(float radius)
    {
        float angle = UnityEngine.Random.Range(0, 360) * Mathf.Deg2Rad; // ������ �������� ��ȯ
        float r = Mathf.Sqrt(UnityEngine.Random.value) * radius; // �Ÿ��� ����
        float x = r * Mathf.Cos(angle);
        float z = r * Mathf.Sin(angle);
        return new Vector3(x, 1.0f, z); // ��ȯ�� ��ǥ�� ��ȯ
    }

    Vector3 GetRandomPositionOnCircleEdge(Vector3 center, float radius)
    {
        float angle = UnityEngine.Random.Range(0, 360) * Mathf.Deg2Rad; // ������ �������� ��ȯ
        float x = center.x + radius * Mathf.Cos(angle);
        float z = center.z + radius * Mathf.Sin(angle);
        return new Vector3(x, center.y, z); // �߽��� y���� �״�� ����
    }

    // ���̺꺰�� �� ����
    private void SpawnByWave(WaveName wave)
    {
        WaveName current = wave;

        // �Ͻ������� �簳�� ����
        PauseSwitchEnemySpawn();

        switch (current)
        {
            case WaveName.General:
                break;
            case WaveName.Trainee:
                {
                    //StartCactusSpawn();
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


    // �� ���� �Ͻ����� �����״�
    public void PauseSwitchEnemySpawn()
    {
        isPaused = !isPaused;
    }

    // ������ ���� �ڷ�ƾ�Դϴ�.
    private IEnumerator C_EnemySpawn_Mushroom()
    {
        while (true)
        {
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

    // �������� ���� �ڷ�ƾ�Դϴ�.
    private IEnumerator C_EnemySpawn_Cactus()
    {
        while (true)
        {
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
}
