using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemySpawnTurn
{
    mushroomTurn = 1,
    cactusTurn,
}

public enum EnemySpecialSpawnTurn
{
    none,
    mushroomSpecialTurn,
    cactusSpecialTurn,
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
    private EnemySpecialSpawnTurn _specialTurnPivot;

    // �� ����/�� ���� ����
    private int _mushroomSpawnedCount;
    private int _cactusSpawnedCount;
    private int _totalCount;

    private int _mushroomSpecialSpawnedCount;
    private int _cactusSpecialSpawnedCount;

    public static int TotalEnemyCount;
    public static int MushroomSpecialCount;
    public static int CactusSpecialCount;
    public static int TotalSpecialEnemyCount;

    private Coroutine spawnLoopCoroutine;

    private bool startSpecialEnemySpawn;

    public void SwitchSpawnValue(bool spawnValue)
    {
        startSpecialEnemySpawn = spawnValue;
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

        //e.agent.speed = data.MoveSpeed;
        e.MoveSpeed = data.MoveSpeed;
        e.AttackForce = data.AttackForce;
        e.AttackTime = data.AttackTime;
        e.AttackSpeed = data.AttackSpeed;
        e.AttackRange = data.AttackRange;
        e.SpecialAttackCoolTime = data.SpecialAttackCoolTime;
        e.SpecialAttackRange = data.SpecialAttackRange;
        e.SpecialAttackTime = data.SpecialAttackTime;
        e.onDead += () => onEnemyDead?.Invoke(1);
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




    /// <summary>
    /// �� ���� ������ ���� �ڷ�ƾ�Դϴ�. ������ ���� ���ʴ�� �����մϴ�.
    /// </summary>
    /// <returns></returns>
    private IEnumerator C_SpawnLoop()
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
                        ++TotalEnemyCount;
                        //++EnemyManager.Instance.TotalCount;

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
                        ++TotalEnemyCount;

                        //++EnemyManager.Instance.TotalCount;

                        //Debug.Log("������");
                    }
                    break;
                default:
                    break;
            }


            switch (_specialTurnPivot)
            {
                case EnemySpecialSpawnTurn.none:
                    {
                        if (MushroomSpecialCount == 0)
                        {
                            _specialTurnPivot = EnemySpecialSpawnTurn.mushroomSpecialTurn;
                        }
                        else if (CactusSpecialCount == 0)
                        {
                            _specialTurnPivot = EnemySpecialSpawnTurn.cactusSpecialTurn;
                        }
                    }
                    break;
                case EnemySpecialSpawnTurn.mushroomSpecialTurn:
                    {
                        EnemyInfoData enemydata;
                        EnemySpawnInfoData spawndata;

                        enemydata = mushroomData[1];
                        spawndata = mushroomSpawnInfo[1];

                        Vector3 randomPosition = GetRandomPositionOnCircleEdge(mushroomSpawnAxis.position, spawndata.SpawnRadius);

                        float spawnTime = spawndata.SpawnTime - mushroomSpawnInfo[0].SpawnTime;

                        yield return new WaitForSeconds(spawnTime);

                        GameObject newEnemy = Instantiate(spawndata.EnemyPrefab, randomPosition, Quaternion.identity);

                        EnemyInit_Special(newEnemy, enemydata);
                        //++_mushroomSpawnedCount;
                        ++MushroomSpecialCount;
                        ++TotalSpecialEnemyCount;

                        //temp
                        _specialTurnPivot++;
                    }
                    break;
                case EnemySpecialSpawnTurn.cactusSpecialTurn:
                    {
                        EnemySpawnInfoData spawndata;
                        EnemyInfoData enemydata;

                        enemydata = cactusData[1];
                        spawndata = cactusSpawnInfo[1];

                        Vector3 randomPosition = GetRandomPositionOnCircleEdge(cactusSpawnAxis.position, spawndata.SpawnRadius);

                        float spawnTime = spawndata.SpawnTime - cactusSpawnInfo[0].SpawnTime;

                        yield return new WaitForSeconds(spawnTime);

                        //Vector3 cactusRotation = cactusSpawnAxis.position - randomPosition;
                        GameObject newEnemy = Instantiate(spawndata.EnemyPrefab, randomPosition, Quaternion.identity);

                        EnemyInit_Special(newEnemy, enemydata);
                        //++_cactusSpawnedCount;
                        ++CactusSpecialCount;
                        ++TotalSpecialEnemyCount;

                        //temp
                        _specialTurnPivot = EnemySpecialSpawnTurn.none;
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

        _mushroomSpawnedCount = 0;
        _cactusSpawnedCount = 0;

        MushroomSpecialCount = 0;
        CactusSpecialCount = 0;

        TotalEnemyCount = 0;
        TotalSpecialEnemyCount = 0;

        // �������� ���� ����
        _turnPivot = EnemySpawnTurn.mushroomTurn;
        _specialTurnPivot = EnemySpecialSpawnTurn.cactusSpecialTurn;
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